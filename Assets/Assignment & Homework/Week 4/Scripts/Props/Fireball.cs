using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Fireball : MonoBehaviour {
        private Animator animator;

        public GameObject Shooter;
        public float Speed;

        private void Awake() {
            animator = GetComponentInChildren<Animator>();
        }

        private void Update() {
            transform.position += transform.right * Speed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.isTrigger && other.gameObject!=Shooter) {
                Debug.Log(other.gameObject.name);
                Destroy(this.gameObject, 1f);
                animator.SetTrigger("Explode");
                if (other.gameObject.name == "Player")
                {
                    GameManager.Singleton.GetPlayer().PlayerState = PlayerState.Dead;
                }
            }
          
        }
    }
}
