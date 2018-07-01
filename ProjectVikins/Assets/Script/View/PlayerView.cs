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

        PlayerModes playerMode = PlayerModes.Attack;
        BoxCollider2D colliderTransform;
        KeyMove input = new KeyMove(null, new Vector2(), false);
        [SerializeField] bool isPlayable;
        Helpers.CountDown attackCountDown = new Helpers.CountDown(1.5);
        Helpers.CountDown changeCharacterCountDown = new Helpers.CountDown();
        [SerializeField] GameObject FieldOfViewObj;
        Camera mainCamera;
        CameraView cv;
        DAL.Player dal;

        private void Start()
        {
            mainCamera = Camera.main;
            cv = mainCamera.GetComponent<CameraView>();
            playerController = new PlayerController(this.gameObject);
            dal = playerController.GetInitialData(transform.position);
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
                        playerController.Increase(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                    if (Input.GetKey(KeyCode.L))
                    {
                        playerMode = PlayerModes.Attack;
                        playerController.Decrease(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                        attackCountDown.StartToCount();
                    }
                }
                else
                    playerController.Attack(transform, BoxCollider2D.size);

                if (changeCharacterCountDown.CoolDown <= 0 && Input.GetKeyDown(KeyCode.K))
                    playerController.ChangeControllableCharacter();
            }
            else
            {
                if (playerMode == PlayerModes.Follow)
                {
                    playerController.WalkToPlayer(transform, cv.player.transform);
                    if (Mathf.Abs(Vector3.Distance(transform.position, cv.player.transform.position)) > 0.5)
                        PlayerAnimator.SetBool("isWalking", true);
                    else
                        PlayerAnimator.SetBool("isWalking", false);
                }
                else if (playerMode == PlayerModes.Attack)
                {
                    if (playerController.fow.visibleTargets.Count > 0)
                    {
                        playerController.WalkTowardTo(transform);
                        if (playerController.target != null && Mathf.Abs(Vector3.Distance(transform.position, playerController.target.position)) > 0.5)
                            PlayerAnimator.SetBool("isWalking", true);
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

                    if (playerController.canAttack)
                        attackCountDown.CoolDown = attackCountDown.Rate;

                    if (attackCountDown.ReturnedToZero)
                        playerController.Increase(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                }
                else if (playerController.canAttack)
                {
                    playerController.Attack(transform, BoxCollider2D.size);
                }
            }

            if (Vector3.Distance(transform.position, cv.player.transform.position) > 15)
            {
                playerMode = PlayerModes.Follow;
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
            }
            else
            {
                foreach (var playerMode in utils.playerModes)
                {
                    if (Input.GetKey(playerMode.KeyButton[0]) && Input.GetKey(playerMode.KeyButton[1]))
                    {
                        this.playerMode = playerMode.Value;

                        if(playerMode.Value == PlayerModes.Wait)
                            PlayerAnimator.SetBool("isWalking", false);
                    }
                }
                Debug.Log(playerMode);
            }
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }
    }
}
