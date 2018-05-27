using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL.Shared;
using Assets.Script.DAL;

namespace Assets.Script.BLL
{
    public class EnimyBossFunctions : BLLFunctions<DAL.EnimyBoss>
    {
        public EnimyBossFunctions() 
            : base("EnimyBossId")
        {
            SetListContext();
        }

        public override int Create(EnimyBoss model)
        {
            ListContext.Add(model);
            return model.EnimyBossId;
        }
        
        public override void SetListContext()
        {
            this.ListContext = context.GetEnimyBosses();
        }
    }
}
