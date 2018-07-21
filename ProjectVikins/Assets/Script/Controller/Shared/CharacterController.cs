using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Script.Controller.Shared
{
    public abstract class _CharacterController<TViewModel> : ICharacterController<TViewModel>
        where TViewModel : class
    {
        public System.Random rnd = new System.Random();
        public List<int> targetsAttacked = new List<int>();
        Type className;
        
        public Helpers.PossibleMoviment GetDirection(Vector3 position, Vector3 targetPosition, bool InDegrees = false)
        {
            var vectorDirection = targetPosition - position;
            var degrees = Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg;
            var _position = (int)((Mathf.Round(degrees / 45f) + 8) % 8);

            switch (_position)
            {
                case 0:
                    return Helpers.PossibleMoviment.Right;
                case 1:
                    return Helpers.PossibleMoviment.Up_Right;
                case 2:
                    return Helpers.PossibleMoviment.Up;
                case 3:
                    return Helpers.PossibleMoviment.Up_Left;
                case 4:
                    return Helpers.PossibleMoviment.Left;
                case 5:
                    return Helpers.PossibleMoviment.Down_Left;
                case 6:
                    return Helpers.PossibleMoviment.Down;
                case 7:
                    return Helpers.PossibleMoviment.Down_Right;
                default:
                    return Helpers.PossibleMoviment.None;
            }
        }

        public Vector3 PositionAttack(Vector2 colSize, Helpers.PossibleMoviment direction)
        {
            switch (direction)
            {
                case Helpers.PossibleMoviment.Down:
                    return new Vector2(0, -(colSize.y / 2));
                case Helpers.PossibleMoviment.Down_Left:
                    return new Vector2(-(colSize.x / 2), -(colSize.y / 2));
                case Helpers.PossibleMoviment.Down_Right:
                    return new Vector2(colSize.x / 2, -(colSize.y / 2));
                case Helpers.PossibleMoviment.Left:
                    return new Vector2(-colSize.x, 0);
                case Helpers.PossibleMoviment.Right:
                    return new Vector2(colSize.x, 0);
                case Helpers.PossibleMoviment.Up:
                    return new Vector2(0, colSize.y / 2);
                case Helpers.PossibleMoviment.Up_Left:
                    return new Vector2(-(colSize.x / 2), colSize.y / 2);
                case Helpers.PossibleMoviment.Up_Right:
                    return new Vector2(colSize.x / 2, colSize.y / 2);
                default:
                    return new Vector2(0, 0);
            }
        }

        public void NPCInteraction(Component NPCView)
        {
            NPCView.SendMessage("Interaction");
        }
                
        public abstract int GetDamage();
        public abstract Vector3 PositionCenterAttack(Vector3 colSize, Transform transform);
    }
}