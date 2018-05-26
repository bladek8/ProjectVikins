using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Models.Shared
{
    public class KeyMove
    {
        public KeyCode KeyCode { get; set; }
        public Vector2 Vector2 { get; set; }
        public bool? Flip { get; set; }

        public KeyMove()
        {
        }
        public KeyMove(KeyCode keyCode, Vector2 vector2, bool flip)
        {
            this.KeyCode = keyCode;
            this.Vector2 = vector2;
            this.Flip = flip;
        }
        public KeyMove(KeyCode keyCode, Vector2 vector2)
        {
            this.KeyCode = keyCode;
            this.Vector2 = vector2;
        }
    }
}
