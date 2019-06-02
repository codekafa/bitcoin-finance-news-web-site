using BTC.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class ContactModel
    {

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return "Adı: " + Name + " Firma: " + CompanyName + " Email: " + Email + " Telefon: " + Phone + " Mesaj: " + Message;
        }

        public ResponseModel IsValid()
        {
            ResponseModel result = new ResponseModel();
            if (string.IsNullOrEmpty(Name))
            {
                result.Message = "İsim alanı zorunludur!";
                return result;
            }
            if (string.IsNullOrEmpty(Phone))
            {
                result.Message = "Telefon alanı zorunludur!";
                return result;
            }
            if (string.IsNullOrEmpty(Message))
            {
                result.Message = "Mesaj alanı zorunludur!";
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

    }
}
