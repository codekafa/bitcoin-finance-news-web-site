using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using BTC.Repository.ViewRepository;
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
        StaticPageRepository _staticRepo;
        PageUrlItemRepository _pagerepo;
        public MainMenuManager()
        {
            _menuRepo = new MainMenuRepository();
            _staticRepo = new StaticPageRepository();
            _pagerepo = new PageUrlItemRepository();
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
                var exist = _menuRepo.GetByCustomQuery("select * from MainMenu where ID != @ID and RowNumber = @Row and ParentID = @ParentID", new { ID = menu.ID, Row = menu.RowNumber, ParentID = menu.ParentID }).FirstOrDefault();

                if (exist != null)
                {
                    result.Message = "Menu sıra numarasına ait farklı bir menu mevcut! {" + menu.Title + "}";
                    return result;
                }

            }
            else
            {
                var exist = _menuRepo.GetByCustomQuery("select * from MainMenu where  RowNumber = @Row and ParentID = 0", new { ID = menu.ID, Row = menu.RowNumber }).FirstOrDefault();

                if (exist != null)
                {
                    result.Message = "Menu sıra numarasına ait farklı bir menu mevcut! {" + menu.Title + "}";
                    return result;
                }
            }

            result.IsSuccess = true;
            return result;
        }
        public List<MainMenu> GetParentMenuItems()
        {
            return _menuRepo.GetByCustomQuery("select * from MainMenu where ParentId is null or ParentId = 0", null);
        }
        public List<MainMenu> GetSubMenuItems(int parent_id)
        {
            return _menuRepo.GetByCustomQuery("select * from MainMenu where ParentID = @ParentID", new { ParentID = parent_id });
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

        public ResponseModel AddOrEditSubMenu(AddOrEditSubMenuModel menuItem)
        {
            throw new NotImplementedException();
        }

        public List<StaticPages> GetStaticPages()
        {
            return _staticRepo.GetAll();
        }

        public ResponseModel AddStaticPageToMenu(int menu_id, int page_id, int row_number)
        {
            ResponseModel result = new ResponseModel();

            try
            {
                var sstatic_menu = _staticRepo.GetByID(page_id);
                _menuRepo.Insert(new MainMenu { IsActive = true, ParentID = menu_id, Title = sstatic_menu.Name, Url = sstatic_menu.Uri, IsStatic = true, RowNumber = row_number });
                result.IsSuccess = true;
                result.Message = "Menü başarı ile eklendi!";

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;

        }

        public ResponseModel DeleteSubMenuItem(int menu_id)
        {
            ResponseModel result = new ResponseModel();

            try
            {
                _menuRepo.ExecuteQuery("delete from MainMenu where ID = @ID", new { ID = menu_id });
                result.IsSuccess = true;
                result.Message = "Menü başarı ile silindi!";

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;

        }


        public List<PageUrlItem> GetPageUrl(int type_id)
        {
            switch (type_id)
            {
                case 1:
                    return _pagerepo.GetCategoryList();
                case 2:
                    return _pagerepo.GetUserListByRoleID((int)BTC.Setting.EnumVariables.Roles.Writer);
                case 3:
                    return _pagerepo.GetUserListByRoleID((int)BTC.Setting.EnumVariables.Roles.Supplier);
                case 4:
                    return _pagerepo.GetPageList();
                case 5:
                    return _pagerepo.GetContentList();
                case 6:
                    return _pagerepo.GetPostList();
                case 7:
                    return _pagerepo.GetUserListByRoleID((int)BTC.Setting.EnumVariables.Roles.Member);
                default:
                    return new List<PageUrlItem>();
            }
        }

    }
}
