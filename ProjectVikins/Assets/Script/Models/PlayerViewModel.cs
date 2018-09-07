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
        public double CurrentLife { get; set; }
        public double MaxLife { get; set; }
        public double SpeedWalk { get; set; }
        public double SpeedRun { get; set; }
        public int AttackMin { get; set; }
        public int AttackMax { get; set; }
        public bool IsBeingControllable { get; set; }
        public PlayerModes? PlayerMode { get; set; }
        public double InitialX { get; set; }
        public double InitialY { get; set; }
        public bool ForceToWalk { get; set; }
        public bool ForceToStop { get; set; }
        public bool IsDead { get; set; }
        public bool IsTank { get; set; }
        public List<int> DirectionsDefended { get; set; }
        }
}
