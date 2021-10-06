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

        private bool IsInLayer(GameObject obj, string[] targetLayerMasks) {
            foreach (string targetLayerMask in targetLayerMasks) {
                if (LayerMask.LayerToName(obj.layer) == targetLayerMask) {
                    return true;
                }
            }

            return false;
        }
    }
}

