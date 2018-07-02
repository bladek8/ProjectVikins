using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.DAL
{
    public class Player : Shared.Character
    {
        public string Key = "PlayerId";
        public int PlayerId { get; set; }
        public float InitialX { get; set; }
        public float InitialY { get; set; }
        public PossibleMoviment LastMoviment { get; set; }
        public bool IsBeingControllable { get; set; }
        public PlayerModes PlayerMode { get; set; }
        public Transform Transform { get; set; }
    }
}