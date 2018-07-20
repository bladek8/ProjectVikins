using Assets.Script.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Script.SystemManagement
{
    public static class SystemManagement
    {
        public static List<ArcherView> ArcherView = new List<ArcherView>();
        public static List<CamTriggerView> CamTriggerView = new List<View.CamTriggerView>();
        public static List<EnemyArcherView> EnemyArcherView = new List<View.EnemyArcherView>();
        public static List<EnemyView> EnemyView = new List<View.EnemyView>();
        public static List<EnemyWarriorView> EnemyWarriorView = new List<View.EnemyWarriorView>();
        public static List<NPCView> NPCView = new List<View.NPCView>();
        public static List<PlayerView> PlayerView = new List<View.PlayerView>();
        public static List<ScenaryObjectView> ScenaryObjectView = new List<View.ScenaryObjectView>();
        public static List<SeagullView> SeagullView = new List<View.SeagullView>();

        public static Classes<EnemyWarriorView, EnemyArcherView> Enemies = new Classes<EnemyWarriorView, EnemyArcherView>
        {
            Class1 = EnemyWarriorView,
            Class2 = EnemyArcherView
        };


        public static object CallMethod(object _class, string methodName, object value)
        {
            try
            {
                object[] obj = new object[] { value };
                if (_class == null) return null;

                Type classType = _class.GetType();
                MethodInfo Method = classType.GetMethod(methodName);
                return Method.Invoke(_class, obj);
            }
            catch
            {
                return null;
            }
        }

        public static object CallMethod(object _class, string methodName, object[] value)
        {
            try
            {
                if (_class == null) return null;

                Type classType = _class.GetType();
                MethodInfo Method = classType.GetMethod(methodName);
                return Method.Invoke(_class, value);
            }
            catch
            {
                return null;
            }
        }
    }

    public class Classes<TClass1, TClass2, TClass3, TClass4>
    where TClass1 : class
    where TClass2 : class
    where TClass3 : class
    where TClass4 : class
    {
        public TClass1 Class1;
        public TClass2 Class2;
        public TClass3 Class3;
        public TClass4 Class4;
    }

    public class Classes<TClass1, TClass2, TClass3>
    where TClass1 : class
    where TClass2 : class
    where TClass3 : class
    {
        public TClass1 Class1;
        public TClass2 Class2;
        public TClass3 Class3;
    }

    public class Classes<TClass1, TClass2>
    where TClass1 : class
    where TClass2 : class
    {
        public List<TClass1> Class1;
        public List<TClass2> Class2;

        public bool Exists(object _class)
        {
            if (_class.GetType() == typeof(TClass1))
            {
                foreach (var _class1 in Class1)
                    if (_class1 == (TClass1)_class)
                        return true;
            }
            else if (_class.GetType() == typeof(TClass2))
            {
                foreach (var _class2 in Class2)
                    if (_class2 == (TClass2)_class)
                        return true;
            }
            return false;

        }

        public object GetClass(object _class)
        {
            if (_class.GetType() == typeof(TClass1))
            {
                foreach (var _class1 in Class1)
                    if (_class1 == (TClass1)_class)
                        return _class1;
            }
            else if (_class.GetType() == typeof(TClass2))
            {
                foreach (var _class2 in Class2)
                    if (_class2 == (TClass2)_class)
                        return _class2;
            }
            return null;
        }
    }

    public class Classes<TClass1>
    where TClass1 : class
    {
        public TClass1 Class;

        public static TClass _GetClass<TClass>(List<TClass> _classes, TClass _class)
        where TClass : class
        {
            foreach (var _class1 in _classes)
                if (_class1 == _class)
                    return _class1;
            return null;
        }
    }
}