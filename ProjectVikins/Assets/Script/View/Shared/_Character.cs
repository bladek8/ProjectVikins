using Assets.Script.Controller;
using Assets.Script.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.View.Shared
{
    public class _Character : MonoBehaviour
    {
        public PlayerController playerController;
        public Utils utils = new Utils();

        [HideInInspector] public Animator PlayerAnimator;

        [HideInInspector] public SpriteRenderer PlayerSpriteRenderer;

        [HideInInspector] public BoxCollider2D PlayerBoxCollider2D;

        [HideInInspector] public BoxCollider2D colliderTransform;
        [HideInInspector] public KeyMove input = new KeyMove(null, new Vector2(), false);
        [HideInInspector] public bool isPlayable;
        public GameObject FieldOfViewObj;
        public float DistanceOfPlayer;
        [HideInInspector] public GameObject camera;
        [HideInInspector] public CameraView cv;

        public Models.PlayerViewModel model;

        public Helpers.CountDown changeCharacterCountDown = new Helpers.CountDown();
        public Helpers.CountDown savePlayerCountDown = new Helpers.CountDown(3);

        //public GameObject leftFoot;
        //public GameObject rightFoot;
        public Slider LifeBar;
        Transform oldTarget = null;

        private void Start()
        {
            #region GetComponents
            PlayerAnimator = gameObject.GetComponent<Animator>();
            PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            var allBoxColliders = GetComponents<BoxCollider2D>();
            colliderTransform = allBoxColliders.Single(x => x.isTrigger == false);
            PlayerBoxCollider2D = allBoxColliders.Single(x => x.isTrigger == true);
            #endregion

            camera = GameObject.FindGameObjectWithTag("camera");
            cv = camera.GetComponent<CameraView>();
            playerController = new PlayerController();
            model = playerController.GetInitialData(gameObject);
            model.ForceToWalk = false;
            model.ForceToStop = false;
            LifeBar.value = CalculateLife();
            playerController.SetFieldOfView(FieldOfViewObj.GetComponent<FieldOfView>());
            if (model.IsBeingControllable) camera.SendMessage("UpdatePlayerTranform");
        }
        public void CharacterUpdate()
        {
            CountDown.DecreaseTime(changeCharacterCountDown);
            CountDown.DecreaseTime(playerController.followEnemy);

            var tempIsControllable = playerController.GetIsControllable();

            if (isPlayable != tempIsControllable)
                changeCharacterCountDown.StartToCount();

            isPlayable = tempIsControllable;

            if (isPlayable)
            {
                //if (LifeBar.name == "Slider")
                //{
                //    LifeBar.transform.localScale = new Vector2(1.29f, 1.52f);
                //    LifeBar.transform.localPosition = new Vector2(-274, 112);
                //}
                //else if (LifeBar.name == "Slider (1)")
                //{
                //    LifeBar.transform.localScale = new Vector2(1.29f, 1.52f);
                //    LifeBar.transform.localPosition = new Vector2(-274, 81.69998f);
                //}
                //else if (LifeBar.name == "Slider (2)")
                //{
                //    LifeBar.transform.localScale = new Vector2(1.29f, 1.52f);
                //    LifeBar.transform.localPosition = new Vector2(-274, 51.47721f);
                //}

                if (playerController.target != oldTarget)
                {
                    oldTarget = playerController.target;
                    if (playerController.target != null)
                        PlayerController.GetSliderEnemy(playerController.target);
                }

                #region Mover

                if (!model.ForceToStop)
                {
                    input.Vector2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                    foreach (var keyMove in utils.moveKeyCode)
                    {
                        if (Input.GetKey(keyMove.KeyCode.Value))
                        {
                            if (Input.GetKey(KeyCode.LeftShift) && !model.ForceToWalk)
                            {
                                PlayerAnimator.SetBool("isRunning", true);
                                PlayerAnimator.SetBool("isWalking", false);
                                playerController.Walk(keyMove.Vector2, model.SpeedRun);
                            }
                            else
                            {
                                PlayerAnimator.SetBool("isWalking", true);
                                PlayerAnimator.SetBool("isRunning", false);
                                playerController.Walk(keyMove.Vector2, model.SpeedWalk);
                            }

                            playerController.SetLastMoviment(input.Vector2.x, input.Vector2.y);
                            PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                            PlayerAnimator.SetFloat("speedY", input.Vector2.y);

                            if (!keyMove.Flip.HasValue) continue;
                            PlayerSpriteRenderer.flipX = keyMove.Flip.Value;
                        }
                        if (Input.GetKeyUp(keyMove.KeyCode.Value))
                        {
                            PlayerAnimator.SetBool("isWalking", false);
                            PlayerAnimator.SetBool("isRunning", false);
                        }
                    }
                }
                if (model.ForceToStop)
                {
                    PlayerAnimator.SetBool("isWalking", false);
                    PlayerAnimator.SetBool("isRunning", false);
                }
                #endregion

                #region Change Character

                if (changeCharacterCountDown.CoolDown <= 0 && Input.GetKeyDown(KeyCode.K) && !model.ForceToStop)
                {
                    playerController.ChangeControllableCharacter();

                    camera.SendMessage("UpdatePlayerTranform");
                }

                #endregion

                #region Interact

                if (Input.GetKey(KeyCode.J))
                {
                    playerController.Interact(transform, PlayerBoxCollider2D.size);
                }
                #endregion
            }

            else
            {
                //if (LifeBar.name == "Slider")
                //{
                //    LifeBar.transform.localScale = new Vector2(1, 1);
                //    LifeBar.transform.localPosition = new Vector2(-287, 112);
                //}
                //else if (LifeBar.name == "Slider (1)")
                //{
                //    LifeBar.transform.localScale = new Vector2(1, 1);
                //    LifeBar.transform.localPosition = new Vector2(-287, 81.69998f);
                //}
                //else if (LifeBar.name == "Slider (2)")
                //{
                //    LifeBar.transform.localScale = new Vector2(1, 1);
                //    LifeBar.transform.localPosition = new Vector2(-287, 51.47721f);
                //}

                #region Follow

                if (model.PlayerMode == PlayerModes.Follow)
                {
                    if (Mathf.Abs(Vector3.Distance(transform.position, cv.playerGameObj.transform.position)) > DistanceOfPlayer)
                    {
                        playerController.WalkToPlayer(transform, cv.playerGameObj.transform, ref model);
                        PlayerAnimator.SetBool("isWalking", true);
                    }
                    else
                    {
                        PlayerAnimator.SetBool("isWalking", false);
                        PlayerAnimator.SetBool("isRunning", false);
                    }
                }

                #endregion

                #region Walk Input

                input = playerController.GetInput();
                PlayerSpriteRenderer.flipX = input.Flip.Value;
                PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                PlayerAnimator.SetFloat("speedY", input.Vector2.y);

                #endregion

                if (Vector3.Distance(transform.position, cv.playerGameObj.transform.position) > 15)
                {
                    model.PlayerMode = PlayerModes.Follow;
                }
            }

            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);

            #region Change PlayerMode

            foreach (var playerMode in utils.playerModes)
            {
                if (Input.GetKey(playerMode.KeyButton[0]) && Input.GetKey(playerMode.KeyButton[1]))
                {
                    model.PlayerMode = playerMode.Value;
                    if (playerMode.Value == PlayerModes.Wait)
                    {
                        PlayerAnimator.SetBool("isWalking", false);
                        PlayerAnimator.SetBool("isRunning", false);
                    }
                }
            }

            #endregion
        }

        public bool GetDamage(int damage)
        {
            playerController.AttackMode();
            model.CurrentLife -= damage;
            LifeBar.value = CalculateLife();

            if (model.CurrentLife <= 0)
            {
                if (model.IsBeingControllable)
                {
                    playerController.ChangeControllableCharacter();
                    camera.SendMessage("UpdatePlayerTranform");
                }
                model.IsDead = true;
                DAL.ProjectVikingsContext.alivePlayers.Remove(model.GameObject);
                PlayerAnimator.SetBool("isWalking", false);
                PlayerAnimator.SetBool("isRunning", false);
                GetComponents<BoxCollider2D>().Where(x => !x.isTrigger).ToList().ForEach(x => x.enabled = false);
                return true;
            }
            return false;
        }
        public void StartSavePlayer()
        {
            if (!model.IsDead || model.ForceToStop) return;
            SetForceToStop(true);
            StartCoroutine("SavingPlayer");
        }

        public void SetForceToWalk(bool value)
        {
            playerController.SetForceToWalk(value);
        }

        public void SetForceToStop(bool value)
        {
            playerController.SetForceToStop(value);
        }

        IEnumerator SavingPlayer()
        {
            savePlayerCountDown.StartToCount();
            while (true)
            {
                if (Input.GetKey(KeyCode.J))
                {
                    CountDown.DecreaseTime(savePlayerCountDown);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                else
                {
                    print("não salvou!");
                    savePlayerCountDown.CoolDown = 0;
                    break;
                }
                if (savePlayerCountDown.ReturnedToZero)
                {
                    print("salvou!");
                    model.IsDead = false;
                    DAL.ProjectVikingsContext.alivePlayers.Add(model.GameObject);
                    break;
                }
            }
            SetForceToStop(false);
        }

        float CalculateLife()
        {
            return model.CurrentLife / model.MaxLife;
        }
    }
}
