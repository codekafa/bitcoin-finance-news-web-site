using BTC.Common.Constants;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Repository;
using BTC.Setting;

namespace BTC.Business.Managers
{
    public class SiteSettingManager
    {
        SiteSettingsRepository _siteRepo;
        MailSettingRepository _mailRepo;
        SmsSettingsRepository _smsRepo;
        public SiteSettingManager()
        {
            _siteRepo = new SiteSettingsRepository();
            _mailRepo = new MailSettingRepository();
            _smsRepo = new SmsSettingsRepository();
        }

        public ResponseModel ValidateUpdateSiteSettings(SiteSettings settings)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(settings.SiteTitle))
            {
                result.Message = "Site başlığı zorunludur!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(settings.SiteKeywords))
            {
                result.Message = "Site anahtar kelimeleri zorunludur!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(settings.SiteDescription))
            {
                result.Message = "Site başlığı zorunludur!";
                return result;
            }

            if (settings.SiteDescription.Length > 365)
            {
                result.Message = "Site açıklaması 365 karakterden fazla  olamaz!";
                return result;
            }


            if (string.IsNullOrWhiteSpace(settings.SiteDescription))
            {
                result.Message = "Site açıklaması zorunludur!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(settings.ComtactEmail))
            {
                result.Message = "Site iletişim maili zorunludur!";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

        public ResponseModel ValidateUpdateMailSettings(MailSettings settings)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(settings.Mail))
            {
                result.Message = "Email adresi zorunludur!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(settings.Password))
            {
                result.Message = "Email şifresi zorunludur!";
                return result;
            }

            if (settings.Port <= 0)
            {
                result.Message = "Post numarası zorunludur!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(settings.SmtpAddress))
            {
                result.Message = "SMTP adres zorunludur!";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

        public ResponseModel UpdateSiteSettings(SiteSettings updObj)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateUpdateSiteSettings(updObj);

            if (!result.IsSuccess)
            {
                return result;
            }

            bool value = _siteRepo.Update(updObj);
            result.IsSuccess = value;
            if (result.IsSuccess)
            {
                result.Message = "Bilgiler başarı ile güncellendi!";
            }


            StaticSettings.ReloadSettings();
            return result;
        }
        public ResponseModel UpdateMailSettings(MailSettings updObj)
        {
            ResponseModel result = new ResponseModel();

            result = ValidateUpdateMailSettings(updObj);

            if (!result.IsSuccess)
            {
                return result;
            }

            bool value = _mailRepo.Update(updObj);
            result.IsSuccess = value;
            if (result.IsSuccess)
            {
                result.Message = "Bilgiler başarı ile güncellendi!";
            }
            StaticSettings.ReloadSettings();
            return result;
        }
        public ResponseModel UpdateSmsSettings(SmsSettings updObj)
        {
            ResponseModel result = new ResponseModel();
            bool value = _smsRepo.Update(updObj);
            result.IsSuccess = value;
            if (result.IsSuccess)
            {
                result.Message = "Bilgiler başarı ile güncellendi!";
            }
            StaticSettings.ReloadSettings();
            return result;
        }
    }
}
