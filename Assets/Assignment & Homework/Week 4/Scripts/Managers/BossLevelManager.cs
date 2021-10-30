using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class BossLevelManager : MonoBehaviour {
        public static BossLevelManager Singleton;

        public int Floor = 1;
        private int lastFloor = 1;


        private bool dialogueTriggered = false;


        [SerializeField]
        protected List<string> endingDialogues = new List<string>();

        private bool dialogueFinished = false;
        public bool DialogueFinished => dialogueFinished;

        [SerializeField] private GameObject gamePassDoorPrefab;
        [SerializeField] private Vector2 gamePassDoorPos;


        private void Awake() {
            Singleton = this;
        }

        private void Start() {
            SimpleEventSystem.OnBossDie += OnBossDie;
        }

        private void OnBossDie() {
            StartCoroutine(BossDieEventTrigger());
            GameObject gamepassDoor = Instantiate(gamePassDoorPrefab, gamePassDoorPos,
                Quaternion.identity);


        }

        IEnumerator BossDieEventTrigger() {
            yield return new WaitForSeconds(1f);
            dialogueTriggered = true;
            if (endingDialogues.Count > 0)
            {
                DialogueManager.Singleton.StartNewDialogue(DialogueType.Bubble, endingDialogues,
                    Vector3.zero);
            }
        }

        private void Update() {
            if (lastFloor != Floor) {
                SimpleEventSystem.OnPlayerFloorChange?.Invoke(lastFloor,Floor);
                lastFloor = Floor;
            }

            if (dialogueTriggered) {
                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))
                {
                    if (endingDialogues.Count > 0 && !dialogueFinished)
                    {
                        ShowDialogue();
                    }
                }
            }
            
        }

        protected void ShowDialogue()
        {
            int page = DialogueManager.Singleton.NextPage();
            if (page == -1)
            {
                dialogueFinished = true;
            }
        }

        private void OnDestroy() {
            SimpleEventSystem.OnBossDie -= OnBossDie;
        }
    }
}
