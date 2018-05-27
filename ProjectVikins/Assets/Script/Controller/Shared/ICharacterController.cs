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
        object DecreaseStats(string stats, object value, object id);
        object IncreaseStats(string stats, object value, object id);
        void UpdateStats(string stats, object value, object id);
        void UpdateMultipleStats(Dictionary<string, object> datas, object id);
    }
}
