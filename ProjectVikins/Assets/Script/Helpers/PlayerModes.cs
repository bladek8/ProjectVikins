using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helpers
{
    public enum PlayerModes
    {
        Attack = 1,
        Wait = 2,
        Follow = 3
    }

    public class _PlayerModes
    {
        public PlayerModes Value { get; set; }
        public List<KeyCode> KeyButton { get; set; }

        public _PlayerModes(PlayerModes Value, List<KeyCode> KeyButton)
        {
            this.Value = Value;
            this.KeyButton = KeyButton;
        }
    }

}