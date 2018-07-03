using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Helpers;
using System;

namespace Assets.Script.DAL
{
    [Serializable]
    public class Player : Shared.Character
    {
        public string Key = "PlayerId";
        public int PlayerId { get; set; }
        public float InitialX { get; set; }
        public float InitialY { get; set; }
        public PossibleMoviment LastMoviment { get; set; }
        public bool IsBeingControllable { get; set; }
        public PlayerModes PlayerMode { get; set; }
        //public Vector3 Position { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}