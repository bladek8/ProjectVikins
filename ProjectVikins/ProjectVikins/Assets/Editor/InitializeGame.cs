using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Editor
{
    [InitializeOnLoad]
    static class InitializeGame
    {
        static InitializeGame()
        {
            Assets.Script.DAL.MVC_Game2Context.GetSave();
        }
    }
}
