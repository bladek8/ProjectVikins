using Assets.Script.Controller;
using Assets.Script.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class ArcherView : MonoBehaviour
    {
        PlayerController playerController;
        private Utils utils = new Utils();

        private Animator _playerAnimator;
        private Animator PlayerAnimator { get { return _playerAnimator ?? (_playerAnimator = GetComponent<Animator>()); } }

        private SpriteRenderer _playerSpriteRenderer;
        private SpriteRenderer PlayerSpriteRenderer { get { return _playerSpriteRenderer ?? (_playerSpriteRenderer = GetComponent<SpriteRenderer>()); } }

        //private BoxCollider2D _boxCollider2D;
        //private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }


        BoxCollider2D colliderTransform;
        KeyMove input = new KeyMove(null, new Vector2(), false);
        Vector2 mouseIn;
        Vector2 mouseOut;
        private void Start()
        {
            playerController = new PlayerController(this.gameObject);
        }

        private void FixedUpdate()
        {
            input.Vector2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            foreach (var keyMove in utils.moveKeyCode)
            {
                if (Input.GetKey(keyMove.KeyCode.Value))
                {
                    PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                    PlayerAnimator.SetFloat("speedY", input.Vector2.y);

                    transform.Translate(keyMove.Vector2 * Time.deltaTime * 2);
                    if (!keyMove.Flip.HasValue) continue;
                    PlayerSpriteRenderer.flipX = keyMove.Flip.Value;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                mouseIn =  Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                mouseOut = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Debug.Log("mouseIn: " + mouseIn + "/ mouseOut: " + mouseOut);
                Debug.Log(Vector3.Distance(mouseIn, mouseOut));

                var vectorDirection = mouseIn - mouseOut;
                var degrees = Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg;
                Debug.Log(degrees);

            }

        }
    }
}
