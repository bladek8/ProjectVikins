using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Helpers;
using UnityEngine;

namespace Assets.Script.Models
{
    public class PlayerViewModel
    {
        public GameObject GameObject { get; set; }
        public PossibleMoviment? LastMoviment { get; set; }
        public int PlayerId { get; set; }
        public int? CharacterTypeId { get; set; }
        public int Life { get; set; }
        public float SpeedWalk { get; set; }
        public float SpeedRun { get; set; }
        public int AttackMin { get; set; }
        public int AttackMax { get; set; }
        public bool IsBeingControllable { get; set; }
        public PlayerModes? PlayerMode { get; set; }
        public float InitialX { get; set; }
        public float InitialY { get; set; }
        public bool ForceToWalk { get; set; }
        public bool ForceToStop { get; set; }
        public bool IsDead { get; set; }
    }
}
