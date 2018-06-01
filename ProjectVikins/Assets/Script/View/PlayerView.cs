using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Controller;

namespace Assets.Script.View
{
    public class PlayerView : MonoBehaviour
    {
        PlayerController playerController;
        private Helpers.Utils utils = new Helpers.Utils();

        private Animator _playerAnimator;
        private Animator PlayerAnimator { get { return _playerAnimator ?? (_playerAnimator = GetComponent<Animator>()); } }

        private SpriteRenderer _playerSpriteRenderer;
        private SpriteRenderer PlayerSpriteRenderer { get { return _playerSpriteRenderer ?? (_playerSpriteRenderer = GetComponent<SpriteRenderer>()); } }

        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }
        BoxCollider2D boxColliderAttack;

        bool returnSpeed = false;
        float inputX, inputY;
        Helpers.CountDown attackCountDown = new Helpers.CountDown(1.5);
        List<int> enimiesAttacked = new List<int>();

        private void Start()
        {
            boxColliderAttack = this.gameObject.AddComponent<BoxCollider2D>();
            playerController = new PlayerController(new DAL.Player { PlayerId = this.gameObject.GetInstanceID(), Life = 2, CharacterTypeId = 5, SpeedRun = 3, SpeedWalk = 3, AttackMin = 1, AttackMax = 1 }, this.gameObject);
            boxColliderAttack = BoxCollider2D;
            boxColliderAttack.isTrigger = true;
            boxColliderAttack.enabled = false;
        }

        private void FixedUpdate()
        {
            attackCountDown.DecreaseTime(attackCountDown);

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
                boxColliderAttack.enabled = false;
                enimiesAttacked.Clear();

                if (Input.GetKey(KeyCode.L))
                {
                    playerController.Decrease(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                    boxColliderAttack.offset = playerController.Attack(boxColliderAttack.size);
                    attackCountDown.CoolDown = attackCountDown.Rate;
                    boxColliderAttack.enabled = true;
                }
                if (returnSpeed)
                {
                    playerController.Increase(new Script.Models.PlayerViewModel() { SpeedWalk = 2, SpeedRun = 2 });
                    returnSpeed = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.L))
                returnSpeed = true;
            //boxColliderAttack.enabled = false;
            //boxColliderAttack.offset = new Vector2(0, 0);
            //attackCountDown.CoolDown = 0;

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
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Enimy")//Input.GetKey(KeyCode.L) &&
            {
                //boxColliderAttack.offset = new Vector2(0, 0);
                if (enimiesAttacked.Contains(collision.gameObject.GetInstanceID()))
                    return;

                enimiesAttacked.Add(collision.gameObject.GetInstanceID());
                if (Convert.ToInt32(playerController.DecreaseStats(collision.gameObject.name, "Life", playerController.GetDamage(), collision.gameObject.GetInstanceID())) <= 0)
                    Destroy(collision.gameObject);
                //playerController.GiveDamage("Enimy", playerController.GetDamage(), collision.gameObject.GetInstanceID());
            }
            //var script = collision.gameObject.GetComponents(typeof(MonoBehaviour)).First();

        }

    }
}
