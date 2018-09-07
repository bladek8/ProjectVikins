using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using UnityEngine;
using Assets.Script.Helpers.Shared;
using Assets.Script.Models;

namespace Assets.Script.DAL
{
    public class ProjectVikingsContext
    {

        public static Sql<Player> Player = new Sql<Player>();
        public static Sql<Enemy> Enemy = new Sql<Enemy>();
        public static Sql<CharacterType> CharacterType = new Sql<CharacterType>();
        public static Sql<InventoryItem> InventoryItem = new Sql<InventoryItem>();
        public static Sql<ItemType> ItemTypes = new Sql<ItemType>();
        public static Sql<Item> Item = new Sql<Item>();

        public static void GetSave()
        {
            ItemTypes.GetData();
            Item.GetData();
            Player.GetData();
            Enemy.GetData();
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
            aliveEnemies = enemieModels.Entity.Where(x => !x.IsDead).Select(x => x.GameObject).ToList();
            alivePlayers = playerModels.Entity.Where(x => !x.IsDead).Select(x => x.GameObject).ToList();
            alivePrefPlayers = playerModels.Entity.Where(x => !x.IsDead && x.IsTank).Select(x => x.GameObject).ToList();
            alivePrefEnemies = enemieModels.Entity.Where(x => !x.IsDead && x.IsTank).Select(x => x.GameObject).ToList();
        }

        #region [Players/Enemies]
        public static List<CharacterType> CharactersType;

        public static ViewComponentViewModel<EnemyViewModel> enemieModels = new ViewComponentViewModel<EnemyViewModel> { Entity = new List<EnemyViewModel>() };
        public static List<GameObject> aliveEnemies = new List<GameObject>();
        public static List<GameObject> alivePrefEnemies = new List<GameObject>();
        public static ViewComponentViewModel<PlayerViewModel> playerModels = new ViewComponentViewModel<PlayerViewModel> { Entity = new List<PlayerViewModel>() };
        public static List<GameObject> alivePlayers = new List<GameObject>();
        public static List<GameObject> alivePrefPlayers = new List<GameObject>();
        #endregion

        #region [Inventory]
        public static ViewComponentViewModel<ItemViewModel> ItemViewModel = new ViewComponentViewModel<ItemViewModel> { Entity = new List<ItemViewModel>()};
        public static ViewComponentViewModel<InventoryItem> InventoryItens = new ViewComponentViewModel<InventoryItem> { Entity = new List<InventoryItem>()};
        #endregion
        
        public class ViewComponentViewModel<TEntity> where TEntity : class
        {
            public List<TEntity> _entity;
            public List<TEntity> Entity { get { return _entity; } set { _entity = value; } }
        }
    }
}
