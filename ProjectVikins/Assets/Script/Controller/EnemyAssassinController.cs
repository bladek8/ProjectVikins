using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Models;
using UnityEngine;

namespace Assets.Script.Controller
{
    class EnemyAssassinController : Shared._CharacterController<Models.EnemyAssassinViewModel>
    {

        private readonly BLL.EnemyAssassinFunctions enemyAssassinFunctions = new BLL.EnemyAssassinFunctions();
        private readonly int id;
        private readonly System.Random rnd = new System.Random();

        public EnemyAssassinController(Models.EnemyAssassinViewModel model)
        {
            enemyAssassinFunctions.Create(model);
            this.id = model.EnemyAssassinId.Value;
        }

        public void Attack(Transform transform, Vector3 size)
        {
            throw new NotImplementedException();
        }

        public override void Decrease(EnemyAssassinViewModel model)
        {
            enemyAssassinFunctions.Decrease(model);
        }

        public override int GetDamage()
        {
            var model = enemyAssassinFunctions.GetDataById(id);
            return rnd.Next(model.AttackMin, model.AttackMax);
        }

        public override void Increase(EnemyAssassinViewModel model)
        {
            enemyAssassinFunctions.Increase(model);
        }

        public override Vector3 PositionCenterAttack(Vector3 colSize, Transform transform)
        {
            throw new NotImplementedException();
        }

        public override void UpdateStats(EnemyAssassinViewModel model)
        {
            enemyAssassinFunctions.UpdateStats(model);
        }
    }
}
