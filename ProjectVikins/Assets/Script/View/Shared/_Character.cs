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
        [HideInInspector] public CapsuleCollider2D PlayerCollider2D;
        [HideInInspector] public BoxCollider2D colliderTransform;
        [HideInInspector] public KeyMove input = new KeyMove(null, new Vector2(), false);
        [HideInInspector] public bool isPlayable;
        [HideInInspector] public new GameObject camera;
        [HideInInspector] public CameraView cv;
        [HideInInspector] public float halfSizeY;
        [HideInInspector] public float distanceCenterWater;

        public int id;
        public float DistanceOfPlayer;
        public Models.PlayerViewModel model = new Models.PlayerViewModel();

        public CountDown changeCharacterCountDown = new CountDown();
        public CountDown savePlayerCountDown = new CountDown(3);
        public CountDown disabledCountDown = new CountDown();
        private SAP2D.SAP2DManager manager;
        public Vector2[] path;
        public int pathIndex;
        public SAP2D.PathfindingConfig2D Config;

        public Slider LifeBar;
        RectTransform rectT;
        Transform oldTarget = null;

        private void Start()
        {
            playerController = new PlayerController();
            BLL.PlayerFunctions playerFunctions = new BLL.PlayerFunctions();

            #region [GetComponents]
            PlayerAnimator = gameObject.GetComponent<Animator>();
            PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            colliderTransform = GetComponent<BoxCollider2D>();
            PlayerCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
            #endregion

            #region [Camera]
            camera = GameObject.FindGameObjectWithTag("camera");
            cv = camera.GetComponent<CameraView>();
            playerController.SetFieldOfView(gameObject.GetComponentInChildren<FieldOfView>());
            #endregion
                        
            #region [Model]
            //model = playerFunctions.GetDataViewModel(DAL.ProjectVikingsContext.defaultPlayer);
            model = playerController.GetInitialData(id, gameObject);
            model.ForceToWalk = false;
            model.ForceToStop = false;
            #endregion

            #region [LifeBar]
            rectT = LifeBar.GetComponent<RectTransform>();
            LifeBar.value = CalculateLife();
            SetSlideSizes();
            #endregion

            halfSizeY = PlayerSpriteRenderer.size.y / 2;
            if (model.IsBeingControllable) camera.SendMessage("UpdatePlayerTranform");

            manager = SAP2D.SAP2DManager.singleton;
            Config = ScriptableObject.CreateInstance<SAP2D.PathfindingConfig2D>();

            StartCoroutine(FindPath());
        }

        public void CharacterUpdate()
        {
            CountDown.DecreaseTime(changeCharacterCountDown);
            CountDown.DecreaseTime(playerController.followEnemy);

            var tempIsControllable = playerController.GetIsControllable();

            if (isPlayable != tempIsControllable)
                changeCharacterCountDown.StartToCount();

            if (isPlayable != tempIsControllable)
                SetSlideSizes();

            isPlayable = tempIsControllable;

            if (isPlayable)
            {
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
                                playerController.Walk(keyMove.Vector2, (float)model.SpeedRun);
                            }
                            else
                            {
                                PlayerAnimator.SetBool("isWalking", true);
                                PlayerAnimator.SetBool("isRunning", false);
                                playerController.Walk(keyMove.Vector2, (float)model.SpeedWalk);
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
                    SetSlideSizes();
                }

                #endregion

                #region Interact

                if (Input.GetKey(KeyCode.J))
                {
                    playerController.Interact(transform, PlayerCollider2D.size);
                }
                #endregion

            }

            else
            {
                #region Follow

                if (model.PlayerMode == PlayerModes.Follow)
                {
                    if (Mathf.Abs(Vector3.Distance(transform.position, cv.playerGameObj.transform.position)) > DistanceOfPlayer)
                    {
                        //playerController.WalkToPlayer(transform, cv.playerGameObj.transform, ref model);
                        playerController.target = cv.playerGameObj.transform;
                        Move();
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

                input = playerController.GetInput(model.LastMoviment.Value);
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

        public bool GetDamage(int damage, Vector3? enemyPosition, float? disabledRate, bool recue = false)
        {
            if (enemyPosition.HasValue && model.DirectionsDefended != null && model.DirectionsDefended.Count > 0)
            {
                if (model.DirectionsDefended.Contains((int)playerController.GetDirection(transform.position, enemyPosition.Value)))
                    return false;
            }

            if (disabledRate.HasValue)
            {
                disabledCountDown.Rate = disabledRate.Value;
                disabledCountDown.StartToCount();
            }

            playerController.AttackMode();
            model.CurrentLife -= damage;
            LifeBar.value = CalculateLife();

            if (recue)
                transform.Translate(playerController.GetInput(playerController.GetDirection(enemyPosition.Value, transform.position)).Vector2 * 0.15f);

            StopCoroutine("SavingPlayer");
            SetForceToStop(false);

            if (model.CurrentLife <= 0)
            {
                if (model.IsBeingControllable)
                {
                    playerController.ChangeControllableCharacter();
                    camera.SendMessage("UpdatePlayerTranform");
                    SetSlideSizes();
                }
                model.IsDead = true;
                DAL.ProjectVikingsContext.alivePlayers.Remove(model.GameObject);
                PlayerAnimator.SetBool("isWalking", false);
                PlayerAnimator.SetBool("isRunning", false);
                GetComponents<BoxCollider2D>().Where(x => !x.isTrigger).ToList().ForEach(x => x.enabled = false);
                if (model.IsTank)
                    DAL.ProjectVikingsContext.alivePrefPlayers.Remove(model.GameObject);
                return true;
            }
            return false;
        }

        public void SetForceToWalk(bool value)
        {
            playerController.SetForceToWalk(value);
        }

        public void SetForceToStop(bool value)
        {
            playerController.SetForceToStop(value);
        }

        private float CalculateLife()
        {
            return (float)(model.CurrentLife / model.MaxLife);
        }

        private void SetSlideSizes()
        {
            if (model.IsBeingControllable)
            {
                rectT.localScale = new Vector2(1.29f, 1.52f);
                rectT.anchoredPosition = new Vector2(126, rectT.anchoredPosition.y);
            }
            else
            {
                rectT.localScale = new Vector2(1f, 1f);
                rectT.anchoredPosition = new Vector2(113, rectT.anchoredPosition.y);
            }
        }

        #region [SavePlayer]

        public void StartSavePlayer()
        {
            if (!model.IsDead || model.ForceToStop) return;
            SetForceToStop(true);
            StartCoroutine("SavingPlayer");
        }

        private IEnumerator SavingPlayer()
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
                    model.CurrentLife = 3;
                    DAL.ProjectVikingsContext.alivePlayers.Add(model.GameObject);
                    GetComponents<BoxCollider2D>().Where(x => !x.isTrigger).ToList().ForEach(x => x.enabled = true);
                    LifeBar.value = CalculateLife();
                    if (model.IsTank)
                        DAL.ProjectVikingsContext.alivePrefPlayers.Add(model.GameObject);
                    PlayerAnimator.SetBool("WasSafe", true);
                    break;
                }
            }
            SetForceToStop(false);
        }

        public void HealthParticles(GameObject particles)
        {
            Instantiate(particles, new Vector3(transform.position.x, transform.position.y, 99), transform.rotation);
        }

        public void EndSafeAnimiation()
        {
            PlayerAnimator.SetBool("WasSafe", false);
        }
        #endregion

        public void InWater(float distanceToCenter)
        {
            //print(distanceToCenter);
            model.SpeedRun = 1;
            model.SpeedWalk = 1;

            distanceCenterWater = distanceToCenter > halfSizeY ? halfSizeY : distanceToCenter;
        }
        public void OutWater()
        {
            model.SpeedRun = 2;
            model.SpeedWalk = 2;
            distanceCenterWater = 0;
        }

        IEnumerator FindPath()
        { //path loop update

            //if (isTargetWalkable())
            //if the object is already in the target point, the path should not be searched
            if (playerController.target != null)
            {
                if (manager.grid.GetTileFromWorldPosition(colliderTransform.bounds.center).WorldPosition != manager.grid.GetTileFromWorldPosition(playerController.target.position).WorldPosition)
                {
                    path = manager.FindPath(transform.position, playerController.target.position, Config);
                    pathIndex = 0;
                }
            }
            yield return new WaitForSeconds(0.15f);

            StartCoroutine(FindPath());
        }

        void Move()
        { //object movement
            if (playerController.target != null)
            {
                Vector3 targetVector = manager.grid.GetTileFromWorldPosition(playerController.target.position).WorldPosition; //target tile position

                if (colliderTransform.bounds.center != targetVector)
                {
                    if (path != null && path.Length > 0)
                    {

                        Vector3 currentTargetVector = manager.grid.GetTileFromWorldPosition(path[pathIndex]).WorldPosition; //current tile position

                        //line movement to current tile
                        transform.position = Vector2.MoveTowards(transform.position, currentTargetVector, Time.deltaTime * (float)model.SpeedWalk);
                        playerController.fow.TurnView(playerController.target);
                        model.LastMoviment = playerController.GetDirection(transform.position, playerController.target.position);

                        if (Vector2.Distance(transform.position, currentTargetVector) < 0.1f)
                        { //if the object has approached a sufficient distance,
                            if (pathIndex < path.Length - 1)                                                     //to move to the next tile
                                pathIndex++;
                        }
                    }
                }
            }
        }

        public void UseItem(List<Models.ItemAttributtesViewModel> itemAttr)
        {
            foreach (var item in itemAttr)
            {
                switch (item.Name)
                {
                    case "Health":
                        model.CurrentLife += item.Value;
                        if (model.CurrentLife > model.MaxLife) model.CurrentLife = model.MaxLife;
                        LifeBar.value = CalculateLife();
                        break;
                    case "Strenght":
                        model.AttackMin += (int)item.Value;
                        model.AttackMax += (int)item.Value;
                        break;
                }
            }
        }
    }
}