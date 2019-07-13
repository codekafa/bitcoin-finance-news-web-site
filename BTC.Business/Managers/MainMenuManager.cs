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
                var exist = _menuRepo.GetByCustomQuery("select * from MainMenu where  RowNumber = @Row and ParentID = @ParentID", new { ID = menu.ID, Row = menu.RowNumber, ParentID = menu.ParentID }).FirstOrDefault();

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

        public List<MainMenu> GetAllMenuItems()
        {
            return _menuRepo.GetByCustomQuery("select * from MainMenu where IsActive = 1", null);
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

        public ResponseModel AddSubPageToMenu(int menu_id, int page_id, int row_number, int type_id)
        {
            ResponseModel result = new ResponseModel();
            MainMenu parentMenu = _menuRepo.GetByID(menu_id);
            PageUrlItem item = null;
            string url = "";
            switch (type_id)
            {
                case 1:
                    item = _pagerepo.GetCategoryList(page_id).FirstOrDefault();
                    break;
                case 2:
                    item = _pagerepo.GetUserListByRoleID((int)BTC.Setting.EnumVariables.Roles.Writer, page_id).FirstOrDefault();
                    break;
                case 3:
                    item = _pagerepo.GetUserListByRoleID((int)BTC.Setting.EnumVariables.Roles.Supplier, page_id).FirstOrDefault();
                    break;
                case 4:
                    item = _pagerepo.GetPageList(page_id).FirstOrDefault();
                    break;
                case 5:
                    item = _pagerepo.GetContentList(page_id).FirstOrDefault();
                    break;
                case 6:
                    item = _pagerepo.GetPostList(page_id).FirstOrDefault();
                    break;
                case 7:
                    item = _pagerepo.GetUserListByRoleID((int)BTC.Setting.EnumVariables.Roles.Member, page_id).FirstOrDefault();
                    break;
                default:
                    item = null;
                    break;
            }

            if (item == null)
            {
                result.Message = "Sayfa bulunamadı!";
                return result;
            }

            switch (type_id)
            {
                case 1:
                    url = "/blog/kategori/" + item.Uri;
                    break;
                case 2:
                    url = "/yayinlar/yazar/" + item.Uri;
                    break;
                case 3:
                    url = "/tedarikciler/" + item.Uri;
                    break;
                case 4:
                    url = "/sayfalar/" + parentMenu.Url + "/" + item.Uri;
                    break;
                case 5:
                    url = "/icerik/" + item.Uri;
                    break;
                case 6:
                    url = "/blog-detay/" + item.Uri;
                    break;
                case 7:
                    url = "/uyeler/" + item.Uri;
                    break;
                default:
                    item = null;
                    break;
            }

            MainMenu new_menu_item = new MainMenu();
            new_menu_item.IsActive = true;
            new_menu_item.IsStatic = false;
            new_menu_item.ParentID = parentMenu.ID;
            new_menu_item.RowNumber = row_number;
            new_menu_item.Title = item.Name;
            new_menu_item.Url = url;

            result = this.AddMenu(new_menu_item);
            return result;

        }

        public List<PageUrlItem> GetCitiesMenuItems(bool is_regional)
        {
            var list = _pagerepo.GetCityPageList(is_regional);
            return list;
        }

    }
}
