using BTC.Model.View;
using BTC.Repository.ViewRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class SupplierManager
    {


        SupplierListModelRepository _spRepo;

        public SupplierManager()
        {
            _spRepo = new SupplierListModelRepository();
        }

        public List<SupplierListModel> GetSuppliersByCityID(int city_id)
        {
                    return _spRepo.GetByCustomQuery(@"select u.ID as [UserID], uc.ID as [CompanyID], (select COUNT(*) from UserProducts ud where ud.UserID = u.ID) as[ProductCount],
                        u.FirstName,
                        u.LastName,
                        u.Uri,
                        uc.Name ,
                        ISNULL(uc.Email,u.Email) as[Email] ,
                        ISNULL(uc.Phone,u.Phone) as[Phone], 
                        uc.Address,
                        uc.Description
                        from UserCompanies uc
                        inner join Users u on u.ID = uc.UserID
                        inner join Cities c on c.ID = uc.CityID
                        where uc.IsActive = 1 and u.IsActive = 1 and u.IsApproved = 1 and c.ID 
                         = @CityID",
                             new { CityID = city_id });
        }

        public SupplierListModel GetSupplierByUri(string uri)
        {
            return _spRepo.GetByCustomQuery(@"select u.ID as [UserID], uc.ID as [CompanyID], (select COUNT(*) from UserProducts ud where ud.UserID = u.ID) as[ProductCount],
                        u.FirstName,
                        u.LastName,
                        u.Uri,
                        uc.Name ,
                        ISNULL(uc.Email,u.Email) as[Email] ,
                        ISNULL(uc.Phone,u.Phone) as[Phone], 
                        uc.Address,
                        uc.Description
                        from UserCompanies uc
                        inner join Users u on u.ID = uc.UserID
                        inner join Cities c on c.ID = uc.CityID
                        where uc.IsActive = 1 and u.IsActive = 1 and u.IsApproved = 1 and u.Uri =@Uri",
                     new { Uri = uri }).FirstOrDefault();
        }

    }
}
