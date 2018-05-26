using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller.Shared
{
    public abstract class CharacterController<TFunctions> : MonoBehaviour, ICharacterController
        where TFunctions : MonoBehaviour
    {
        protected TFunctions _TFuncitons;
        
        System.Random rnd = new System.Random();

        public void GiveDamage(Component script, int damage)
        {
            script.SendMessage("TakeDamage", damage);
        }

        public int GetDamage(int minDamage, int maxDamage)
        {
            return rnd.Next(minDamage, maxDamage);
        }

        public void DecreaseStatus(string stats, object value, object id)
        {
            _TFuncitons.SendMessage("DecreaseStats", new { stats, value, id });
        }
        public void IncreaseStatus(string stats, object value, object id)
        {
            _TFuncitons.SendMessage("IncreaseStatus", new { stats, value, id });
        }
        public void UpdateStatus(string stats, object value, object id)
        {
            _TFuncitons.SendMessage("UpdateStatus", new { stats, value, id });
        }
        public void UpdateMultipleStatus(Dictionary<string, object> datas, object id)
        {
            _TFuncitons.SendMessage("UpdateMultipleStatus", new { datas, id });
        }

        public abstract void SetFunction();
    }
}