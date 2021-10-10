using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Week4
{
    public class CallbackInteraction : InteractableObject {
        [SerializeField] private UnityEvent callback;
        protected override void OnStageChanged(int stage) {
            
        }

        protected override void OnInteract(int stage) {
            callback?.Invoke();
        }
    }
}
