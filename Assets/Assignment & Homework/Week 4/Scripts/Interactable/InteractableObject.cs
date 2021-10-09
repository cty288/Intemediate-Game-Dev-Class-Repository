using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public enum DialogueType {
        Bubble,
        Textbox
    }
    public abstract class InteractableObject : MonoBehaviour {
        protected InfoUI infoUI;
        [SerializeField]
        protected DialogueType dialogueType;

        [SerializeField] 
        protected List<string> dialogues = new List<string>();

        protected bool dialogueTriggered = false;

        [SerializeField] 
        protected bool alwaysShowInfoUI = false;

        [SerializeField] 
        protected Vector3 bubbleDialoguePosition = Vector3.zero;

        protected Transform playerTransform;

        [SerializeField] 
        protected bool canRepeatTrigger = true;

        private bool triggeredBefore = false;

        private int stage = 0;
        protected virtual void Awake() {
            infoUI = transform.GetComponentInChildren<InfoUI>();
            if (!infoUI)
            {
                Debug.LogError("Could not find InfoUI!");
            }

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start() {
            if (alwaysShowInfoUI) {
                ActivateInfoUI(true);
            }
        }

        private void Update() {
          
            if (Vector3.Distance(transform.position, playerTransform.position) <= 3) {
                
                if (triggeredBefore)
                {
                    if (!canRepeatTrigger)
                    {
                        infoUI.Activate(false);
                        return;
                    }
                }
                infoUI.Activate(!DialogueManager.Singleton.IsTalking());

                if (Input.GetKeyDown(KeyCode.F)) {
                    

                    if (!dialogueTriggered)
                    {
                        dialogueTriggered = true;
                        DialogueManager.Singleton.StartNewDialogue(dialogueType, dialogues,
                            bubbleDialoguePosition);
                    }
                    else
                    {
                        ShowDialogue();
                    }

                }
            }
            else {
                DialogueManager.Singleton.CloseUI();
            }
          
          
        }

        protected virtual void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                ActivateInfoUI(true);
            }
        }

       

        protected virtual void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                if (!alwaysShowInfoUI) {
                    ActivateInfoUI(false);
                }

                dialogueTriggered = false;
                
            }
        }

        protected virtual void ActivateInfoUI(bool state) {
            infoUI.Activate(state);
        }

        protected void ShowDialogue() {
            switch (dialogueType) {
                case DialogueType.Bubble:
                    int page = DialogueManager.Singleton.NextPage();
                    if (page == -1) {
                        triggeredBefore = true;
                        dialogueTriggered = false;
                        stage++;
                        OnStageChanged(stage);
                    }
                    break;
                case DialogueType.Textbox:
                    break;
                
            }
        }

        protected abstract void OnStageChanged(int stage);
    }
}
