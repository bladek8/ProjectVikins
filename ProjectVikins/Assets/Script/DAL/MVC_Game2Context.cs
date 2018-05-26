using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Script.DAL
{
    public class MVC_Game2Context
    {
        public List<CharacterType> CharactersType;
        public List<Enimy> enimies = new List<Enimy>();
        public List<EnimyBoss> enimyBosses = new List<EnimyBoss>();
        public List<Player> players = new List<Player>();

        public MVC_Game2Context()
        {
            CharactersType = new List<CharacterType>
            {
                new CharacterType(1, "Player", "PLAYER"),
                new CharacterType(2, "SimpleEnimy", "SIMPLEENIMY")
            };
        }
    }


}
