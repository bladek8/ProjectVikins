using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

namespace Assets.Script.DAL
{
    public class MVC_Game2Context
    {
        Dictionary<string, string> varTxt = new Dictionary<string, string>();
        public MVC_Game2Context()
        {
            teste();
        }
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

        //PEGAR DADOS
        public void teste()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataDirectory = Path.Combine(currentDirectory, "teste");
            var files = new DirectoryInfo(dataDirectory).GetFiles("*.txt");
            foreach (var file in files)
            {
                StreamReader stwToLaw = new StreamReader(Path.Combine(dataDirectory, file.FullName));
                var fileName = file.Name.Split('.');
                var className = Type.GetType("Assets.Script.DAL." + fileName[0]);
                var dal = Activator.CreateInstance(className);

                string b;
                b = stwToLaw.ReadToEnd();
                var c = b.Split(';');
                foreach (var d in c)
                {
                    if (!d.Contains("="))
                        continue;
                    var e = d.Split('=');
                    className.GetProperty(e[0]).SetValue(dal, int.Parse(e[1]), null);
                    varTxt.Add(e[0], e[1]);
                    Debug.Log(e[0] + " " + e[1]);
                }
                if (className == typeof(Player))
                    players.Add((Player)dal);
                else if (className == typeof(Enimy))
                    enimies.Add((Enimy)dal);
            }
        }
    }
}
