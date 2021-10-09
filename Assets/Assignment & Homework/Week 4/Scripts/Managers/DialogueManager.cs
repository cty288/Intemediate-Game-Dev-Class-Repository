using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class DialogueManager : MonoBehaviour {
        public static DialogueManager Singleton;

        [SerializeField]
        private int page = -1;
        [SerializeField]
        private List<string> currentDialogue = new List<string>();

        private DialogueType currentDialogueType;


        private PlayerControl player;

        [SerializeField]
        private Vector3 npcBubblePosition;

        [SerializeField]
        private SpeechUI speechUI;
        private void Awake() {
            Singleton = this;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        }

        private void Update() {
            if (page >= 0) {
                player.PlayerState = PlayerState.Talking;
                switch (currentDialogueType)
                {
                    case DialogueType.Bubble:
                        UpdateBubbleUI();
                        break;
                    case DialogueType.Textbox:
                        break;
                }
            }
           
        }

        public bool IsTalking() {
            return page != -1;
        }

        public void StartNewDialogue(DialogueType type, List<string> dialogueTexts, Vector3 NPCSpeechPosition) {
            currentDialogueType = type;
            page = 0;
            currentDialogue = dialogueTexts;
            SetDialogueUIText(currentDialogue[page]);
            
            
            this.npcBubblePosition = NPCSpeechPosition;
        }

        public void StartNewDialogue(DialogueType type, List<string> dialogueTexts)
        {
            currentDialogueType = type;
            page = 0;
            currentDialogue = dialogueTexts;

            SetDialogueUIText(currentDialogue[page]);
            
            this.npcBubblePosition = Vector3.zero;
        }

        public int NextPage() {
            page++;
            if (page < currentDialogue.Count) {
                SetDialogueUIText(currentDialogue[page]);
            }
            else {
                page = -1;
                CloseUI();
                player.PlayerState = PlayerState.Idle;

            }

            return page;
        }

        void SetDialogueUIText(string text) {
            switch (currentDialogueType) {
                case DialogueType.Bubble:
                    if (text.Contains("P:"))
                    {
                        text = text.Substring(2);
                    }
                    speechUI.SetText(text);
                    break;
                case DialogueType.Textbox:
                    break;
            }
        }

        void UpdateBubbleUI() {
            string text = currentDialogue[page];
            if (text.Contains("P:")) {
                speechUI.Activate(true,player.transform.position + new Vector3(-2,2,0));
            }
            else {
                speechUI.Activate(true, npcBubblePosition);
            }
        }

        public void CloseUI() {
            page = -1;
            //player.PlayerState = PlayerState.Idle;
            switch (currentDialogueType)
            {
                case DialogueType.Bubble:
                    speechUI.Activate(false);
                    speechUI.ResetMsg();
                    break;
                case DialogueType.Textbox:
                    break;
            }
        }
    }
}
