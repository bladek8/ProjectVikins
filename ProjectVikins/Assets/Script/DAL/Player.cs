using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.DAL
{
    public class Player : Shared.Character
    {
        public Helpers.PossibleMoviment LastMoviment { get; set; }
        public int PlayerId { get; set; }

        public Player()
        {
        }

        public Player(int characterTypeId, int life, int speedWalk, int speedRun, int attackMin, int attackMax)
        {
            this.CharacterTypeId = characterTypeId;
            this.Life = life;
            this.SpeedWalk = speedWalk;
            this.SpeedRun = speedRun;
            this.AttackMin = attackMin;
            this.AttackMax = attackMax;

        }
    }
}