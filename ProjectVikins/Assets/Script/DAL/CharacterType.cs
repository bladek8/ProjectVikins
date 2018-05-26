using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL
{
    public class CharacterType
    {
        public int CharacterTypeId { get; set; }
        public string Name { get; set; }
        public string ExternalCode { get; set; }

        public CharacterType()
        {
        }

        public CharacterType(int CharacterTypeId, string Name, string ExternalCode)
        {
            this.CharacterTypeId = CharacterTypeId;
            this.Name = Name;
            this.ExternalCode = ExternalCode;
        }
    }
}
