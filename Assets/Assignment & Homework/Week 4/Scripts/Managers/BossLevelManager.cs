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
        private void Awake() {
            Singleton = this;
        }

        private void Update() {
            if (lastFloor != Floor) {
                SimpleEventSystem.OnPlayerFloorChange?.Invoke(lastFloor,Floor);
                lastFloor = Floor;
            }
        }
    }
}
