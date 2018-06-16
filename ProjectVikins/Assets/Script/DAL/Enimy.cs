using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.DAL
{
    public class Enimy : Shared.Character
    {   
        public int EnimyId { get; set; }
        public Helpers.PossibleMoviment LastMoviment { get; set; }
    }
}
