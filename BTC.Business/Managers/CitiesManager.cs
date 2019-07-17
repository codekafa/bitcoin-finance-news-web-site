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
  
    public class CitiesManager
    {
        CitiesRepository _cityRepo;
        public CitiesManager()
        {
            _cityRepo = new CitiesRepository();
        }

        public List<Cities> GetAllCities()
        {
            return _cityRepo.GetAll().OrderBy(x=> x.Name).ToList();
        }

        public Cities GetCityByID(int city_id)
        {
            return _cityRepo.GetByID(city_id);
        }

        public Cities GetCityUri(string uri)
        {
            return _cityRepo.GetByCustomQuery("select * from Cities where Uri = @Uri", new { Uri = uri }).FirstOrDefault();
        }
        public ResponseModel AddCity(Cities city)
        {
            ResponseModel result = new ResponseModel();

            try
            {
                city.CountryID = 1;
                city.ID = _cityRepo.Insert(city);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                throw;
            }

            result.IsSuccess = true;
            result.Message = "Kaydetme işlemi başarılı.";
            return result;

        }

        public ResponseModel Update(Cities city)
        {
            ResponseModel result = new ResponseModel();

            try
            {
                city.CountryID = 1;
                _cityRepo.Update(city);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                throw;
            }

            result.IsSuccess = true;
            result.Message = "Güncelleme işlemi başarılı.";
            return result;

        }

        public ResponseModel Remove(int city_id)
        {
            ResponseModel result = new ResponseModel();

            try
            {
                var db_city = GetCityByID(city_id);
                _cityRepo.Delete(db_city);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                throw;
            }

            result.IsSuccess = true;
            result.Message = "Silme işlemi başarılı.";
            return result;

        }
    }


}
