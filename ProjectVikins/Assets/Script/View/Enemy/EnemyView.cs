using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.View
{
    public class EnemyView : MonoBehaviour
    {
        [HideInInspector] public BoxCollider2D BoxCollider2D;
        [HideInInspector] public Animator EnemyAnimator;
        [HideInInspector] public SpriteRenderer EnemySpriteRenderer;

        public LayerMask playerLayer;
        BoxCollider2D colliderTransform;
        [HideInInspector] public Controller.EnemyController enemyController;
        [HideInInspector] public CountDown attackCountDown = new CountDown(3);
        [HideInInspector] public Models.EnemyViewModel model;

        void Start()
        {
            #region GetComponent
            BoxCollider2D = GetComponent<BoxCollider2D>();
            EnemyAnimator = GetComponent<Animator>();
            EnemySpriteRenderer = GetComponent<SpriteRenderer>();
            #endregion

            enemyController = new Controller.EnemyController();
            model = enemyController.GetInitialData(gameObject);
            enemyController.SetFieldOfView(gameObject.GetComponentInChildren<FieldOfView>());
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
        }

        public void EnemyUpdate()
        {
            CountDown.DecreaseTime(enemyController.followPlayer);
            CountDown.DecreaseTime(attackCountDown);

            var input = enemyController.GetInput(model.LastMoviment.Value);
            EnemySpriteRenderer.flipX = input.Flip.Value;
            EnemyAnimator.SetFloat("speedX", input.Vector2.x);
            EnemyAnimator.SetFloat("speedY", input.Vector2.y);
        }
        private void Update()
        {
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }

        public bool GetDamage(int damage, Vector3? playerPosition, bool recue = false)
        {
            if (playerPosition.HasValue && model.DirectionsDefended != null && model.DirectionsDefended.Count > 0)
            {
                if (model.DirectionsDefended.Contains((int)enemyController.GetDirection(transform.position, playerPosition.Value)))
                    return false;
            }

            model.CurrentLife -= damage;

            if (recue)
                transform.Translate(enemyController.GetInput(enemyController.GetDirection(playerPosition.Value, transform.position)).Vector2 * 0.15f);

            if (model.CurrentLife <= 0)
            {
                model.IsDead = true;
                DAL.ProjectVikingsContext.aliveEnemies.Remove(model.GameObject);
                GetComponents<BoxCollider2D>().Where(x => !x.isTrigger).ToList().ForEach(x => x.enabled = false);
                EnemyAnimator.SetBool("isWalking", false);
                if (model.PrefToBeAttacked)
                    DAL.ProjectVikingsContext.alivePrefEnemies.Remove(model.GameObject);
                return true;
            }
            return false;
        }
    }
}
