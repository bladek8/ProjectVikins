using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Models
{
    public class EnemyViewModel
    {
        public Helpers.PossibleMoviment? LastMoviment { get; set; }
        public int EnemyId { get; set; }
        public int? CharacterTypeId { get; set; }
        public int? Life { get; set; }
        public int? SpeedWalk { get; set; }
        public int? SpeedRun { get; set; }
        public int? AttackMin { get; set; }
        public int? AttackMax { get; set; }
        public float? InitialX { get; set; }
        public float? InitialY { get; set; }
    }
}
