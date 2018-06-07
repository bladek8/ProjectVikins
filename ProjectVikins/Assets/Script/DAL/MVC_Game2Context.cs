using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.DAL
{
    public class MVC_Game2Context
    {
        //public PlayerPrefs Player = new PlayerPrefs();
        //PlayerPrefs a = new PlayerPrefs();
        public static List<CharacterType> CharactersType;
        public static List<Enimy> enimies = new List<Enimy>();
        //public static List<EnimyBoss> enimyBosses = new List<EnimyBoss>();
        public static List<Player> players = new List<Player>();
        public static List<EnemyAssassin> enemyAssassins = new List<EnemyAssassin>();

        //public MVC_Game2Context()
        //{
        //    CharactersType = new List<CharacterType>
        //    {
        //        new CharacterType(1, "Player", "PLAYER"),
        //        new CharacterType(2, "SimpleEnimy", "SIMPLEENIMY")
        //    };
        //}

        public void SetEnimy(Enimy model)
        {
            enimies.Add(model);
        }
        public List<Enimy> GetEnimies()
        {
            return enimies;
        }
        public void SetEnemyAssassin(EnemyAssassin model)
        {
            enemyAssassins.Add(model);
        }
        public List<EnemyAssassin> GetEnemyAssassin()
        {
            return enemyAssassins;
        }
        //public List<EnimyBoss> GetEnimyBosses()
        //{
        //    return enimyBosses;
        //}
        public void SetPlayer(Player model)
        {
            players.Add(model);
        }
        public List<Player> GetPlayers()
        {
            return players;
        }
    }


}
