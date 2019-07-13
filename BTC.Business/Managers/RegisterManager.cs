using BTC.Common.Cryptology;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BTC.Business.Managers
{
    public class RegisterManager
    {

        UserManager _userM;
        SmsManager _smsM;
        EmailManager _emailM;
        RegisterEmailsRepository _regRepo;
        PasswordChangeMailsRepository _passRepo;
        LoginManager _lM;
        public RegisterManager()
        {
            _userM = new UserManager();
            _smsM = new SmsManager();
            _emailM = new EmailManager();
            _regRepo = new RegisterEmailsRepository();
            _lM = new LoginManager();
            _passRepo = new PasswordChangeMailsRepository();
        }
        public ResponseModel ValidateRegisterModel(RegisterUserModel registerUser)
        {
            ResponseModel result = new ResponseModel();
            result.IsSuccess = true;

            var userEmail = _userM.GetUserByEmail(registerUser.Email);

            if (registerUser.Password != registerUser.PasswordAgain)
            {
                result.IsSuccess = false;
                result.Message = "Şifre ve  şifre tekrar bilgileri aynı olmalıdır!";
                return result;
            }


            if (string.IsNullOrWhiteSpace(registerUser.Phone))
            {
                result.IsSuccess = false;
                result.Message = "Telefon numarası zorunlu alandır!";
                return result;
            }
            if (registerUser.Phone.Length != 14)
            {
                result.IsSuccess = false;
                result.Message = "Eksik yada hatalı bir telefon numarası girdiniz.Lütfen bilgilerinizi kontrol edin!";
                return result;
            }

            if (userEmail != null)
            {
                result.IsSuccess = false;
                result.Message = "Girilen bilgilere göre bir kulllanıcı hesabı mevcut.Lütfen bilgilerinizi kontrol ediniz.";
                return result;
            }
            var userPhone = _userM.GetUserByPhone(registerUser.Phone);

            if (userPhone != null)
            {
                result.IsSuccess = false;
                result.Message = "Girilen bilgilere göre bir kulllanıcı hesabı mevcut.Lütfen bilgilerinizi kontrol ediniz.";
                return result;
            }

            return result;
        }

        public ResponseModel RegisterUser(RegisterUserModel registerUser)
        {

            ResponseModel result = new ResponseModel();

            using (TransactionScope transaction = new TransactionScope())
            {

                try
                {
                    result = ValidateRegisterModel(registerUser);
                    if (!result.IsSuccess)
                        return result;

                    Users new_user = new Users();
                    new_user.CreateDate = DateTime.Now;
                    new_user.Email = registerUser.Email;
                    new_user.FirstName = registerUser.FirstName;
                    new_user.IsActive = true;
                    new_user.IsApproved = false;
                    new_user.IsVip = registerUser.IsVip;
                    new_user.LastName = registerUser.LastName;
                    new_user.Password = Encryption.GenerateMD5(registerUser.Password);
                    new_user.Phone = registerUser.Phone;
                    new_user = _userM.CreateUser(new_user);

                    var user_role_rel = _userM.CreateUserRoleRel(Setting.EnumVariables.Roles.Member, new_user.ID);

                    //email yada sms gönderimi ve approve sayfasına yönlendirme

                    if (_smsM.IsRequiredRegisterSmsSendClient())
                    {
                        // sms gönderimi
                        _smsM.SendRegisterSmsToClient(new_user.ID);
                        result.IsSuccess = true;
                        result.Message = "login-approve-sms";
                    }

                    if (_emailM.IsApproveSendMailNewUser())
                    {
                        // mail gönderimi
                        result = _emailM.SendRegisterApproveMail(new_user.ID);

                        if (result.IsSuccess)
                        {
                            result.IsSuccess = true;
                            result.Message = "login-approve-mail";
                        }
                    }

                    if (result.IsSuccess)
                        transaction.Complete();
                    else
                        transaction.Dispose();

                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                    transaction.Dispose();

                }
            }

            return result;

        }

        public ResponseModel SendApproveSmsAgain(string phone)
        {
            ResponseModel result = new ResponseModel();


            var user = _userM.GetUserByPhone(phone);

            if (user == null || user.IsApproved)
            {
                result.Message = "Kullanıcı bulunamadı!";
                return result;
            }

            var reg_list = _smsM.GetRegisterMessagesByUserPhone(phone);

            if (reg_list == null)
            {
                reg_list = new List<RegisterMessages>();
            }

            DateTime control_date = DateTime.Now.AddMinutes(-3);

            var is_res = reg_list.Where(x => x.CreateDate > control_date).FirstOrDefault();

            if (is_res != null)
            {
                TimeSpan ts = new TimeSpan();
                ts = is_res.CreateDate - control_date;
                result.Message = "Tekrar sms talep edebilmek için " + String.Format("{0:0.00}", ts.TotalMinutes) + " dakika beklemeniz gerekmektedir!";
                return result;
            }

            result = _smsM.SendRegisterSmsToClient(user.ID);
            result.Message = "Tekrar sms gönderim işlemi aşarılı!";
            result.IsSuccess = true;
            return result;

        }

        public ResponseModel ValidateApproveUserMailGuid(string guid)
        {
            ResponseModel result = new ResponseModel();
            Guid parseGuid = Guid.Parse(guid);
            var mail = _regRepo.GetByCustomQuery("select * from RegisterEmails where RegisterGuid = @Guid", new { GUid = parseGuid }).FirstOrDefault();

            if (mail == null || mail.IsUsed)
            {
                result.Message = "Yanlış istek yapıldı!";
                return result;
            }

            TimeSpan time = DateTime.Now - mail.CreateDate;
            mail.IsUsed = true;
            _regRepo.Update(mail);

            if (time.Hours > 24)
            {
                _emailM.CreateRegisterEmail(mail.UserID);
                result.Message = "İstek zaman aşımına uğradığı için bu işlemde url kullanılamaz. Üyeliğinizin onaylanması için yeni bir onaylanma linki gönderdik.Lütfen posta kutunuzu kontrol edin.";
                return result;
            }
            result.IsSuccess = true;
            return result;
        }

        public ResponseModel ValidateSendChangePasswordMailUser(string email)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(email))
            {
                result.Message = "Email alanı zorunludur!";
                return result;
            }


            var user = _userM.GetUserByEmail(email);

            if (user == null)
            {
                result.Message = "Kullanıcı bulunamadı!";
                return result;
            }

            if (!user.IsActive || !user.IsApproved)
            {
                result.Message = "Kullanıcınız pasif yada henüz doğrulanmamış!";
                return result;
            }

            var mail = _passRepo.GetByCustomQuery("select  top 1 * from PasswordChangeMails where UserID = @UserID order by ID desc", new { UserID = user.ID }).FirstOrDefault();

            if (mail == null)
            {
                result.IsSuccess = true;
                return result;
            }
            else
            {
                if (!mail.IsUsed)
                {
                    TimeSpan time = DateTime.Now - mail.CreateDate;

                    if (time.Hours > 24)
                    {
                        mail.IsUsed = true;
                        _passRepo.Update(mail);
                        _emailM.CreatePasswordChangeEmail(mail.UserID);
                        result.Message = "İstek zaman aşımına uğradığı için bu işlemde url kullanılamaz. Şifrenizi değiştirebilmeniz  için yeni bir şifre değitşirme linki gönderdik.Lütfen posta kutunuzu kontrol edin.";
                        return result;
                    }
                    else
                    {
                        result.Message = "Gün içerisinde size daha önce mail sıfırlama linki gönderilmiş.Lütfen posta kutunuzu kontrol ediniz!";
                        return result;
                    }
                }
            }
            result.IsSuccess = true;
            return result;
        }

        public ResponseModel ApproveUserMailGuid(string guid)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateApproveUserMailGuid(guid);

            if (!result.IsSuccess)
                return result;

            result = ApproveUserAndLoginToDatabase(guid);
            return result;
        }

        public ResponseModel ApproveUserAndLoginToDatabase(string register_mail_guid)
        {
            ResponseModel result = new ResponseModel();
            Guid parseGuid = Guid.Parse(register_mail_guid);
            var mail = _regRepo.GetByCustomQuery("select * from RegisterEmails where RegisterGuid = @Guid", new { GUid = parseGuid }).FirstOrDefault();

            var user = _userM.GetUserByID(mail.UserID);
            user.IsApproved = true;
            bool update_value = _userM.UpdateUser(user);
            result.IsSuccess = update_value;

            if (result.IsSuccess)
                _lM.LoginUser(user.ID);

            return result;
        }

        public ResponseModel SendChangePasswordMailGuid(string email)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateSendChangePasswordMailUser(email);

            if (!result.IsSuccess)
                return result;

            var user = _userM.GetUserByEmail(email);

            result = _emailM.SendPasswordChangeMail(user.ID);

            if (result.IsSuccess)
            {
                result.Message = "Mail adresinize şifre sıfırlama linki gönderdik.Lütfen mail adresinizi kontrol edin!";
            }

            return result;
        }

        public ResponseModel ValidateChangePasswordUserByGuid(string password_change_guid)
        {
            ResponseModel result = new ResponseModel();
            Guid parseGuid = Guid.Parse(password_change_guid);
            var mail = _passRepo.GetByCustomQuery("select * from PasswordChangeMails where ChangeGuid = @Guid", new { Guid = parseGuid }).FirstOrDefault();

            if (mail == null || mail.IsUsed)
            {
                result.Message = "Yanlış istek yapıldı!";
                return result;
            }

            TimeSpan time = DateTime.Now - mail.CreateDate;
            mail.IsUsed = true;
            _passRepo.Update(mail);

            if (time.Hours > 24)
            {
                _emailM.CreatePasswordChangeEmail(mail.UserID);
                result.Message = "İstek zaman aşımına uğradığı için bu işlemde url kullanılamaz. Üyeliğinizin onaylanması için yeni bir onaylanma linki gönderdik.Lütfen posta kutunuzu kontrol edin.";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

        public ResponseModel ValidateForgatChangePasswordModel(PasswordChangeModel obj)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(obj.Password) || string.IsNullOrWhiteSpace(obj.PasswordAgain))
            {
                result.Message = "Şifreler zorunlu alanlardır.";
                return result;
            }

            if (obj.Password.Length < 6)
            {
                result.Message = "Şifre en az 6 haneli olmalıdır!";
                return result;
            }

            if (obj.Password != obj.PasswordAgain)
            {
                result.Message = "Şifreler uyuşmamaktadır!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(obj.ProcessGuid))
            {
                result.Message = "İşlem süreçleri ile ilgili bir sorun var.";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

        public ResponseModel ValidateChangePasswordModel(PasswordChangeModel obj)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(obj.OldPassword) || string.IsNullOrWhiteSpace(obj.Password) || string.IsNullOrWhiteSpace(obj.PasswordAgain))
            {
                result.Message = "Şifreler zorunlu alanlardır.";
                return result;
            }

            var user = _userM.GetUserByID(obj.UserID);

            if (user.Password != Encryption.GenerateMD5(obj.OldPassword))
            {
                result.Message = "Mevcut şifre bilgileri yanlış!";
                return result;
            }

            if (obj.Password.Length < 6)
            {
                result.Message = "Şifre en az 6 haneli olmalıdır!";
                return result;
            }

            if (obj.Password != obj.PasswordAgain)
            {
                result.Message = "Yeni Şifreler uyuşmamaktadır!";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }
        public ResponseModel ChangeForgatPasswordUser(PasswordChangeModel obj)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateForgatChangePasswordModel(obj);

            if (!result.IsSuccess)
                return result;

            Guid parseGuid = Guid.Parse(obj.ProcessGuid);
            var mail = _passRepo.GetByCustomQuery("select * from PasswordChangeMails where ChangeGuid = @Guid", new { Guid = parseGuid }).FirstOrDefault();

            var user = _userM.GetUserByID(mail.UserID);
            user.Password = Encryption.GenerateMD5(obj.Password);
            result.IsSuccess = _userM.UpdateUser(user);

            if (!result.IsSuccess)
            {
                result.Message = "İşlemlerle ilgili  bir sorun oluştu.Sistem yöneticinize başvurun!";
                return result;
            }
            result.IsSuccess = true;
            result.Message = "Şifreniz başarı ile değiştirilmiştir.Lütfen giriş yapmayı deneyiniz!";
            return result;
        }

        public ResponseModel ChangePasswordUser(PasswordChangeModel obj)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateChangePasswordModel(obj);

            if (!result.IsSuccess)
                return result;

            var user = _userM.GetUserByID(obj.UserID);
            user.Password = Encryption.GenerateMD5(obj.Password);
            result.IsSuccess = _userM.UpdateUser(user);

            if (!result.IsSuccess)
            {
                result.Message = "İşlemlerle ilgili  bir sorun oluştu.Sistem yöneticinize başvurun!";
                return result;
            }
            result.IsSuccess = true;
            result.Message = "Şifreniz başarı ile değiştirilmiştir.";
            return result;
        }


        public ResponseModel ApproveUserBySmsCode(string phone, string sms_code)
        {
            ResponseModel result = new ResponseModel();
            try
            {
                var reg = _smsM.GetRegisterMessageBySmsCode(phone, sms_code);

                if (reg == null || reg.ID <= 0 || !reg.IsActive)
                {
                    result.Message = "Girdiğiniz üyelik doğrulama kodu hatalı!";
                    return result;
                }

                bool res_update = false;

                var user = _userM.GetUserByPhone(phone);
                user.IsApproved = true;
                reg.IsActive = false;
                res_update = _userM.UpdateUser(user) && _smsM.UpdateRegisterMessage(reg);

                if (res_update)
                    _lM.LoginUser(user.ID);
                else
                {
                    result.Message = "Veritabanı işlemleri gerçekleştirilirken hata oluştu.";
                    return result;
                }

                result.IsSuccess = true;
                result.Message = "Üyelik doğrulama işleminiz başarı ile tamamlanmıştır.";
                return result;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;

        }

    }
}
