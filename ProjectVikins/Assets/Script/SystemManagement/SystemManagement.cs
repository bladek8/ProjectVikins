﻿using Assets.Script.View;
using Assets.Script.DAL;
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
        public static Dictionary<GameObject, MonoBehaviour> Scripts = new Dictionary<GameObject, MonoBehaviour>();

        public static object CallMethod(this object _class, string methodName, params object[] value)
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
    
}