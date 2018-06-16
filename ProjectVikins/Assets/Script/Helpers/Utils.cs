using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helpers
{
    public class Utils
    {
        public List<KeyMove> moveKeyCode = new List<KeyMove>
        {
           new KeyMove(KeyCode.A, Vector2.left, false),
           new KeyMove(KeyCode.D, Vector2.right, true),
           new KeyMove(KeyCode.W, Vector2.up),
           new KeyMove(KeyCode.S, Vector2.down)
        };

        public List<Psm> Psm = new List<Psm>
        {
           new Psm(PossibleMoviment.Down, false),
           new Psm(PossibleMoviment.Down_Left, false),
           new Psm(PossibleMoviment.Down_Right, true),
           new Psm(PossibleMoviment.Left, false),
           new Psm(PossibleMoviment.Right, true),
           new Psm(PossibleMoviment.Up, false),
           new Psm(PossibleMoviment.Up_Left, false),
           new Psm(PossibleMoviment.Up_Right, true),
           new Psm(PossibleMoviment.None, false)
        };
        
        public List<Transform> GetTransformInLayer(string layer)
        {
            if (string.IsNullOrEmpty(layer)) return new List<Transform>();

            var playerLayerId = LayerMask.NameToLayer(layer);

            var players = new List<Transform>();
            GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

            foreach (GameObject go in gos)
                if (go.layer == playerLayerId)
                    players.Add(go.transform);

            return players;
        }


        public Transform NearTargetInView(List<Transform> players, List<Transform> visiblePlayers, Transform transform)
        {
            if (players.Count == 0 || visiblePlayers.Count == 0) return null;
            List<Transform> playersList = ConcatEqualItemList(players, visiblePlayers);
            Transform _player = players[0];
            foreach (var player in playersList)
                if (Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, _player.transform.position))
                    _player = player;

            return _player;
        }

        public Transform NearTarget(List<Transform> players, Transform transform, Transform target)
        {
            if (players.Count == 0) return null;
            Transform _player = target;
            foreach (var player in players)
            {
                var distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < 2 && distance < Vector3.Distance(transform.position, _player.transform.position))
                    _player = player;
            }

            return _player;
        }

        public List<Transform> ConcatEqualItemList(List<Transform> list1, List<Transform> list2)
        {
            var newList = new List<Transform>();
            foreach (var item in list2)
            {
                if (!list1.Contains(item)) continue;
                newList.Add(item);
            }
            return newList;
        }

        public static Vector3 SetPositionZ(Transform objectTransform, float vectorPositionZ)
        {
            return new Vector3(objectTransform.position.x, objectTransform.position.y, vectorPositionZ);
        }
    }
}
