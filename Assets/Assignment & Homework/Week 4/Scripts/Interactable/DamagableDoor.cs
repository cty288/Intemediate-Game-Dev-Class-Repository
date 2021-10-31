using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class DamagableDoor : MonoBehaviour,IDamageable {
        [SerializeField] private AudioClip damagedSound;
        private void Awake() {
            health = MaxHealth;
        }

        public void DealDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                AudioManager.Singleton.PlayObjectSounds(damagedSound,0.5f);
                Destroy(this.gameObject);
            }
        }

        public int Health {
            get {
                return health;
            }
        }
        private int health;
        
        public int MaxHealth { get; } = 200;
    }
}
