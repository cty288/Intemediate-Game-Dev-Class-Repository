using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Fireball : MonoBehaviour {
        private Animator animator;

        public GameObject Shooter;

        [SerializeField] private AudioClip explodeAudioClip;

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
                Speed = 0;
                animator.SetTrigger("Explode");

                AudioManager.Singleton.PlayObjectSounds(explodeAudioClip,0.7f);

                if (other.gameObject.name == "Player")
                {
                    GameManager.Singleton.GetPlayer().KillPlayer();
                }else if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
                    enemy.DealDamage(20);
                }
            }
          
        }
    }
}
