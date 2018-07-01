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
        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }

        private Animator _enemyAnimator;
        private Animator enemyAnimator { get { return _enemyAnimator ?? (_enemyAnimator = GetComponent<Animator>()); } }

        private SpriteRenderer _enemySpriteRenderer;
        private SpriteRenderer enemySpriteRenderer { get { return _enemySpriteRenderer ?? (_enemySpriteRenderer = GetComponent<SpriteRenderer>()); } }

        public LayerMask playerLayer;
        BoxCollider2D colliderTransform;
        Controller.EnemyController enemyController;
        [SerializeField] GameObject gObject;
        CountDown attackCountDown = new CountDown(3);
        public DAL.Enemy dal;
        
        void Start()
        {
            enemyController = new Controller.EnemyController();
            dal = enemyController.GetInitialData(transform.position);
            enemyController.SetFieldOfView(gObject.GetComponent<FieldOfView>());
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
        }

        private void FixedUpdate()
        {
            CountDown.DecreaseTime(enemyController.followPlayer);
            CountDown.DecreaseTime(attackCountDown);
            
            if (enemyController.fow.visibleTargets.Count > 0)
            {
                enemyController.WalkTowardTo(transform);
                enemyAnimator.SetBool("isWalking", true);
            }
            else
            {
                enemyAnimator.SetBool("isWalking", false);
                enemyController.target = null;
                enemyController.canAttack = false;
            }

            if (attackCountDown.CoolDown <= 0)
            {
                enemyController.targetsAttacked.Clear();
                if (enemyController.canAttack)
                    attackCountDown.CoolDown = attackCountDown.Rate;
            }
            else if (enemyController.canAttack)
            {
                enemyController.Attack(transform, BoxCollider2D.size, playerLayer);
            }

            var input = enemyController.GetInput();
            enemySpriteRenderer.flipX = input.Flip.Value;
            enemyAnimator.SetFloat("speedX", input.Vector2.x);
            enemyAnimator.SetFloat("speedY", input.Vector2.y);
        }
        private void Update()
        {
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }
    }
}
