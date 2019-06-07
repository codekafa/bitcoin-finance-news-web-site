using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class MainMenu : IEntity
    {
        public int ID { get; set; }
        public int RowNumber { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }

        public bool IsStatic { get; set; }
    }
}
