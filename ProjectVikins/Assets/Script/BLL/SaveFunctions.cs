﻿using Assets.Script.DAL;
using Assets.Script.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Assets.Script.BLL
{
    public class SaveFunctions : MonoBehaviour
    {

        public static void Save()
        {
            PlayerFunctions playerFunctions = new PlayerFunctions();
            EnemyFunctions enemyFunctions = new EnemyFunctions();

            //ProjectVikingsContext.players = playerFunctions.GetDataByViewModel(ProjectVikingsContext.playerModels);
            //ProjectVikingsContext.enemies = enemyFunctions.GetDataByViewModel(ProjectVikingsContext.enemieModels);

            var currentDirectory = Directory.GetCurrentDirectory();
            var dataDirectory = Path.Combine(currentDirectory, "Save");
            var files = new DirectoryInfo(dataDirectory).GetFiles("*.dat");

            foreach (var file in files)
            {
                var fileName = file.Name.Split('.');
                var className = Type.GetType("Assets.Script.DAL." + fileName[0]);

                BinaryFormatter bf = new BinaryFormatter();
                FileStream __file = File.Create(file.FullName);
                
                //if (className == typeof(Enemy))
                //    bf.Serialize(__file, ProjectVikingsContext.enemies);
                
                __file.Close();
            }

            //GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            //foreach (var go in allObjects)
            //{
            //    object dal;
            //    var name = go.name;
            //    var goType = Type.GetType("Assets.Script.View." + name + "View");
            //    if (goType == null) continue;
            //    var goView = go.GetComponent(goType);
            //    var goDal = Type.GetType("Assets.Script.DAL." + name);
            //    if (goDal == null) continue;
            //    dal = Activator.CreateInstance(goDal);
            //    var _dal = GetDal(goType, goView);
            //    if (_dal.Dal == null) continue;

            //    string newData = "";
            //    string lineData = "";
            //    var path = Path.Combine(Directory.GetCurrentDirectory(), "teste");
            //    string currentData = "";
            //    StreamReader sReader = new StreamReader(Path.Combine(path, name + ".txt"));
            //    while (true)
            //    {
            //        lineData = sReader.ReadLine();
            //        if (lineData == null)
            //            break;
            //        currentData += DataManagement.DataManagement.Decrypt(lineData, "FelipeMae");
            //        if (lineData.Contains("\0"))
            //            break;
            //    }
            //    sReader.Close();
            //    if (!currentData.Contains(name + "Id=" + _dal.Id))
            //    {
            //        return;
            //    }
            //    int pFrom = currentData.IndexOf(name + "Id=" + _dal.Id);
            //    var partData = currentData.Substring(pFrom, currentData.Length - pFrom);
            //    int _pTo = partData.IndexOf("\n");
            //    var a = partData.Substring(0, _pTo+1);
            //    var b = currentData.Remove(pFrom, a.Length);
            //    foreach (var variable in _dal.Dal.GetType().GetProperties())
            //    {
            //        newData += variable.Name + "=" + variable.GetValue(_dal.Dal, null) + ";";
            //    }
            //    newData += "\n";
            //    StreamWriter sWriter = new StreamWriter(Path.Combine(path, name + ".txt"));
            //    sWriter.WriteLine(DataManagement.DataManagement.Encrypt(b + newData, "FelipeMae"));
            //    sWriter.Close();
            //}
        }
        private static DalDataStruct GetDal(object goType, object goView)
        {
            if (typeof(PlayerView) == goType)
            {
                var dal = (PlayerView)goView;
                return new DalDataStruct() { Dal = dal.model, Id = dal.model.PlayerId };
            }
            else if (typeof(EnemyView) == goType)
            {
                var dal = (EnemyView)goView;
                return new DalDataStruct() { Dal = dal.model, Id = dal.model.EnemyId };
            }
            return new DalDataStruct() { Dal = null, Id = 0 };
        }

        public struct DalDataStruct
        {
            public object Dal { get; set; }
            public int Id { get; set; }
        }
    }
}
