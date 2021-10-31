using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Spikeball : MonoBehaviour {
        [SerializeField] private AudioClip ballSound;
        private void Awake() {
            SimpleEventSystem.OnPlayerRespawn += OnPlayerRespawn;
        }

        private void OnPlayerRespawn() {
            Destroy(this.gameObject);
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerRespawn -= OnPlayerRespawn;
        }

        private void OnCollisionEnter2D(Collision2D other) {

            if (Vector2.Distance(transform.position, GameManager.Singleton.GetPlayer().transform.position)
                <= 15) {
                AudioManager.Singleton.PlayObjectSounds(ballSound, 0.9f);
            }
            
            
           
            if (other.collider.gameObject.name == "Player") {
                GameManager.Singleton.GetPlayer().KillPlayer();
            }else if (other.collider.GetComponent<Enemy>()) {
                other.collider.GetComponent<Enemy>().DealDamage(500);
            }
        }
    }
}
