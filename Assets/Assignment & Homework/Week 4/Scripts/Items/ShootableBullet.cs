using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class ShootableBullet : MonoBehaviour {
        [SerializeField] 
        private List<string> ignoreParentNames = new List<string>();

        private Rigidbody2D rigidbody;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Shoot(float shootSpeed, Vector2 shootAngle) {
            rigidbody.velocity = shootAngle.normalized * shootSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            bool ignored = false;
            foreach (string ignoreParentName in ignoreParentNames) {
                if (other.gameObject.name == ignoreParentName) {
                    ignored = true;
                }
            }

            if (!ignored) {
                SimpleEventSystem.OnShot?.Invoke(other.gameObject.name);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
