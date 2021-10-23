using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4 {
    public class TriggerCheck : MonoBehaviour {
        private int count = 0;
        public int Count => count;

        public string[] TargetLayers;

        [SerializeField]
        private List<Collider2D> colliders;
       
        public List<Collider2D> Colliders => colliders;

        [SerializeField] private bool ignoreTriggers = true;
        
        public bool Triggered
        {
            get { return count > 0; }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsInLayer(other.gameObject, TargetLayers)) {
                count++;
                colliders.Add(other);
            }

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsInLayer(other.gameObject, TargetLayers)) {
                count--;
                colliders.Remove(other);
            }

        }

        private void Update() {
            CheckNull();
        }

        private void CheckNull() {
            int index = 0;
            bool isNull = false;
            foreach (Collider2D collider in colliders) {
                if (!collider) {
                    isNull = true;
                }
            }

            if (isNull) {
                colliders.Clear();
            }
        }

        private bool IsInLayer(GameObject obj, string[] targetLayerMasks) {
            foreach (string targetLayerMask in targetLayerMasks) {
                if (LayerMask.LayerToName(obj.layer) == targetLayerMask) {
                    if (obj.GetComponent<Collider2D>().isTrigger) {
                        if (ignoreTriggers) {
                            return false;
                        }
                    }
                    return true;
                }
            }

            return false;
        }
    }
}

