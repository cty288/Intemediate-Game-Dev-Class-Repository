using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Villain : InteractableObject {

        [SerializeField] protected List<string> stage2Dialogues;

        protected override void OnStageChanged(int stage) {
            dialogues = stage2Dialogues;
        }
    }
}
