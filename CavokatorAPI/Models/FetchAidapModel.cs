using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CavokatorAPI.Models
{
    public class FetchAidapModel
    {
        public string uid { get; set; }
        public string password { get; set; }
        public string active { get; set; }
        public string type { get; set; }
        public string location_id { get; set; }
    }
}