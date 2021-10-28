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

        [SerializeField] protected float minimumTriggerDistance = 3;

        private bool triggeredBefore = false;

        private int stage = 0;
        protected virtual void Awake() {
            infoUI = transform.GetComponentInChildren<InfoUI>();
            if (!infoUI)
            {
                Debug.LogError("Could not find InfoUI!");
            }

            
        }

        protected virtual void Start() {
            if (alwaysShowInfoUI) {
                ActivateInfoUI(true);
            }
            playerTransform = GameManager.Singleton.GetPlayer().transform;
        }

        protected virtual void Update() {
            
            if (Vector3.Distance(transform.position, playerTransform.position)
                <= minimumTriggerDistance || dialogueTriggered) {
                
                if (triggeredBefore)
                {
                    if (!canRepeatTrigger)
                    {
                        infoUI.Activate(false);
                        return;
                    }
                }

                if (infoUI) {
                    infoUI.Activate(!DialogueManager.Singleton.IsTalking());
                }
               

                if (Input.GetKeyDown(KeyCode.F) || (Input.GetKeyDown(KeyCode.Space) && dialogueTriggered)) {

                    if (dialogues.Count > 0) {
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
                    OnInteract(stage);

                }
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
            int page = DialogueManager.Singleton.NextPage();
            if (page == -1)
            {
                triggeredBefore = true;
                dialogueTriggered = false;
                stage++;
                OnStageChanged(stage);
            }
        }

        protected abstract void OnStageChanged(int stage);

        protected abstract void OnInteract(int stage);
    }
}
