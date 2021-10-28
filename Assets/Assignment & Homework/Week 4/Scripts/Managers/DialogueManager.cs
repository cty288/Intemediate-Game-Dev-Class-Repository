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

        
        private SpeechUI speechUI;

        private MessageBox messageBoxUI;

        private void Awake() {
            if (Singleton != null) {
                DestroyImmediate(this.gameObject);
            }
            else {
                Singleton = this;
                DontDestroyOnLoad(this);
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
            }
            
        }

        private void Update() {
            if (!player) {
                player = GameManager.Singleton.GetPlayer();
            }

            if (!speechUI) {
                speechUI = GameObject.FindGameObjectWithTag("SpeechBubble").GetComponent<SpeechUI>();
            }

            if (!messageBoxUI) {
                messageBoxUI = GameObject.Find("MessageBox").GetComponent<MessageBox>();
            }

            if (player.PlayerState != PlayerState.Dead) {
                if (page >= 0)
                {
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
            else {
                player.PlayerState = PlayerState.Dead;
                CloseUI();
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

                    if (text.Contains("[R]")) {
                        speechUI.SetColor(Color.red);
                        text = text.Substring(3);
                    }
                    else {
                        speechUI.SetColor(Color.black);
                    }
                    speechUI.SetText(text);
                    break;
                case DialogueType.Textbox:
                    messageBoxUI.Activate(text);
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
                    messageBoxUI.Disactivate();
                    break;
            }
        }
    }
}
