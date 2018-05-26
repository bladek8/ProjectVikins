using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Controller
{
    public class EnimyBossController : Shared.CharacterController<BLL.EnimyBossFunctions>
    {
        private readonly BLL.EnimyBossFunctions enimyBossFunctions;
        private readonly int id;

        public EnimyBossController(int enimyBossId, int characterTypeId, int life, int speedWalk, int speedRun, int attackMin, int attackMax)
        {
            enimyBossFunctions = new BLL.EnimyBossFunctions(enimyBossId, characterTypeId, life, speedWalk, speedRun, attackMin, attackMax);
            this.id = enimyBossId;
        }

        public void MudarVelocidade(int novavelocidade)
        {
            enimyBossFunctions.UpdateStats("SpeedWalk", novavelocidade, id);
        }

        public override void SetFunction()
        {
            this._TFuncitons = enimyBossFunctions;
        }
    }
}
