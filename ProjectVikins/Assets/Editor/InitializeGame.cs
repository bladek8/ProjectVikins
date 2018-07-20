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
            Script.DAL.ProjectVikingsContext.GetSave();
        }
    }
}
