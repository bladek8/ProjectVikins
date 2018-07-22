using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using System.Linq;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    [Range(0, 360)]
    public float rotation;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public bool IsPlayer;
    List<GameObject> _aliveTargets;
    List<GameObject> _alivePrefTargets;

    [HideInInspector]
    public List<Transform> visibleTargets;

    void Start()
    {
        #region  [GetTargets]
        if (IsPlayer)
        {
            _aliveTargets = Assets.Script.DAL.ProjectVikingsContext.aliveEnemies;
            _alivePrefTargets = Assets.Script.DAL.ProjectVikingsContext.alivePrefEnemies;
        }
        else
        {
            _aliveTargets = Assets.Script.DAL.ProjectVikingsContext.alivePlayers;
            _alivePrefTargets = Assets.Script.DAL.ProjectVikingsContext.alivePrefPlayers;
        }
        #endregion

        visibleTargets = new List<Transform>();
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        StartCoroutine("FindTargetWithDelay", 0.2f);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        #region  [GetTargets]
        if (IsPlayer)
        {
            _aliveTargets = Assets.Script.DAL.ProjectVikingsContext.aliveEnemies;
            _alivePrefTargets = Assets.Script.DAL.ProjectVikingsContext.alivePrefEnemies;
        }
        else
        {
            _aliveTargets = Assets.Script.DAL.ProjectVikingsContext.alivePlayers;
            _alivePrefTargets = Assets.Script.DAL.ProjectVikingsContext.alivePrefPlayers;
        }
        #endregion

        visibleTargets.Clear();

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        var _targetsInViewRadius = targetsInViewRadius.Select(x => x.gameObject).ToList();
        var PreferenceTargets = _targetsInViewRadius.Intersect(_alivePrefTargets);

        if (PreferenceTargets.Count() > 0)
            _targetsInViewRadius = PreferenceTargets.ToList();

        for (int i = 0; i < _targetsInViewRadius.Count; i++)
        {
            if (!_aliveTargets.Contains(_targetsInViewRadius[i])) continue;
            
            Transform target = _targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }


    public Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void TurnView(Transform target)
    {
        var angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}