using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class MainMenuManager
    {

        private MainMenuRepository _menuRepo;
        public MainMenuManager()
        {
            _menuRepo = new MainMenuRepository();
        }
        public ResponseModel ValidateAddOrEditMenu(MainMenu menu)
        {
            ResponseModel result = new ResponseModel();
            if (string.IsNullOrWhiteSpace(menu.Title))
            {
                result.Message = "Menu başlığı boş olamaz!";
                return result;
            }

            if (menu.ID > 0)
            {
                var exist = _menuRepo.GetByCustomQuery("select * from MainMenu where ID != @ID and RowNumber = @Row", new { ID = menu.ID, Row = menu.RowNumber }).FirstOrDefault();

                if (exist != null)
                {
                    result.Message = "Menu sıra numarasına ait farklı bir menu mevcut! {" + menu.Title + "}";
                    return result;
                }

            }
            else
            {
                var exist = _menuRepo.GetByCustomQuery("select * from MainMenu where  RowNumber = @Row", new { ID = menu.ID, Row = menu.RowNumber }).FirstOrDefault();

                if (exist != null)
                {
                    result.Message = "Menu sıra numarasına ait farklı bir menu mevcut! {" + menu.Title + "}";
                    return result;
                }
            }

            result.IsSuccess = true;
            return result;
        }
        public List<MainMenu> GetMenuItems()
        {
            return _menuRepo.GetAll();
        }
        public ResponseModel UpdateMenuState(int id, bool state)
        {
            ResponseModel result = new ResponseModel();

            bool p = _menuRepo.ExecuteQuery("update MainMenu set IsActive = @State where ID = @ID", new { ID = id, State = state });

            if (p)
            {
                result.IsSuccess = true;
                result.Message = "Güncelleme başarılı!";
                return result;
            }
            else
            {
                result.Message = "Güncelleme hatalı!";
                return result;
            }
        }
        public ResponseModel UpdateMenu(MainMenu menu)
        {
            ResponseModel result = new ResponseModel();
            result = ValidateAddOrEditMenu(menu);
            if (!result.IsSuccess)
            {
                return result;
            }

            bool p = _menuRepo.Update(menu);
            result.IsSuccess = p;
            result.Message = "Güncelleme başarılı!";
            return result;
        }
        public ResponseModel AddMenu(MainMenu menu)
        {
            ResponseModel result = new ResponseModel();
            result = ValidateAddOrEditMenu(menu);
            if (!result.IsSuccess)
            {
                return result;
            }

            _menuRepo.Insert(menu);
            result.IsSuccess = true;
            result.Message = "Güncelleme başarılı!";
            return result;
        }
    }
}
