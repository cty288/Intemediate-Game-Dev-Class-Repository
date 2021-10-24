using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Week4
{
    public class SpikeBallGenerator : MonoBehaviour {
        [SerializeField] private Transform generatePosition;
        [SerializeField] private float initialYSpeed = 5f;
        [SerializeField] private float generateInterval = 3f;
        [SerializeField] private GameObject spikeBallPrefab;

        private float timer = 0;

        private void Update() {
            timer += Time.deltaTime;
            if (timer >= generateInterval) {
                timer = 0;
                GenerateSpikeball();
            }
        }

        private void GenerateSpikeball() {
            GameObject spikeBall = Instantiate(spikeBallPrefab, generatePosition.position,
                Quaternion.identity);
            spikeBall.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -initialYSpeed);
            spikeBall.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-360f, 360f);
            Destroy(spikeBall,30f);
        }
    }
}
