using BTC.Common.Cryptology;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Business.Managers
{
    public class UserManager
    {

        private UserRepository _userRepo;

        UserRoleRelRepository _userRoleRepo;

        ImageManager _imageM;
        public UserManager()
        {
            _userRepo = new UserRepository();
            _userRoleRepo = new UserRoleRelRepository();
            _imageM = new ImageManager();
        }
        public Users CreateUser(Users userModel)
        {
            return _userRepo.CreateUser(userModel);
        }

        public bool UpdateUser(Users userModel)
        {
            return _userRepo.Update(userModel);
        }
        public Users GetUserByID(int user_id)
        {
            return _userRepo.GetByID(user_id);
        }

        public Users GetUserByEmail(string email)
        {
            return _userRepo.GetByCustomQuery("select * from Users where Email=@Email", new { Email = email }).FirstOrDefault();
        }

        public Users GetUserByPhone(string phone)
        {
            return _userRepo.GetByCustomQuery("select * from Users where Phone=@Phone", new { Phone = phone }).FirstOrDefault();
        }
        public Users GetUserByEmailAndPassword(string email, string password)
        {
            password = Encryption.GenerateMD5(password);
            return _userRepo.GetByUserEmailAndPassword(email, password);
        }
        public CurrentUserModel GetCurrentUserModel(Users user)
        {
            CurrentUserModel result = new CurrentUserModel();
            result.CurrentUser = user;
            result.Roles = _userRoleRepo.GetByCustomQuery("select * from UserRoleRels where UserID = UserID", new { UserID = user.ID }).ToList();
            return result;
        }

        public ResponseModel ValidateUpdateProfileUser(Users user)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
            {
                result.Message = "İsim ve soy isim bilgileri zorunludur!";
                return result;
            }
            result.IsSuccess = true;
            return result;
        }
        public ResponseModel UpdateProfileUser(Users user, HttpPostedFileBase profile_photo)
        {
            ResponseModel result = ValidateUpdateProfileUser(user);
            var db_user = _userRepo.GetByID(user.ID);
            //image operations
            string profile_path = null;
            if (profile_photo != null)
                profile_path = _imageM.SaveProfileImageOrigin(profile_photo);
            else
                profile_path = db_user.ProfilePhotoUrl;

            db_user.FirstName = user.FirstName;
            db_user.LastName = user.LastName;
            db_user.Facebook = user.Facebook;
            db_user.Instagram = user.Instagram;
            db_user.Linkedin = user.Linkedin;
            db_user.ProfilePhotoUrl = profile_path;
            db_user.Summary = user.Summary;
            result.IsSuccess = _userRepo.Update(db_user);

            if (!result.IsSuccess)
                result.Message = "Profil bilgiler güncellenirken hata oluştu.";
            else
                result.Message = "Profil bilgileri başarı ile güncellenmiştir.";

            return result;
        }

    }
}
