using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEditor;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Script.DAL
{
    [InitializeOnLoad]
    public static class MVC_Game2Context
    {
        //private readonly BLL.PlayerFunctions playerFunctions = new ();

        static MVC_Game2Context()
        {

            var currentDirectory = Directory.GetCurrentDirectory();
            var dataDirectory = Path.Combine(currentDirectory, "Save");
            var files = new DirectoryInfo(dataDirectory).GetFiles("*.dat");

            foreach (var file in files)
            {
                //if (file.Name == "Enemy.dat")
                //{
                //    BinaryFormatter bf = new BinaryFormatter();
                //    FileStream __file = File.Create(file.FullName);
                //    var player = new List<Enemy>() { new Enemy() { EnemyId = 1, AttackMax = 2, AttackMin = 2, CharacterTypeId = 2, InitialX = 0, InitialY = 0, SpeedWalk = 3 }, new Enemy() { EnemyId = 2, AttackMax = 3, AttackMin = 3, CharacterTypeId = 2, InitialX = 3, InitialY = 3 } };
                //    bf.Serialize(__file, player);
                //    __file.Close();
                //}
                var fileName = file.Name.Split('.');
                var className = Type.GetType("Assets.Script.DAL." + fileName[0]);

                if (file.Directory.Exists)
                {
                    FileStream _file = File.Open(file.FullName, FileMode.Open);

                    if (className == typeof(Player))
                        players = SetList<Player>(_file);
                    if (className == typeof(Enemy))
                        enemies = SetList<Enemy>(_file);

                    _file.Close();
                }
            }
        }
        
            public static List<TEntity> SetList<TEntity>(FileStream file)
            where TEntity : class
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (List<TEntity>)bf.Deserialize(file);
        }

        public static List<CharacterType> CharactersType;

        public static List<Models.EnemyViewModel> enemieModels = new List<Models.EnemyViewModel>();
        public static List<Models.PlayerViewModel> playerModels = new List<Models.PlayerViewModel>();
        
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Player> players = new List<Player>();

        public static readonly Player defaultPlayer = new Player() { PlayerId = players.Count, PlayerMode = Helpers.PlayerModes.Follow, IsBeingControllable = false, AttackMin = 2, AttackMax = 4, LastMoviment = Helpers.PossibleMoviment.None, Life = 3, SpeedRun = 3, SpeedWalk = 4, CharacterTypeId = 1 };
        public static readonly Enemy defaultEnemy = new Enemy() { EnemyId = enemies.Count, AttackMin = 2, AttackMax = 4, LastMoviment = Helpers.PossibleMoviment.None, Life = 1, SpeedRun = 3, SpeedWalk = 4, CharacterTypeId = 1  };
    }
}
