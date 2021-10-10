using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Key : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Singleton.ChangeKey(1);
                Destroy(this.gameObject);
            }
        }
    }
}
