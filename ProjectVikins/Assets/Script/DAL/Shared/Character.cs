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
        public double CurrentLife { get; set; }
        public double MaxLife { get; set; }
        public double SpeedWalk { get; set; }
        public double SpeedRun { get; set; }
        public int AttackMin { get; set; }
        public int AttackMax { get; set; }
        public bool IsDead { get; set; }
        public bool IsTank { get; set; }
    }
}
