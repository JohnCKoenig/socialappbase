using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvShare.Models
{
    class PostResponseModel
    {
        public DateTime PostDateTime { get; set; }
        public string PostText { get; set; }
        public byte[] PostImage { get; set; }
    }
}
