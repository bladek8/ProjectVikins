using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Models
{
    public class EnemyViewModel
    {
        public Helpers.PossibleMoviment? LastMoviment { get; set; }
        public int EnemyId { get; set; }
        public int? CharacterTypeId { get; set; }
        public float? CurrentLife { get; set; }
        public float? MaxLife { get; set; }
        public float? SpeedWalk { get; set; }
        public float? SpeedRun { get; set; }
        public int? AttackMin { get; set; }
        public int? AttackMax { get; set; }
        public float? InitialX { get; set; }
        public float? InitialY { get; set; }
        public GameObject GameObject { get; set; }
        public bool IsDead { get; set; }
        public bool PrefToBeAttacked { get; set; }
    }
}
