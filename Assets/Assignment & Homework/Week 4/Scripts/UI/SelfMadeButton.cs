using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;


namespace Week4
{
    public class SelfMadeButton : MonoBehaviour {
        private bool activated = false;
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private BoxCollider2D boxCollider;
        private Animation animation;

        public UnityEvent onClicked;
        private Camera camera;
        private void Awake()
        {
            camera = Camera.main;

            animation = GetComponent<Animation>();
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
                RaycastHit2D ray = Physics2D.GetRayIntersection(camera.ScreenPointToRay(Input.mousePosition));

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

            if (animation) {
                animation.Play();
            }
        }

        public void DisActive() {
            activated = false;
            boxCollider.enabled = false;
            spriteRenderer.color = new Color(1,1,1,0);
            
            if (animation)
            {
                animation.Stop();
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
