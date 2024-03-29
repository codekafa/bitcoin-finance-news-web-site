﻿using BTC.Model.Entity;
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
    public  class StaticSettings
    {
        private static readonly StaticSettings instance = new StaticSettings();

        private static SiteSettingsRepository _siteRepo;
        private static MailSettingRepository _mailRepo;
        private static SmsSettingsRepository _smsRepo;
        private static CommentRepository _commentRepo;
        private static ContentViewRepository _contentRepo;
        private static UserRepository _userRepo;
        private static CategoryRepository _catRepo;
        private static StaticPageRepository _staticRepo;
        private static MainPageSettingRepository _mainRepo;
        private static MainSliderSettingRepository _sliderRepo;
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
            Writers = new List<Users>();
            StaticPages = new List<StaticPages>();
            Categories = new List<Categories>();
            _catRepo = new CategoryRepository();
            _staticRepo = new StaticPageRepository();
            _siteRepo = new SiteSettingsRepository();
            _mailRepo = new MailSettingRepository();
            _smsRepo = new SmsSettingsRepository();
            _commentRepo = new CommentRepository();
            _contentRepo = new ContentViewRepository();
            _userRepo = new UserRepository();
            _mainRepo = new MainPageSettingRepository();
            _sliderRepo = new MainSliderSettingRepository();
            ReloadSettings();
        }

        public static void ReloadSettings()
        {
            SiteSettings = _siteRepo.GetAll().FirstOrDefault();
            MailSettings = _mailRepo.GetAll().FirstOrDefault();
            SmsSettings = _smsRepo.GetAll().FirstOrDefault();
            LastComments = _commentRepo.GetByCustomQuery("select * from Comments where IsPublish = 1 order by ID desc", null).ToList();
            ContentViews = _contentRepo.GetPublishedViewList();
            Writers = _userRepo.GetByCustomQuery("select * from Users where (select COUNT(*) from UserRoleRels ur where ur.RoleID = 2) > 0 and IsActive = 1 and IsApproved = 1", null).ToList();

            Categories = _catRepo.GetByCustomQuery("select * from Categories where IsActive = 1", null).ToList();

            StaticPages = _staticRepo.GetAll();

            MainPageSetting = _mainRepo.GetAll().FirstOrDefault();

            MainPageSliders = _sliderRepo.GetAll();

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

            if (Writers == null)
            {
                Writers = new List<Users>();
            }

            if (Categories == null)
            {
                Categories = new List<Categories>();
            }

            if (StaticPages == null)
            {
                StaticPages = new List<StaticPages>();
            }

            if (MainPageSetting == null)
            {
                MainPageSetting = new MainPageSettings();
            }

            if (MainPageSliders == null)
            {
                MainPageSliders = new List<MainSliderSettings>();
            }

        }
        public static List<MainSliderSettings> MainPageSliders { get; set; }
        public static MainPageSettings MainPageSetting { get; private set; }
        public static SiteSettings SiteSettings { get; private set; }
        public static List<ContentViewListModel> ContentViews { get; private set; }
        public static MailSettings MailSettings { get; private set; }
        public static SmsSettings SmsSettings { get; private set; }
        public static List<Comments> LastComments { get; private set; }
        public static List<Users> Writers { get; private set; }
        public static List<StaticPages> StaticPages { get; private set; }
        public static List<Categories> Categories { get; private set; }
        public static StaticSettings Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
