using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Controller;
using Assets.Script.Helpers;

namespace Assets.Script.View
{
    public class PlayerView : MonoBehaviour
    {
        PlayerController playerController;
        private Utils utils = new Utils();

        private Animator _playerAnimator;
        private Animator PlayerAnimator { get { return _playerAnimator ?? (_playerAnimator = GetComponent<Animator>()); } }

        private SpriteRenderer _playerSpriteRenderer;
        private SpriteRenderer PlayerSpriteRenderer { get { return _playerSpriteRenderer ?? (_playerSpriteRenderer = GetComponent<SpriteRenderer>()); } }

        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }

        BoxCollider2D colliderTransform;
        public LayerMask enimyLayer;
        bool returnSpeed = false;
        float inputX, inputY;
        Helpers.CountDown attackCountDown = new Helpers.CountDown(1.5);

        private void Start()
        {
            playerController = new PlayerController(new Models.PlayerViewModel { PlayerId = this.gameObject.GetInstanceID(), Life = 2, CharacterTypeId = 5, SpeedRun = 3, SpeedWalk = 3, AttackMin = 1, AttackMax = 1 }, this.gameObject);
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
        }

        private void FixedUpdate()
        {
            CountDown.DecreaseTime(attackCountDown);

            foreach (var keyMove in utils.moveKeyCode)
            {
                if (Input.GetKey(keyMove.KeyCode))
                {
                    playerController.Walk(keyMove.Vector2);
                    if (!keyMove.Flip.HasValue) continue;
                    PlayerSpriteRenderer.flipX = keyMove.Flip.Value;
                }
            }
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");

            if (attackCountDown.CoolDown <= 0)
            {
                playerController.targetsAttacked.Clear();

                if (Input.GetKey(KeyCode.L))
                {
                    playerController.Decrease(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                    attackCountDown.CoolDown = attackCountDown.Rate;
                }
                if (returnSpeed)
                {
                    playerController.Increase(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                    returnSpeed = false;
                }
            }
            else
                playerController.Attack(transform, BoxCollider2D.size, enimyLayer);

            if (Input.GetKeyUp(KeyCode.L))
                returnSpeed = true;

            playerController.SetLastMoviment(inputX, inputY);
            PlayerAnimator.SetFloat("speedX", inputX);
            PlayerAnimator.SetFloat("speedY", inputY);
        }

        private void Update()
        {
            foreach (var keyMove in utils.moveKeyCode)
            {
                if (Input.GetKey(keyMove.KeyCode))
                {
                    PlayerAnimator.SetBool("isWalking", true);
                }
                if (Input.GetKeyUp(keyMove.KeyCode))
                {
                    PlayerAnimator.SetBool("isWalking", false);
                }
            }
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }
    }
}
