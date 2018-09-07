using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Helpers;
using System;
using System.ComponentModel;

namespace Assets.Script.DAL
{
    [Serializable]
    public class Player : Shared.Character
    {
        [DisplayName("Key")]
        public int PlayerId { get; set; }
        public double InitialX { get; set; }
        public double InitialY { get; set; }
        public int LastMoviment { get; set; }
        public bool IsBeingControllable { get; set; }
        public PlayerModes PlayerMode { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}