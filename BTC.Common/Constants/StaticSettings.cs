using BTC.Model.Entity;
using BTC.Model.View;
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
        private static CommentRepository _commentRepo;
        private static ContentViewRepository _contentRepo;
        static StaticSettings()
        {

        }
        private StaticSettings()
        {
            SiteSettings = new SiteSettings();
            MailSettings = new MailSettings();
            SmsSettings = new SmsSettings();
            LastComments = new List<Comments>();
            ContentViews = new List<ContentViewListModel>();
            _siteRepo = new SiteSettingsRepository();
            _mailRepo = new MailSettingRepository();
            _smsRepo = new SmsSettingsRepository();
            _commentRepo = new CommentRepository();
            _contentRepo = new ContentViewRepository();
            ReloadSettings();
        }

        public static void ReloadSettings()
        {
            SiteSettings = _siteRepo.GetAll().FirstOrDefault();
            MailSettings = _mailRepo.GetAll().FirstOrDefault();
            SmsSettings = _smsRepo.GetAll().FirstOrDefault();
            LastComments = _commentRepo.GetByCustomQuery("select * from Comments where IsPublish = 1 order by ID desc", null).ToList();
            ContentViews = _contentRepo.GetPublishedViewList();
            if (LastComments == null)
            {
                LastComments = new List<Comments>();
            }

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

            if (ContentViews == null)
            {
                ContentViews = new List<ContentViewListModel>();
            }
        }

        public static SiteSettings SiteSettings { get; private set; }

        public static List<ContentViewListModel> ContentViews { get; private set; }
        public static MailSettings MailSettings { get; private set; }

        public static SmsSettings SmsSettings { get; private set; }

        public static List<Comments> LastComments { get; private set; }
        public static StaticSettings Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
