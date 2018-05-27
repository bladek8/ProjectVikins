using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller.Shared
{
    public abstract class CharacterController : MonoBehaviour, ICharacterController
    {
        System.Random rnd = new System.Random();
        Type className;
        object instatiatedFunction;

        public CharacterController(string function)
        {
            string item = "Assets.Script.BLL." + function;
            className = Type.GetType(item);
            instatiatedFunction = Activator.CreateInstance(className);
        }

        public void GiveDamage(Component script, int damage)
        {
            script.SendMessage("TakeDamage", damage);
        }

        public int GetDamage(int minDamage, int maxDamage)
        {
            return rnd.Next(minDamage, maxDamage);
        }

        public object DecreaseStats(string stats, object value, object id)
        {
            MethodInfo m = className.GetMethod("DecreaseStats");
            return m.Invoke(instatiatedFunction, new object[] { stats, value, id });
        }
        public object IncreaseStats(string stats, object value, object id)
        {
            MethodInfo m = className.GetMethod("IncreaseStats");
            return m.Invoke(instatiatedFunction, new object[] { stats, value, id });
        }
        public void UpdateStats(string stats, object value, object id)
        {
            MethodInfo m = className.GetMethod("UpdateStats");
            m.Invoke(instatiatedFunction, new object[] { stats, value, id });
        }
        public void UpdateMultipleStats(Dictionary<string, object> datas, object id)
        {
            MethodInfo m = className.GetMethod("UpdateMultipleStats");
            m.Invoke(instatiatedFunction, new object[] { datas, id });
        }
    }
}