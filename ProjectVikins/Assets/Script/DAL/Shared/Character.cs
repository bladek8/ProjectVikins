using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL.Shared
{
    [Serializable]
    public class Character
    {
        public int CharacterTypeId { get; set; }
        public int Life { get; set; }
        public float SpeedWalk { get; set; }
        public float SpeedRun { get; set; }
        public int AttackMin { get; set; }
        public int AttackMax { get; set; }
    }
}
