using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL
{
    public class ItemType
    {
        [DisplayName("Key")]
        public int ItemTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExternalCode { get; set; }
    }
}
