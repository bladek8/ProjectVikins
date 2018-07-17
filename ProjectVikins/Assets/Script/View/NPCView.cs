using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Helpers;
using UnityEngine.Events;

namespace Assets.Script.View
{
    public class NPCView : MonoBehaviour
    {
        [SerializeField] UnityEvent startInteraction;
        [SerializeField] UnityEvent endInteraction;

        BoxCollider2D colliderTransform;
        public Image dialogBox;
        public Text npcText;
        public float letterPause = 0.2f;
        public static bool canInteract = true;
        Coroutine textCoroutine;

        private void Start()
        {
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
        }

        public void Interaction()
        {
            textCoroutine = StartCoroutine(TypeText());
            dialogBox.enabled = true;
            startInteraction.Invoke();
        }

        private void FixedUpdate()
        {
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
            if (Input.GetButtonDown("Interaction"))
            {
                canInteract = true;
                dialogBox.enabled = false;
                npcText.text = "";
                endInteraction.Invoke();
                StopCoroutine(textCoroutine);
            }
        }

        IEnumerator TypeText()
        {
            foreach (char letter in ("Bem vindo Harry!").ToCharArray())
            {
                npcText.text += letter;
                yield return 0;
                yield return new WaitForSeconds(letterPause);
            }
        }
    }
}
