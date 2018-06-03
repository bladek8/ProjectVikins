using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller.Shared
{
    public interface ICharacterController<TViewModel>
        where TViewModel : class
    {
        void GiveDamage(string targetFunction, int? damage, int? id);
        int GetDamage();
        object DecreaseStats(string target, string stats, object value, object id);
        object IncreaseStats(string target, string stats, object value, object id);
        void UpdateStats(string target, string stats, object value, object id);
        void UpdateMultipleStats(string target, Dictionary<string, object> datas, object id);

        void UpdateStats(TViewModel model);
        void Decrease(TViewModel model);
        void Increase(TViewModel model);
        void Attack(Transform transform, Vector3 size, LayerMask targetLayer);
        Vector3 PositionCenterAttack(Vector3 colSize, Transform transform);
    }
}
