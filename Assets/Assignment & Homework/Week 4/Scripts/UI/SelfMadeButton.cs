using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Week4
{
    public class SelfMadeButton : MonoBehaviour {
        private bool activated = false;
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private BoxCollider2D boxCollider;
        private Animation anim;

        public UnityEvent onClicked;
        private Camera cam;
        private void Awake()
        {
            cam = Camera.main;

            anim = GetComponent<Animation>();
            spriteRenderer = GetComponent<SpriteRenderer>();
          

            DisActive();
        }

        private void Update()
        {
            ClickCheck();
        }

        private void ClickCheck()
        {
            if (Input.GetMouseButtonDown(0) && activated)
            {
                RaycastHit2D ray = Physics2D.GetRayIntersection(cam.ScreenPointToRay(Input.mousePosition));

                Collider2D collider = ray.collider;

                if (collider != null && collider.gameObject == gameObject)
                {
                    OnClicked();
                }

            }
        }
      

        public void Activate() {
            activated = true;
            boxCollider.enabled = true;

            if (anim) {
                anim.Play();
            }
        }

        public void DisActive() {
            activated = false;
            boxCollider.enabled = false;
            spriteRenderer.color = new Color(1,1,1,0);
            
            if (anim)
            {
                anim.Stop();
            }
        }

        public void OnClicked() {
            onClicked?.Invoke();
        }

        public void RestartCurrentLevel() {
            DisActive();
            if (GameManager.Singleton.Life <= 0) {
                GameManager.Singleton.ResetToFirstLevel();
            }
            else {
                GameManager.Singleton.Respawn();
            }

        }

        public void NextLevel() {
            DisActive();
            GameManager.Singleton.GoToNextLevel();
        }
    }
}
