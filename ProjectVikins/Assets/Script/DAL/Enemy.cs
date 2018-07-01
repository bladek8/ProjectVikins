using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.DAL
{
    public class Enemy : Shared.Character
    {
        public string Key = "EnemyId";
        public int EnemyId { get; set; }
        public Helpers.PossibleMoviment LastMoviment { get; set; }
        public float InitialX { get; set; }
        public float InitialY { get; set; }
    }
}
