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
        public ResponseModel UpdateSiteSettings(SiteSettings updObj)
        {
            ResponseModel result = new ResponseModel();
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
