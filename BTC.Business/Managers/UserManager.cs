using BTC.Common.Constants;
using BTC.Common.Cryptology;
using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using BTC.Repository.Connection;
using BTC.Setting;
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

        ContentViewManager _contentM;

        ImageManager _imageM;
        public UserManager()
        {
            _userRepo = new UserRepository();
            _userRoleRepo = new UserRoleRelRepository();
            _imageM = new ImageManager();
            _contentM = new ContentViewManager();
        }
        public Users CreateUser(Users userModel)
        {
            return _userRepo.CreateUser(userModel);
        }

        public List<Users> GetAllUsers()
        {
            return _userRepo.GetAll();
        }

        public bool UpdateUser(Users userModel)
        {
            return _userRepo.Update(userModel);
        }
        public Users GetUserByID(int user_id)
        {
            return _userRepo.GetByID(user_id);
        }

        public ResponseModel UpdateUserIsActiveField(int user_id, bool state)
        {
            ResponseModel result = new ResponseModel();

            bool p = _userRepo.ExecuteQuery("update Users set IsActive = @State where ID = @ID", new { ID = user_id, State = state });

            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarı ile gerçekleşti!";
            }

            return result;
        }

        public ResponseModel UpdateUserIsVipField(int user_id, bool state)
        {
            ResponseModel result = new ResponseModel();

            bool p = _userRepo.ExecuteQuery("update Users set IsVip = @State where ID = @ID", new { ID = user_id, State = state });

            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarı ile gerçekleşti!";
            }

            return result;
        }

        public ResponseModel UpdateUserIsApproveField(int user_id, bool state)
        {
            ResponseModel result = new ResponseModel();

            bool p = _userRepo.ExecuteQuery("update Users set IsApproved = @State where ID = @ID", new { ID = user_id, State = state });

            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarı ile gerçekleşti!";
            }

            return result;
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
            result.Roles = _userRoleRepo.GetByCustomQuery("select * from UserRoleRels where UserID = @UserID", new { UserID = user.ID }).ToList();

            foreach (var item in StaticSettings.ContentViews)
            {
                if (result.CurrentUser.IsVip && item.CanSeeVip)
                {
                    result.ContentViews.Add(item);
                    continue;
                }

                if (result.IsMember && item.CanSeeMember)
                {
                    result.ContentViews.Add(item);
                    continue;
                }

                if (result.IsWriter && item.CanSeeWriter)
                {
                    result.ContentViews.Add(item);
                    continue;
                }

                if (result.IsSupplier && item.CanSeeTrader)
                {
                    result.ContentViews.Add(item);
                    continue;
                }

                if (result.IsAdmin)
                {
                    result.ContentViews.Add(item);
                    continue;
                }
            }


            return result;
        }

        public ResponseModel ValidateUpdateProfileUser(UpdateUserModel user)
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
        public ResponseModel UpdateProfileUser(UpdateUserModel user)
        {
            ResponseModel result = ValidateUpdateProfileUser(user);

            if (!result.IsSuccess)
                return result;

            var db_user = _userRepo.GetByID(user.ID);

            //image operations
            if (user.ProfilePhotoUrl != null)
            {
                _imageM.SaveProfileImageOrigin(user.ProfilePhotoUrl, user.PhotoSaveBaseUrl);
                _imageM.RemoveOldUserProfileImages(db_user.ProfilePhotoUrl);
            }

            db_user.FirstName = user.FirstName;
            db_user.LastName = user.LastName;
            db_user.Facebook = user.Facebook;
            db_user.Instagram = user.Instagram;
            db_user.Linkedin = user.Linkedin;
            db_user.ProfilePhotoUrl = user.PhotoName;
            db_user.Summary = user.Summary;
            result.IsSuccess = _userRepo.Update(db_user);

            if (!result.IsSuccess)
                result.Message = "Profil bilgiler güncellenirken hata oluştu.";
            else
                result.Message = "Profil bilgileri başarı ile güncellenmiştir.";

            return result;
        }

        public UserRoleRels CreateUserRoleRel(EnumVariables.Roles roleType, int user_id)
        {
            UserRoleRels result = new UserRoleRels();
            result.ID = _userRoleRepo.Insert(new UserRoleRels() { RoleID = (int)roleType, UserID = user_id });
            return result;
        }

        public ResponseModel RemoveUserRoleRel(UserRoleRels rel)
        {
            ResponseModel result = new ResponseModel();
            result.IsSuccess = _userRoleRepo.Delete(rel); ;
            return result;
        }


        public List<UserSelectListModel> GetSelectedUsers(int role_id)
        {
            var viewRepo = new BaseDapperRepository<BTCConnection, UserSelectListModel>();
            var list = viewRepo.GetByCustomQuery(@"
                            select u.FirstName + ' ' + u.LastName as [FullName] , u.ID as [ID] from Users u
                            inner join UserRoleRels ur on ur.UserID = u.ID
                            where ur.RoleID = @RoleID", new { RoleID = role_id }).ToList();
            return list;
        }
    }
}
