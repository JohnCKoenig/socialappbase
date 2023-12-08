using MobileApp.API.GeneralResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    internal class GeneralResponseModel<T>
    {
        public ResponseCode Code { get; set; }
        public string CodeDescription { get; set; }
        public DateTime time = DateTime.Now;
        public T Data { get; set; }
    }
}
