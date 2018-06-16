using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.View
{
    public class EnimyView : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }

        private Animator _enemyAnimator;
        private Animator enemyAnimator { get { return _enemyAnimator ?? (_enemyAnimator = GetComponent<Animator>()); } }

        private SpriteRenderer _enemySpriteRenderer;
        private SpriteRenderer enemySpriteRenderer { get { return _enemySpriteRenderer ?? (_enemySpriteRenderer = GetComponent<SpriteRenderer>()); } }

        public LayerMask playerLayer;
        BoxCollider2D colliderTransform;
        Controller.EnimyController enimyController;
        [SerializeField] GameObject gObject;
        CountDown attackCountDown = new CountDown(3);
        
        void Start()
        {
            enimyController = new Controller.EnimyController( new Models.EnimyViewModel { EnimyId = this.gameObject.GetInstanceID(), Life = 2, CharacterTypeId = 5, SpeedRun = 3, SpeedWalk = 3, AttackMin = 1, AttackMax = 1 });
            enimyController.SetFieldOfView(gObject.GetComponent<FieldOfView>());
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
        }

        private void FixedUpdate()
        {
            CountDown.DecreaseTime(enimyController.followPlayer);
            CountDown.DecreaseTime(attackCountDown);
            
            if (enimyController.fow.visibleTargets.Count > 0)
            {
                enimyController.WalkTowardTo(transform);
                enemyAnimator.SetBool("isWalking", true);
            }
            else
            {
                enemyAnimator.SetBool("isWalking", false);
                enimyController.target = null;
                enimyController.canAttack = false;
            }

            if (attackCountDown.CoolDown <= 0)
            {
                enimyController.targetsAttacked.Clear();
                if (enimyController.canAttack)
                    attackCountDown.CoolDown = attackCountDown.Rate;
            }
            else if (enimyController.canAttack)
            {
                enimyController.Attack(transform, BoxCollider2D.size, playerLayer);
            }

            var input = enimyController.GetInput();
            Debug.Log(input.Vector2);
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
