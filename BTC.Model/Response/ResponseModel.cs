using System;

namespace BTC.Model.Response
{
    public class ResponseModel
    {

        public ResponseModel()
        {
            IsSuccess = false;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public dynamic ResultData { get; set; }

    }
}
