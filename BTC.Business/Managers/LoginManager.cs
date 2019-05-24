using BTC.Common.Cryptology;
using BTC.Common.Session;
using BTC.Common.Util;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{


    public class LoginManager
    {
        UserManager _userM;
        public LoginManager()
        {
            _userM = new UserManager();
        }

        public ResponseModel LoginUser(LoginUserModel user)
        {

            var db_user = _userM.GetUserByEmailAndPassword(user.UserName, user.Password);
            ResponseModel result = new ResponseModel();
            result.IsSuccess = false;

            if (db_user != null && db_user.IsActive && db_user.IsApproved)
            {
                var currentUserModel = _userM.GetCurrentUserModel(db_user);
                SessionVariables.SetUser(currentUserModel);
                var do_user_log = UserBrowserHelper.getUserRequestInfo();
                DoLogLoginUser(do_user_log);
                result.IsSuccess = true;
            }
            else
            {
                result.Message = "Kullanıcı adı yada şifreniz yanlış!";
            }

            return result;
        }

        public ResponseModel LoginUser(int user_id)
        {
            ResponseModel result = new ResponseModel();
            var db_user = _userM.GetUserByID(user_id);
            var currentUserModel = _userM.GetCurrentUserModel(db_user);
            SessionVariables.SetUser(currentUserModel);
            var do_user_log = UserBrowserHelper.getUserRequestInfo();
            DoLogLoginUser(do_user_log);
            result.IsSuccess = true;
            return result;
        }

        public ResponseModel ResetSessionUser(int user_id)
        {
            ResponseModel result = new ResponseModel();
            var db_user = _userM.GetUserByID(user_id);
            var currentUserModel = _userM.GetCurrentUserModel(db_user);
            SessionVariables.SetUser(currentUserModel);
            result.IsSuccess = true;
            return result;
        }
        public void DoLogLoginUser(LoginAttempts log)
        {
            LoginAttemptsRepository lRepo = new LoginAttemptsRepository();
            lRepo.Insert(log);
        }

        public ResponseModel LogoutUser()
        {
            ResponseModel result = new ResponseModel();
            SessionVariables.RemoveAll();
            result.IsSuccess = true;
            return result;
        }

    }
}
