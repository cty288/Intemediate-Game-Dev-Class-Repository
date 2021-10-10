using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class ElevatorActivator : InteractableObject {
        [SerializeField] 
        private Elevator elevator;

        [SerializeField] private float activationWaitTime = 0.5f;

        private bool onGround = true;
        protected override void OnStageChanged(int stage) {
            
        }

        protected override void OnInteract(int stage) {
            onGround = !onGround;

            StartCoroutine(ChangeElevatorState());
        }

        IEnumerator ChangeElevatorState() {
            yield return new WaitForSeconds(activationWaitTime);
            elevator.OnGround = onGround;
        }
    }
}
