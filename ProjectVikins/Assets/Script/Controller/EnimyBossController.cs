using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Controller
{
    public class EnimyBossController : Shared.CharacterController
    {
        private readonly BLL.EnimyBossFunctions enimyBossFunctions;
        private readonly int id;

        public EnimyBossController(DAL.EnimyBoss model)
            : base("EnimyBossFunctions")
        {
            enimyBossFunctions = new BLL.EnimyBossFunctions();
            enimyBossFunctions.Create(model);
            this.id = model.EnimyBossId;
        }
        
    }
}
