using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Helpers
{
    [Serializable]
    public class Psm
    {
        public PossibleMoviment PossibleMoviment { get; set; }
        public bool Flip { get; set; }
        public Psm(PossibleMoviment possibleMoviment, bool flip)
        {
            this.PossibleMoviment = possibleMoviment;
            this.Flip = flip;
        }
    }
    [Serializable]
    public enum PossibleMoviment
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        Up_Left = 5,
        Up_Right = 6,
        Down_Left = 7,
        Down_Right = 8
    };
}
