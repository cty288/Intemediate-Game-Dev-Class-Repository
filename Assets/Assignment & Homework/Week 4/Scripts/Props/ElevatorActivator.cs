using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Week4
{
    public class ElevatorActivator : InteractableObject {
        [SerializeField] 
        private Elevator elevator;

        [SerializeField] private float activationWaitTime = 0.5f;

        [SerializeField] private bool showCountdown = false;

        private float countDown;
        private TMP_Text infoText;

        private bool onGround = true;

        protected override void Awake() {
            base.Awake();
            infoText = transform.Find("InfoUI/Info/Canvas/InfoText").GetComponent<TMP_Text>();
        }

        protected override void OnStageChanged(int stage) {
            
        }

        protected override void OnInteract(int stage) {
            
            countDown = activationWaitTime;
            StartCoroutine(ChangeElevatorState());
        }

        protected override void Update() {
            base.Update();
            countDown -= Time.deltaTime;

            if (showCountdown) {
                if (countDown > 0) {
                    infoText.transform.localScale = new Vector3(1, 1, 1);
                    int countInteger = Mathf.RoundToInt(countDown);
                    countInteger = Mathf.Max(0, countInteger);
                    infoText.text = countInteger.ToString();
                }
                else {
                    infoText.transform.localScale = new Vector3(2, 1, 1);
                    infoText.text = "!";
                }
                
            }
            else {
                infoText.transform.localScale = new Vector3(2, 1, 1);
                infoText.text = "!";
            }
        }

        IEnumerator ChangeElevatorState() {
            yield return new WaitForSeconds(activationWaitTime);
            onGround = !onGround;
            elevator.OnGround = onGround;
        }
    }
}
