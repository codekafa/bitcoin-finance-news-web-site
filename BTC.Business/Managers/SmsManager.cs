using BTC.Common.Cryptology;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Repository;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class SmsManager
    {
        SmsSettingsRepository smsRepo;
        RegisterMessageRepository regRepo;
        UserManager userM = new UserManager();
        public SmsManager()
        {
            smsRepo = new SmsSettingsRepository();
            regRepo = new RegisterMessageRepository();
        }
        public bool IsRequiredRegisterSmsSendClient()
        {
            var smsSettings = smsRepo.GetAll().FirstOrDefault();
            return smsSettings.RequiredIsRegister;
        }
        public bool IsRequiredChangePasswordSmsSendClient()
        {
            var smsSettings = smsRepo.GetAll().FirstOrDefault();
            return smsSettings.RequiredIsChangePassword;
        }
        public bool IsRequiredForgotPasswordSmsSendClient()
        {
            var smsSettings = smsRepo.GetAll().FirstOrDefault();
            return smsSettings.RequiredIsForgatPassword;
        }
        public RegisterMessages CreateRegisterMessage(int user_id, string message, string message_code)
        {
            RegisterMessages new_message = new RegisterMessages();
            new_message.UserID = user_id;
            new_message.IsActive = true;
            new_message.MessageBody = message;
            new_message.MessageCode = new_message.MessageBody;
            new_message.CreateDate = DateTime.Now;
            new_message.ID = regRepo.Insert(new_message);
            return new_message;
        }
        public ResponseModel SendRegisterSmsToClient(int user_id)
        {
            ResponseModel result = new ResponseModel();
            string message_value = Encryption.GenerateSixDigitNumber();
            var regMessage = CreateRegisterMessage(user_id, message_value, null);
            var user = userM.GetUserByID(user_id);
            result = SendSms(message_value, user.Phone);

            if (!result.IsSuccess)
                result.Message = "İşlemler gerçekleştirilirken bir sorun oluştu.Lütfen daha sonra tekrar deneyin.";

            return result;
        }
        public ResponseModel SendSms(string message, string phone)
        {
            ResponseModel result = new ResponseModel();
            result.IsSuccess = true;
            return result;
        }


    }
}

