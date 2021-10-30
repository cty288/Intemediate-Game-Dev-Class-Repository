using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class GameEndDoor : InteractableObject {
        private bool interacted = false;
        protected override void OnStageChanged(int stage) {
            
        }

        protected override void OnInteract(int stage) {
            if (!interacted) {
                interacted = true;
                SimpleEventSystem.OnEntireGameEnds?.Invoke();
                GameManager.Singleton.GetPlayer().PlayerState = PlayerState.GamePass;
                Debug.Log("Game ends");
            }
        }
    }
}
