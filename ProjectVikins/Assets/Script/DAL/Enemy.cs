using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.DAL
{
    [Serializable]
    public class Enemy : Shared.Character
    {
        [DisplayName("Key")]
        public int EnemyId { get; set; }
        public int LastMoviment { get; set; }
        public double InitialX { get; set; }
        public double InitialY { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
