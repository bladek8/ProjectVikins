using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using UnityEngine;

namespace Assets.Script.DAL
{
    public class ProjectVikingsContext
    {
        public static void GetSave()
        {
            List<Player> player = new List<Player>();
            List<Enemy> enemy = new List<Enemy>();
            List<HealthItem> healthItem = new List<HealthItem>();
            List<InventoryItem> InventoryItem = new List<InventoryItem>();

            var currentDirectory = Directory.GetCurrentDirectory();
            var dataDirectory = Path.Combine(currentDirectory, "Save");
            var files = new DirectoryInfo(dataDirectory).GetFiles("*.dat");

            foreach (var file in files)
            {
                if (file.Name == "Enemy.dat")
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream __file = File.Create(file.FullName);
                    enemy = new List<Enemy>() { new Enemy() { EnemyId = 1, AttackMax = 1, AttackMin = 1, CharacterTypeId = 2, InitialX = 0, InitialY = -2, SpeedWalk = 1.5f, LastMoviment = Helpers.PossibleMoviment.None, CurrentLife = 5, MaxLife = 5, SpeedRun = 2, IsDead = false }, new Enemy() { EnemyId = 2, AttackMax = 1, AttackMin = 1, CharacterTypeId = 2, InitialX = 0, InitialY = -1, SpeedWalk = 1, LastMoviment = Helpers.PossibleMoviment.None, CurrentLife = 5, MaxLife = 5, SpeedRun = 2, IsDead = false } };
                    bf.Serialize(__file, enemy);
                    __file.Close();
                }
                if (file.Name == "Player.dat")
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream __file = File.Create(file.FullName);
                    player = new List<Player>() { new Player() { PlayerId = 1, AttackMax = 5, AttackMin = 2, CharacterTypeId = 1, InitialX = -27, InitialY = 0, SpeedWalk = 1.5f, SpeedRun = 2, IsBeingControllable = true, LastMoviment = Helpers.PossibleMoviment.None, CurrentLife = 50, MaxLife = 50, PlayerMode = Helpers.PlayerModes.Follow, X = -27, Y = 0, IsDead = false }, new Player() { PlayerId = 2, AttackMax = 5, AttackMin = 2, CharacterTypeId = 1, InitialX = 1.15f, InitialY = -25, SpeedWalk = 3, SpeedRun = 2, IsBeingControllable = false, LastMoviment = Helpers.PossibleMoviment.None, CurrentLife = 5, MaxLife = 5, PlayerMode = Helpers.PlayerModes.Follow, X = -25, Y = 3, IsDead = false }, new Player() { PlayerId = 3, AttackMax = 2, AttackMin = 1, CharacterTypeId = 1, InitialX = -25, InitialY = 0, SpeedWalk = 3, SpeedRun = 2, IsBeingControllable = false, LastMoviment = Helpers.PossibleMoviment.None, CurrentLife = 5, MaxLife = 5, PlayerMode = Helpers.PlayerModes.Follow, X = -25, Y = 0, IsDead = false } };
                    bf.Serialize(__file, player);
                    __file.Close();
                }
                if (file.Name == "HealthItem.dat")
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream __file = File.Create(file.FullName);
                    healthItem = new List<HealthItem>() { new HealthItem() { ItemId = 1, Name = "Coconut", Health = 1, Amount = 1, ItemTypeId = (int)ItemTypes.HealthItem } };
                    bf.Serialize(__file, healthItem);
                    __file.Close();
                }
                if (file.Name == "InventoryItem.dat")
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream __file = File.Create(file.FullName);
                    InventoryItem = new List<InventoryItem>() { new InventoryItem() {InventoryItemId = 1,  ItemId = 1, Amount = 1, ItemTypeId = (int)ItemTypes.HealthItem } };
                    bf.Serialize(__file, InventoryItem);
                    __file.Close();
                }
                var fileName = file.Name.Split('.');
                var className = Type.GetType("Assets.Script.DAL." + fileName[0]);

                if (file.Directory.Exists)
                {
                    FileStream _file = File.Open(file.FullName, FileMode.Open);

                    if (className == typeof(Player))
                        players = SetList<Player>(_file);
                    if (className == typeof(Enemy))
                        enemies = SetList<Enemy>(_file);
                    if (className == typeof(HealthItem))
                        HealthItens = SetList<HealthItem>(_file);
                    if (className == typeof(InventoryItem))
                        InventoryItens = SetList<InventoryItem>(_file);

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
        public static TEntity Set<TEntity>(FileStream file)
        where TEntity : class
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (TEntity)bf.Deserialize(file);
        }

        public static void UpdateAliveLists()
        {
            aliveEnemies = enemieModels.Where(x => !x.IsDead).Select(x => x.GameObject).ToList();
            alivePlayers = playerModels.Where(x => !x.IsDead).Select(x => x.GameObject).ToList();
        }

        #region [Players/Enemies]
        public static List<CharacterType> CharactersType;

        public static List<Models.EnemyViewModel> enemieModels = new List<Models.EnemyViewModel>();
        public static List<GameObject> aliveEnemies = new List<GameObject>();
        public static List<Models.PlayerViewModel> playerModels = new List<Models.PlayerViewModel>();
        public static List<GameObject> alivePlayers = new List<GameObject>();
        
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Player> players = new List<Player>();
        #endregion

        #region [Inventory]
        public static List<HealthItem> HealthItens = new List<DAL.HealthItem>();
        public static List<InventoryItem> InventoryItens = new List<DAL.InventoryItem>();
        #endregion

        #region [DefaultValues]
        public static readonly Player defaultPlayer = new Player() { PlayerId = players.Count, PlayerMode = Helpers.PlayerModes.Follow, IsBeingControllable = false, AttackMin = 2, AttackMax = 4, LastMoviment = Helpers.PossibleMoviment.None, CurrentLife = 3, MaxLife = 3, SpeedRun = 2, SpeedWalk = 2, CharacterTypeId = 1, IsDead = false };
        public static readonly Enemy defaultEnemy = new Enemy() { EnemyId = enemies.Count, AttackMin = 2, AttackMax = 4, LastMoviment = Helpers.PossibleMoviment.None, CurrentLife = 10, MaxLife = 10, SpeedRun = 2, SpeedWalk = 2, CharacterTypeId = 1, IsDead = false };
        public static readonly HealthItem defaultHealthItem = new HealthItem() { ItemId = HealthItens.Count, ItemTypeId = (int)ItemTypes.HealthItem, Health = 2, Amount = 1, Name = "Coconut" };
        #endregion
    }
}
