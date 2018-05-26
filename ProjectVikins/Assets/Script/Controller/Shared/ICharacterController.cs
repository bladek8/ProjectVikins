using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller.Shared
{
    public interface ICharacterController
    {
        void GiveDamage(Component script, int damage);
        int GetDamage(int minDamage, int maxDamage);
        void DecreaseStatus(string stats, object value, object id);
        void IncreaseStatus(string stats, object value, object id);
        void UpdateStatus(string stats, object value, object id);
        void UpdateMultipleStatus(Dictionary<string, object> datas, object id);
    }
}
