using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL
{
    [Serializable]
    public class HealthItem : Shared.Item
    {
        public float Health { get; set; }
    }
}
