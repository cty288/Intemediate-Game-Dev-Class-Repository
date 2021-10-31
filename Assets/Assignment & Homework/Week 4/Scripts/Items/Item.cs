using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public enum ItemType
    {
        Bullet,
        Boot
    }
    public abstract class Item : MonoBehaviour
    {
        [SerializeField]
        private bool canPickMultipleTime = false;
        public bool CanPickMultipleTime => canPickMultipleTime;

        [SerializeField]
        private ItemType itemType;
        public ItemType ItemType => itemType;

        private SpriteRenderer spriteRenderer;
        private Collider2D collider;

        [SerializeField] 
        protected AudioClip pickUpSound;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            SimpleEventSystem.OnPlayerRespawn += OnPlayerRespawn;
        }

        private void OnDestroy()
        {
            SimpleEventSystem.OnPlayerRespawn -= OnPlayerRespawn;
        }

        private void OnPlayerRespawn()
        {
            Active();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                GameManager.Singleton.PickItem(this);

                AudioManager.Singleton.PlayObjectSounds(pickUpSound,1);
                DisActive();
            }
        }



        void DisActive()
        {
            spriteRenderer.enabled = false;
            collider.enabled = false;
        }

        void Active()
        {
            spriteRenderer.enabled = true;
            collider.enabled = true;
        }
    }
}