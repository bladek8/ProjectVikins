using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Helpers
{
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
    public enum PossibleMoviment
    {
        None,
        Left,
        Right,
        Up,
        Down,
        Up_Left,
        Up_Right,
        Down_Left,
        Down_Right
    };
}
