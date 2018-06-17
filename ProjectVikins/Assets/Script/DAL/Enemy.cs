using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.DAL
{
    public class Enemy : Shared.Character
    {   
        public int EnemyId { get; set; }
        public Helpers.PossibleMoviment LastMoviment { get; set; }
    }
}
