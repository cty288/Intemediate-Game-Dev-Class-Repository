using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Week4
{
    public class EnemySpawner : MonoBehaviour {
        private List<Enemy> aliveEnemies;
        private float timer = 0;

        [SerializeField] 
        private List<GameObject> spawnPrefabs;

        [SerializeField] 
        private Vector2 spawnXLimits;

        [SerializeField] 
        private Transform spawnPosition;

        [SerializeField] 
        private float spawnInterval = 5f;

        [SerializeField] 
        private int maxEnemyConcurrence = 4;

        private SpriteRenderer indicator;
        private float indicateTimer = 0;
        private void Awake() {
            aliveEnemies = new List<Enemy>();
            indicator = transform.Find("Indicator").GetComponent<SpriteRenderer>();

        }

        private void Update() {
            timer += Time.deltaTime;
            if (timer >= spawnInterval) {
                timer = 0;
                indicator.color = new Color(1, 0, 0);
                indicateTimer = 0;
                if (aliveEnemies.Count < maxEnemyConcurrence) {
                    SpawnEnemy();
                    
                }
            }

            if (timer >= spawnInterval * 0.7f) {
                indicateTimer += Time.deltaTime;
                if (indicateTimer >= 0.1f) {
                    indicateTimer = 0;
                    Color color = indicator.color;
                    indicator.color = new Color(Mathf.Abs(color.r - 1), 0, 0);
                }
            }
        }

        private void SpawnEnemy() {
            int randomIndex = Random.Range(0, spawnPrefabs.Count);
            GameObject spawnedPrefab = Instantiate(spawnPrefabs[randomIndex],
                spawnPosition.position, Quaternion.identity);

            Enemy spawnedEnemy = spawnedPrefab.GetComponent<Enemy>();
            spawnedEnemy.transform.position = spawnPosition.position;
            aliveEnemies.Add(spawnedEnemy);
            spawnedEnemy.OnEnemyDie += OnEnemyDie;
            spawnedEnemy.PosXLimit = spawnXLimits;
        }

        private void OnEnemyDie(Enemy enemy) {
            aliveEnemies.Remove(enemy);
            enemy.OnEnemyDie -= OnEnemyDie;
        }
    }
}
