using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEditor;
using System.Reflection;

namespace Assets.Script.DAL
{
    [InitializeOnLoad]
    public static class MVC_Game2Context
    {

        static MVC_Game2Context()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataDirectory = Path.Combine(currentDirectory, "teste");
            var files = new DirectoryInfo(dataDirectory).GetFiles("*.txt");

            //StreamWriter sWriter = new StreamWriter(Path.Combine("C:\\Desenvolvimento\\github.com-UpsideDownHub\\ProjectVikins\\ProjectVikins\\teste", "Player.txt"));

            //sWriter.Write(DataManagement.DataManagement.Encrypt("Life=3;SpeedWalk=0;PlayerId=1;InitialX=0;InitialY=0;AttackMax=10;", "FelipeMae"));

            foreach (var file in files)
            {
                StreamReader stwToLaw = new StreamReader(Path.Combine(dataDirectory, file.FullName));
                var fileName = file.Name.Split('.');
                var className = Type.GetType("Assets.Script.DAL." + fileName[0]);
                object dal;

                string b;
                b = stwToLaw.ReadToEnd();
                try
                {
                    b = DataManagement.DataManagement.Decrypt(b, "FelipeMae");
                    var f = b.Split(new[] { "\n" }, StringSplitOptions.None);
                    foreach (var o in f)
                    {
                        dal = Activator.CreateInstance(className);
                        PropertyInfo[] classeInfo = className.GetProperties();
                        if (string.IsNullOrEmpty(o))
                            continue;
                        var c = o.Split(';');
                        foreach (var d in c)
                        {
                            if (!d.Contains("="))
                                continue;
                            var e = d.Split('=');
                            var l = classeInfo.Where(x => x.Name == e[0]).First();
                            if (l.PropertyType.IsEnum)
                                className.GetProperty(e[0]).SetValue(dal, Enum.Parse(l.PropertyType, e[1]), null);
                            else
                                className.GetProperty(e[0]).SetValue(dal, Convert.ChangeType(e[1], l.PropertyType), null);
                        }
                        if (className == typeof(Player))
                            players.Add((Player)dal);
                        else if (className == typeof(Enemy))
                            enemies.Add((Enemy)dal);
                    }
                }
                catch
                {
                    Debug.Log("Save Corrompido.");
                }
                finally
                {
                    stwToLaw.Close();
                }
            }
        }

        public static List<CharacterType> CharactersType;
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Player> players = new List<Player>();
        public static List<EnemyAssassin> enemyAssassins = new List<EnemyAssassin>();

        public static readonly Player defaultPlayer = new Player() { PlayerId = players.Count, PlayerMode = Helpers.PlayerModes.Follow, IsBeingControllable = false, AttackMin = 2, AttackMax = 4, LastMoviment = Helpers.PossibleMoviment.None, Life = 3, SpeedRun = 3, SpeedWalk = 4, CharacterTypeId = 1 };
        public static readonly Enemy defaultEnemy = new Enemy() { EnemyId = enemies.Count, AttackMin = 2, AttackMax = 4, LastMoviment = Helpers.PossibleMoviment.None, Life = 1, SpeedRun = 3, SpeedWalk = 4, CharacterTypeId = 1  };
    }
}
