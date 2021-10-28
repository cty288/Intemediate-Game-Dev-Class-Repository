using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class DialogueTrigger : MonoBehaviour {
        private bool triggered = false;

        [SerializeField]
        protected DialogueType dialogueType;

        [SerializeField]
        protected List<string> dialogues = new List<string>();

        
        [SerializeField]
        protected Vector3 bubbleDialoguePosition = Vector3.zero;

        private bool dialogueFinished = false;
        public bool DialogueFinished => dialogueFinished;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space)) {
                if(dialogues.Count>0 && !dialogueFinished)
                {
                    ShowDialogue();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name == "Player" && !triggered) {
                StartCoroutine(StartDialogue());

            }
        }

        IEnumerator StartDialogue() {
            yield return new WaitForSeconds(0.1f);
            triggered = true;
            if (dialogues.Count > 0)
            {
                DialogueManager.Singleton.StartNewDialogue(dialogueType, dialogues,
                    bubbleDialoguePosition);
            }
        }
        protected void ShowDialogue()
        {
            int page = DialogueManager.Singleton.NextPage();
            if (page == -1) {
                dialogueFinished = true;
            }
        }
        /*
        public void ShowSingleMessageBubbleWithTime(string dialogueText, Transform follow, float time)
        {

            currentDialogueType = DialogueType.Bubble;
            page = 0;
            currentDialogue = new List<string>() { dialogueText };
            SetDialogueUIText(currentDialogue[page]);

        }*/
    }
}
