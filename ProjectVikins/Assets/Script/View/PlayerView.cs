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

        private Animator _playerAnimator;
        private Animator PlayerAnimator { get { return _playerAnimator ?? (_playerAnimator = GetComponent<Animator>()); } }

        private SpriteRenderer _playerSpriteRenderer;
        private SpriteRenderer PlayerSpriteRenderer { get { return _playerSpriteRenderer ?? (_playerSpriteRenderer = GetComponent<SpriteRenderer>()); } }

        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }
        BoxCollider2D boxColliderAttack;

        float inputX, inputY;

        private void Start()
        {
            boxColliderAttack = this.gameObject.AddComponent<BoxCollider2D>();
            playerController = new PlayerController(new DAL.Player { PlayerId = this.GetInstanceID(), Life = 2, CharacterTypeId = 5, SpeedRun = 3, SpeedWalk = 3, AttackMin = 1, AttackMax = 1 }, this.gameObject);
            boxColliderAttack = BoxCollider2D;
            boxColliderAttack.isTrigger = true;
        }

        private void FixedUpdate()
        {
            foreach (var keyMove in playerController.utils.moveKeyCode)
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

            if (Input.GetKey(KeyCode.L))
                boxColliderAttack.offset = playerController.Attack(boxColliderAttack.size);
            if (Input.GetKeyUp(KeyCode.L))
                boxColliderAttack.offset = new Vector2(0, 0);

            playerController.SetLastMoviment(inputX, inputY);
            PlayerAnimator.SetFloat("speedX", inputX);
            PlayerAnimator.SetFloat("speedY", inputY);
        }

        private void Update()
        {
            foreach (var keyMove in playerController.utils.moveKeyCode)
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
            if(collision.tag == "Enimy" && Input.GetKey(KeyCode.L))
            {
                var script = collision.gameObject.GetComponents(typeof(MonoBehaviour)).First();
                playerController.GiveDamage(script, 1);
            }
        }

    }
}
