using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class MainPageManager
    {

        MainSliderSettingRepository _sliderRepo;
        MainPageSettingRepository _mainRepo;
        ImageManager _imgM;
        public MainPageManager()
        {
            _sliderRepo = new MainSliderSettingRepository();
            _imgM = new ImageManager();
            _mainRepo = new MainPageSettingRepository();
        }


        public ResponseModel AddSliderValidate(AddSliderModel slider)
        {
            ResponseModel result = new ResponseModel();

            if (slider.SliderImage == null)
            {
                result.Message = "Slider görseli boş olamaz!";
                return result;
            }
            else
            {
                var img = System.Drawing.Image.FromStream(slider.SliderImage.InputStream, true, true);
                int w = img.Width;
                int h = img.Height;

                if (w < 1910 || w > 1930 || h < 470 || h > 490)
                {
                    result.Message = "Slider görseli istenilen boyutlarda değil! (1920 - 480)";
                    return result;
                }

            }

            if (!string.IsNullOrWhiteSpace(slider.ButtonText) && string.IsNullOrWhiteSpace(slider.ButtonUrl))
            {
                result.Message = "Buton adı dolu ise yönlendirme urli dolu olmalıdır!";
                return result;
            }

            result.IsSuccess = true;
            return result;

        }

        public List<MainSliderSettings> GetMainSliders()
        {
            var items = _sliderRepo.GetAll().ToList();
            return items;
        }

        public ResponseModel CreateSliderItem(AddSliderModel new_slider)
        {
            ResponseModel result = new ResponseModel();

            result = AddSliderValidate(new_slider);

            if (!result.IsSuccess)
                return result;

            try
            {
                MainSliderSettings new_item = new MainSliderSettings();
                new_item.ButtonText = new_slider.ButtonText;
                new_item.ButtonUrl = new_slider.ButtonUrl;
                new_item.MediumTitle = new_slider.MediumTitle;
                new_item.PhotoUrl = new_slider.PhotoUrl;
                new_item.SmallTitle = new_slider.SmallTitle;
                _imgM.SaveSliderImage(new_slider.SliderImage, new_slider.SaveBaseAddress);
                int id = _sliderRepo.Insert(new_item);
                new_slider.ID = id;
                result.IsSuccess = true;
                result.Message = "Slider başarı ile eklendi!";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                throw;
            }
            return result;
        }

        public ResponseModel DeleteSlider(int slider_id)
        {
            ResponseModel result = new ResponseModel();
            try
            {
                _sliderRepo.ExecuteQuery("delete from MainSliderSettings where ID =@ID", new { ID = slider_id });
                result.IsSuccess = true;
                result.Message = "Slider başarı ile silindi!";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }

        }

        public MainPageSettings GetMainPage()
        {
            var item = _mainRepo.GetAll().FirstOrDefault();
            if (item == null)
            {
                item = CreateMainMageSettings(new MainPageSettings());
            }

            return item;

        }

        private MainPageSettings CreateMainMageSettings(MainPageSettings s)
        {
            s.ID = _mainRepo.Insert(s);
            return s;
        }

        public ResponseModel UpdateMainPageSettings(MainPageSettings s)
        {
            ResponseModel result = new ResponseModel();
            try
            {
                _mainRepo.Update(s);
                result.IsSuccess = true;
                result.Message = "Bilgiler başarı ile kaydedilmiştir!";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                throw;
            }
            return result;
        }

    }
}
