﻿using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Repository
{
    public class MainMenuRepository : BaseDapperRepository<BTCConnection, MainMenu>
    {
        public ResponseModel AddSubMenuItem(MainMenu new_menu_item)
        {
            throw new NotImplementedException();
        }
    }
}
