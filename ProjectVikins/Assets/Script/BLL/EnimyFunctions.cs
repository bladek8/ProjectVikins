using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using UnityEngine;

namespace Assets.Script.BLL
{
    public class EnimyFunctions : BLL.Shared.BLLFunctions<DAL.Enimy>
    {
        public EnimyFunctions()
            : base("EnimyId")
        {
            SetListContext();
        }

        public override int Create(Enimy model)
        {
            context.SetEnimy(model);
            return model.EnimyId;
        }
        
        public override void SetListContext()
        {
            this.ListContext = context.GetEnimies();
        }
        
    }
}
