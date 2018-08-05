using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Models
{
    public class StrenghtItemViewModel
    {
        public int ItemId { get; set; }
        public DAL.ItemTypes ItemTypeId { get; set; }
        public string Name { get; set; }
        public int Strenght { get; set; }
        public int Amount { get; set; }
        public float InitialX { get; set; }
        public float InitialY { get; set; }
        public GameObject Prefab { get; set; }
        public Sprite Icon { get; set; }
    }
}
