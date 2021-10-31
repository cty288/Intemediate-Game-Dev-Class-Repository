using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Heart : MonoBehaviour {
        [SerializeField] private AudioClip pickUpSound;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                GameManager.Singleton.AddLife(1);
                AudioManager.Singleton.PlayObjectSounds(pickUpSound, 1);
                Destroy(this.gameObject);
            }
        }
    }
}
