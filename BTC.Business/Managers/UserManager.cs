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

        UserRepository _userRepo;
        UserRoleRelRepository _userRoleRepo;
        ImageManager _imageM;
        UserCompanyRepository _companyRepo;
        //LoginManager _lM;
        public UserManager()
        {
            _userRepo = new UserRepository();
            _userRoleRepo = new UserRoleRelRepository();
            _imageM = new ImageManager();
            _companyRepo = new UserCompanyRepository();
            //_lM = new LoginManager();
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

        public ResponseModel UpdateUserRole(int user_id, bool state, int role_id)
        {
            ResponseModel result = new ResponseModel();
            try
            {
                if (state)
                {
                    UserRoleRels new_role = new UserRoleRels();
                    new_role.RoleID = role_id;
                    new_role.UserID = user_id;
                    _userRoleRepo.Insert(new_role);
                }
                else
                {
                    _userRoleRepo.ExecuteQuery("delete from UserRoleRels where UserID = @UserID and RoleID = @RoleID", new { UserID = user_id, RoleID = role_id });
                }

                result.Message = "Güncelleme başarı ile gerçekleşti!";
                result.IsSuccess = true;
                return result;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
           

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
            result.Company = _companyRepo.GetCompanyByUserId(user.ID);

            if (result.Company == null)
            {
                result.Company = new UserCompanies();
            }

            //foreach (var item in StaticSettings.ContentViews)
            //{
            //    if (result.CurrentUser.IsVip && item.CanSeeVip)
            //    {
            //        result.ContentViews.Add(item);
            //        continue;
            //    }

            //    if (result.IsMember && item.CanSeeMember)
            //    {
            //        result.ContentViews.Add(item);
            //        continue;
            //    }

            //    if (result.IsWriter && item.CanSeeWriter)
            //    {
            //        result.ContentViews.Add(item);
            //        continue;
            //    }

            //    if (result.IsSupplier && item.CanSeeTrader)
            //    {
            //        result.ContentViews.Add(item);
            //        continue;
            //    }

            //    if (result.IsAdmin)
            //    {
            //        result.ContentViews.Add(item);
            //        continue;
            //    }
            //}


            return result;
        }

        public List<Users> GetUserWithRoleId(int role_id)
        {
            var list = _userRepo.GetByCustomQuery(@"select u.FirstName,u.LastName,u.ID,u.Facebook,u.Instagram,u.Email,u.IsActive,u.IsApproved,u.Linkedin,u.IsVip from Users u
                inner join UserRoleRels ur on ur.UserId = u.Id
                where ur.RoleID = @RoleID
                group by  u.FirstName,u.LastName,u.ID,u.Facebook,u.Instagram,u.Email,u.IsActive,u.IsApproved,u.Linkedin,u.IsVip 
                ", new { RoleId = role_id });
            return list;
        }

        public List<UserListModel> GetUserListModel(string search_key = null)
        {
            List<UserListModel> list = new List<UserListModel>();

            var u_repo = new BaseDapperRepository<BTCConnection, UserListModel>();

            if (!string.IsNullOrWhiteSpace(search_key))
            {
                search_key = "%" + search_key + "%";
            }
            else
            {
                search_key = null;
            }

            list = u_repo.GetByCustomQuery(@"select 
                                    u.Email,
                                    u.FirstName + ' ' + u.LastName as [FullName],
                                    u.Phone,
                                    u.ID,
                                    u.IsActive,
                                    u.IsApproved,
                                    u.IsVip,
                                    u.CreateDate,
                                    (select COUNT(*) from UserRoleRels ur where ur.UserID = u.ID and ur.RoleID = 1) as [IsAdmin],
                                    (select COUNT(*) from UserRoleRels ur where ur.UserID = u.ID and ur.RoleID = 2) as [IsWriter],
                                    (select COUNT(*) from UserRoleRels ur where ur.UserID = u.ID and ur.RoleID = 3) as [IsMember],
                                    (select COUNT(*) from UserRoleRels ur where ur.UserID = u.ID and ur.RoleID = 4) as [IsSupplier]
                                    from Users u where (@Key is null or (u.Email like @Key or u.FirstName like @Key or u.LastName like @Key or u.Email like @Key))", new { Key = search_key }).ToList();

            return list;

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


            if (result.IsSuccess)
            {
                var company = _companyRepo.GetByCustomQuery("select * from UserCompanies where UserID = @UserID", new { UserID = user.ID }).FirstOrDefault();

                if (company == null)
                    company = new UserCompanies();

                company.IsActive = true;
                company.Name = user.CompanyName;
                company.Phone = user.CompanyPhone;
                company.Address = user.CompanyAddress;
                company.CityID = user.CompanyCity;
                company.Email = user.CompanyEmail;
                company.UserID = user.ID;
                company.UpdateDate = DateTime.Now;
                company.Description = user.CompanyDescription;


                if (company.ID > 0)
                {
                    _companyRepo.ExecuteQuery("update UserCompanies set Name = @Name, Description = @Description, Phone = @Phone, Email = @Email , Address = @Address, CityID = @CityID , UpdateDate = GETDATE()", company);
                }
                else
                {
                    user.CompanyID = _companyRepo.Insert(company);
                }
                result.ResultData = user;
                result.Message = "Profil bilgileri başarı ile güncellenmiştir.";
                new LoginManager().ResetSessionUser(user.ID);


            }
            else
                result.Message = "Profil bilgileri güncellenirken hata oluştu.";


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

        public UserCompanies GetUserCompanyByUserID(int user_id)
        {
            var iten = _companyRepo.GetByCustomQuery("select * from UserCompanies where UserID = @UserID", new { UserID = user_id }).FirstOrDefault();
            return iten;
        }
    }
}
