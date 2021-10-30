using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Slime : Enemy {
        [SerializeField] 
        private int size = 2;
        [SerializeField]
        private GameObject middleSlimePrefab;

        [SerializeField] 
        private GameObject smallSlimePrefab;

        [SerializeField] 
        private float defaultAnimationSpeed = 1.5f;

        private float currentAnimationSpeed;
        protected override void Start() {
            if (targetX == 0) {
                targetX = posXLimit.y;
            }
            
            player = GameManager.Singleton.Player;
            healthBarInitialScale = healthBarCanvasTr.localScale.x;
            SimpleEventSystem.OnPlayerStateUpdate += OnPlayerStateUpdate;
            SimpleEventSystem.OnPlayerRespawn += OnPlayerRespawn;
            SimpleEventSystem.OnBossDie += OnBossDie;
            currentAnimationSpeed = defaultAnimationSpeed;
        }

        protected override void AnimationControl() {
            switch (state) {
                case EnemyState.Chasing:
                    currentAnimationSpeed = defaultAnimationSpeed * 1.3f;
                    break;
                case EnemyState.Patrol:
                    currentAnimationSpeed = defaultAnimationSpeed;
                    break;
            }
            animator.speed = currentAnimationSpeed;
        }

        protected override void OnBossDie() {
            if (Alive)
            {
                health = 0;
                for (int i = 0; i < awardDiamondCount; i++)
                {
                    float randomPos = Random.Range(-1f, 1f);
                    GameManager.Singleton.SpawnDiamond(transform.position + new Vector3(0, 2.5f, 0) +
                                                       new Vector3(randomPos, randomPos, randomPos));
                }


                GetComponent<CircleCollider2D>().isTrigger = true;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -50);

                Instantiate(smokePrefab, transform.position, Quaternion.identity);

                Destroy(this.gameObject, 5);
            }
        }

        protected override void OnKilled() {
            Instantiate(smokePrefab, transform.position, Quaternion.identity);
            for (int i = 0; i < awardDiamondCount; i++)
            {
                float randomPos = Random.Range(-1f, 1f);
                GameManager.Singleton.SpawnDiamond(transform.position + new Vector3(0, 2.5f, 0) +
                                                   new Vector3(randomPos, randomPos, randomPos));
            }

            GameManager.Singleton.TotalEnemiesKilled++;
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

            if (size == 0) {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -20);
                Destroy(this.gameObject,5);
            }
            else {
                SlimeSplit();
                Destroy(this.gameObject);
            }
        }

        private void SlimeSplit() {
            for (int i = 0; i < 2; i++) {
                Slime slime = null;
                if (size == 2) {
                    slime =  Instantiate(middleSlimePrefab, transform.position +
                                                   i * new Vector3(Random.Range(1f, 1.5f), 0, 0),
                        Quaternion.identity).GetComponent<Slime>();
                    
                }
                else if (size == 1) {
                    slime = Instantiate(smallSlimePrefab, transform.position +
                                                                     i * new Vector3(Random.Range(1f, 1.5f), 0, 0),
                        Quaternion.identity).GetComponent<Slime>();
                }

                if (slime) {
                    slime.posXLimit = this.posXLimit;
                    slime.targetX = this.targetX;
                    slime.patrolSpeed += Random.Range(-0.8f, 0.8f);
                }
              
            }
        }
    }
}
