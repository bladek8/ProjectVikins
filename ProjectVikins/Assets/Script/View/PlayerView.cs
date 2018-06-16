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
        float inputX, inputY;
        [SerializeField] bool isPlayable;
        Helpers.CountDown attackCountDown = new Helpers.CountDown(1.5);
        Helpers.CountDown changeCharacterCountDown = new Helpers.CountDown();
        [SerializeField] GameObject FieldOfViewObj;
        Camera mainCamera;
        CameraView cv;

        private void Start()
        {
            mainCamera = Camera.main;
            cv = mainCamera.GetComponent<CameraView>();
            playerController = new PlayerController(new Models.PlayerViewModel { PlayerId = this.gameObject.GetInstanceID(), Life = 2, CharacterTypeId = 5, SpeedRun = 3, SpeedWalk = 3, AttackMin = 1, AttackMax = 1, IsBeingControllable = isPlayable }, this.gameObject);
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
            playerController.SetFieldOfView(FieldOfViewObj.GetComponent<FieldOfView>());
        }

        private void FixedUpdate()
        {
            CountDown.DecreaseTime(attackCountDown);
            CountDown.DecreaseTime(changeCharacterCountDown);
            var tempIsControllable = playerController.GetIsControllable();
            if (isPlayable != tempIsControllable)
                changeCharacterCountDown.StartToCount();
            isPlayable = tempIsControllable;
            if (isPlayable)
            {
                foreach (var keyMove in utils.moveKeyCode)
                {
                    if (Input.GetKey(keyMove.KeyCode.Value))
                    {
                        playerController.SetLastMoviment(inputX, inputY);
                        PlayerAnimator.SetFloat("speedX", inputX);
                        PlayerAnimator.SetFloat("speedY", inputY);

                        playerController.Walk(keyMove.Vector2);
                        if (!keyMove.Flip.HasValue) continue;
                        PlayerSpriteRenderer.flipX = keyMove.Flip.Value;
                    }
                }
                inputX = Input.GetAxis("Horizontal");
                inputY = Input.GetAxis("Vertical");


                if (changeCharacterCountDown.CoolDown <= 0 && Input.GetKeyDown(KeyCode.K))
                    playerController.ChangeControllableCharacter();
            }
            else
            {
                if (Vector3.Distance(transform.position, cv.player.transform.position) > 5)
                {
                    playerController.WalkToPlayer(transform, cv.player.transform);
                }
                else if (playerController.fow.visibleTargets.Count > 0)
                {
                    playerController.WalkTowardTo(transform);
                    PlayerAnimator.SetBool("isWalking", true);
                }
                else
                {
                    PlayerAnimator.SetBool("isWalking", false);
                    playerController.target = null;
                    playerController.canAttack = false;
                }
            }

            if (attackCountDown.CoolDown <= 0)
            {
                playerController.targetsAttacked.Clear();

                if (attackCountDown.ReturnedToZero)
                    playerController.Increase(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                if (isPlayable)
                {
                    if (Input.GetKey(KeyCode.L))
                    {
                        playerController.Decrease(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                        attackCountDown.StartToCount();
                    }
                }
                else
                {

                }
            }
            else
                playerController.Attack(transform, BoxCollider2D.size);

        }

        private void Update()
        {
            if (isPlayable)
            {
                foreach (var keyMove in utils.moveKeyCode)
                {
                    if (Input.GetKey(keyMove.KeyCode.Value))
                    {
                        PlayerAnimator.SetBool("isWalking", true);
                    }
                    if (Input.GetKeyUp(keyMove.KeyCode.Value))
                    {
                        PlayerAnimator.SetBool("isWalking", false);
                    }
                }
            }
            else
            {

            }
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }
    }
}
