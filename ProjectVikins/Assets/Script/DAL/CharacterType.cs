﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL
{
    public class CharacterType
    {
        [DisplayName("Key")]
        public int CharacterTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExternalCode { get; set; }
    }
}
