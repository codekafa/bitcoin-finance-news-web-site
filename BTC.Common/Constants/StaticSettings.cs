using BTC.Model.Entity;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Common.Constants
{
    public sealed class StaticSettings
    {
        private static readonly StaticSettings instance = new StaticSettings();

        private static SiteSettingsRepository _siteRepo;
        private static MailSettingRepository _mailRepo;
        private static SmsSettingsRepository _smsRepo;
        static StaticSettings()
        {

        }
        private StaticSettings()
        {
            SiteSettings = new SiteSettings();
            MailSettings = new MailSettings();
            SmsSettings = new SmsSettings();
            _siteRepo = new SiteSettingsRepository();
            _mailRepo = new MailSettingRepository();
            _smsRepo = new SmsSettingsRepository();
            ReloadSettings();
        }

        public static void ReloadSettings()
        {
            SiteSettings = _siteRepo.GetAll().FirstOrDefault();
            MailSettings = _mailRepo.GetAll().FirstOrDefault();
            SmsSettings = _smsRepo.GetAll().FirstOrDefault();

            if (SiteSettings == null)
            {
                SiteSettings = new SiteSettings();
            }
            if (MailSettings == null)
            {
                MailSettings = new MailSettings();
            }
            if (SmsSettings == null)
            {
                SmsSettings = new SmsSettings();
            }
        }

        public static SiteSettings SiteSettings { get; private set; }

        public static MailSettings MailSettings { get; private set; }

        public static SmsSettings SmsSettings { get; private set; }
        public static StaticSettings Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
