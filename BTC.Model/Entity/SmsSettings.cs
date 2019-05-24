using BTC.Core.Base.Model;

namespace BTC.Model.Entity
{
    public class SmsSettings : IEntity
    {

        public int ID { get; set; }

        public bool RequiredIsRegister { get; set; }

        public bool RequiredIsChangePassword { get; set; }

        public bool RequiredIsForgatPassword { get; set; }

    }
}
