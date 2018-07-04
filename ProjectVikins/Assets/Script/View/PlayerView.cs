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
        KeyMove input = new KeyMove(null, new Vector2(), false);
        [SerializeField] bool isPlayable;
        Helpers.CountDown attackCountDown = new Helpers.CountDown(1.5);
        Helpers.CountDown changeCharacterCountDown = new Helpers.CountDown();
        [SerializeField] GameObject FieldOfViewObj;
        Camera mainCamera;
        CameraView cv;

        public Models.PlayerViewModel model;


        private void Start()
        {
            mainCamera = Camera.main;
            cv = mainCamera.GetComponent<CameraView>();
            playerController = new PlayerController(this.gameObject);
            model = playerController.GetInitialData(transform);
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
            playerController.SetFieldOfView(FieldOfViewObj.GetComponent<FieldOfView>());
            if (model.IsBeingControllable) mainCamera.SendMessage("UpdatePlayerTranform");
        }

        private void FixedUpdate()
        {
            CountDown.DecreaseTime(attackCountDown);
            CountDown.DecreaseTime(changeCharacterCountDown);
            CountDown.DecreaseTime(playerController.followEnemy);

            var tempIsControllable = playerController.GetIsControllable();

            if (isPlayable != tempIsControllable)
                changeCharacterCountDown.StartToCount();

            isPlayable = tempIsControllable;

            if (isPlayable)
            {
                input.Vector2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                foreach (var keyMove in utils.moveKeyCode)
                {
                    if (Input.GetKey(keyMove.KeyCode.Value))
                    {
                        playerController.SetLastMoviment(input.Vector2.x, input.Vector2.y);
                        PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                        PlayerAnimator.SetFloat("speedY", input.Vector2.y);

                        playerController.Walk(keyMove.Vector2);
                        if (!keyMove.Flip.HasValue) continue;
                        PlayerSpriteRenderer.flipX = keyMove.Flip.Value;
                    }
                }

                if (attackCountDown.CoolDown <= 0)
                {
                    playerController.targetsAttacked.Clear();

                    if (attackCountDown.ReturnedToZero)
                    {
                        model.SpeedRun = model.SpeedWalk * 2;
                        model.SpeedWalk = model.SpeedWalk * 2;
                    }
                    if (Input.GetKey(KeyCode.L))
                    {
                        //colocar todos player em modo de attack
                        model.PlayerMode = PlayerModes.Attack;

                        model.SpeedRun = model.SpeedWalk / 2;
                        model.SpeedWalk = model.SpeedWalk / 2;
                        attackCountDown.StartToCount();
                    }
                }
                else
                    playerController.Attack(transform, BoxCollider2D.size);

                if (changeCharacterCountDown.CoolDown <= 0 && Input.GetKeyDown(KeyCode.K))
                {
                    playerController.ChangeControllableCharacter();

                    mainCamera.SendMessage("UpdatePlayerTranform");
                }
            }
            else
            {
                if (model.PlayerMode == PlayerModes.Follow)
                {
                    if (Mathf.Abs(Vector3.Distance(transform.position, cv.playerTranform.transform.position)) > 2)
                    {
                        playerController.WalkToPlayer(transform, cv.playerTranform.transform, ref model);
                        PlayerAnimator.SetBool("isWalking", true);
                    }
                    else
                        PlayerAnimator.SetBool("isWalking", false);
                }
                else if (model.PlayerMode == PlayerModes.Attack)
                {
                    if (playerController.fow.visibleTargets.Count > 0)
                    {
                        playerController.FindTarget(transform);

                        if (playerController.target != null)
                        {
                            if(Mathf.Abs(Vector3.Distance(transform.position, playerController.target.position)) > 0.5)
                            {
                                playerController.WalkTowardTo(transform, ref model);
                                PlayerAnimator.SetBool("isWalking", true);
                            }
                            else
                            {
                                playerController.canAttack = true;
                            }
                        }
                        else
                            PlayerAnimator.SetBool("isWalking", false);
                    }
                    else
                    {
                        PlayerAnimator.SetBool("isWalking", false);
                        playerController.target = null;
                        playerController.canAttack = false;
                    }
                }

                input = playerController.GetInput();
                PlayerSpriteRenderer.flipX = input.Flip.Value;
                PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                PlayerAnimator.SetFloat("speedY", input.Vector2.y);

                if (attackCountDown.CoolDown <= 0)
                {
                    playerController.targetsAttacked.Clear();

                    if (attackCountDown.ReturnedToZero)
                    {
                        model.SpeedRun = model.SpeedWalk * 2;
                        model.SpeedWalk = model.SpeedWalk * 2;
                    }

                    if (playerController.canAttack)
                    {
                        model.SpeedRun = model.SpeedWalk / 2;
                        model.SpeedWalk = model.SpeedWalk / 2;
                        attackCountDown.StartToCount();
                    }

                }
                else if (playerController.canAttack)
                {
                    playerController.Attack(transform, BoxCollider2D.size);
                }
            }

            if (Vector3.Distance(transform.position, cv.playerTranform.transform.position) > 15)
            {
                model.PlayerMode = PlayerModes.Follow;
            }
            //else if (Vector3.Distance(transform.position, cv.player.transform.position) < 2)
            //{
            //    playerMode = PlayerModes.Attack;
            //}
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
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    playerController.SaveData();

                }
            }
            else
            {
                foreach (var playerMode in utils.playerModes)
                {
                    if (Input.GetKey(playerMode.KeyButton[0]) && Input.GetKey(playerMode.KeyButton[1]))
                    {
                        model.PlayerMode = playerMode.Value;
                        if (playerMode.Value == PlayerModes.Wait)
                            PlayerAnimator.SetBool("isWalking", false);
                    }
                }
            }
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }
    }
}
