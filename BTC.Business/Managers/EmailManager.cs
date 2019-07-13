using BTC.Common.Cryptology;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace BTC.Business.Managers
{
    public class EmailManager
    {
        MailSettingRepository _mailRepo;
        RegisterEmailsRepository _regRepo;
        PasswordChangeMailsRepository _passRepo;
        UserRepository _userRepo;
        public EmailManager()
        {
            _mailRepo = new MailSettingRepository();
            _regRepo = new RegisterEmailsRepository();
            _userRepo = new UserRepository();
            _passRepo = new PasswordChangeMailsRepository();
        }

        public bool IsApproveSendMailNewUser()
        {
            var st = _mailRepo.GetAll().FirstOrDefault();
            return st.ApproveSendMailNewUser;
        }

        public RegisterEmails CreateRegisterEmail(int user_id)
        {
            RegisterEmails mail = new RegisterEmails();
            mail.UserID = user_id;
            string d_value = Encryption.CreateDateTimeUniqeID();
            string uri_value = Encryption.GenerateMD5(d_value);
            mail.RegisterUrl = WebConfigurationManager.AppSettings["ApproveMailAddress"].ToString() + uri_value;
            mail.RegisterGuid = Guid.Parse(uri_value);
            mail.IsUsed = false;
            mail.CreateDate = DateTime.Now;
            mail.ID = _regRepo.Insert(mail);
            return mail;
        }

        public ResponseModel SendRegisterApproveMail(int user_id)
        {
            ResponseModel result = new ResponseModel();
            result.IsSuccess = true;
            var mailLog = CreateRegisterEmail(user_id);
            var user = _userRepo.GetByID(user_id);

            string path = HttpContext.Current.Server.MapPath("~/Content/Templates/VerifyEmail.html");
            string content = System.IO.File.ReadAllText(path);

            content = content.Replace("$verify-email-url", mailLog.RegisterUrl);

            result = SendMail("BTC Finans - Üyelik Onaylama", new string[] { user.Email }, content);
            return result;
        }

        public ResponseModel SendMail(string subject, string[] to, string body)
        {
            ResponseModel result = new ResponseModel();

            var emailSetting = _mailRepo.GetAll().FirstOrDefault();

            MailMessage email = new MailMessage();
            email.From = new MailAddress(emailSetting.Mail,"Bitcoin Finans");

            foreach (var item in to)
            {
                email.To.Add(item.ToString());
            }
            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body = body;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(emailSetting.Mail, emailSetting.Password);
            smtp.Port = emailSetting.Port;
            smtp.Host = emailSetting.SmtpAddress;
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(email);
                result.IsSuccess = true;
                result.Message = "Email başarı le gönderildi.";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Email gönderilirken bir sorun oluştu. Hata Detay : " + ex.Message;
            }
            return result;
        }

        public PasswordChangeMails CreatePasswordChangeEmail(int user_id)
        {
            PasswordChangeMails mail = new PasswordChangeMails();
            mail.UserID = user_id;
            string d_value = Encryption.CreateDateTimeUniqeID();
            string uri_value = Encryption.GenerateMD5(d_value);
            mail.ChangeUrl = WebConfigurationManager.AppSettings["ChangePasswordMailAddress"].ToString() + uri_value;
            mail.ChangeGuid = Guid.Parse(uri_value);
            mail.IsUsed = false;
            mail.CreateDate = DateTime.Now;
            mail.ID = _passRepo.Insert(mail);
            return mail;
        }

        public ResponseModel SendPasswordChangeMail(int user_id)
        {
            ResponseModel result = new ResponseModel();

            var user = _userRepo.GetByID(user_id);

            if (user != null)
            {
                result.IsSuccess = true;
                var mailLog = CreatePasswordChangeEmail(user.ID);
                result = SendMail("BTC Finans - Şifre Değiştirme", new string[] { user.Email }, mailLog.ChangeUrl);
                return result;
            }
            else
            {
                result.Message = "Kullanıcı bulunamadı!";
            }

            return result;

        }

    }
}
