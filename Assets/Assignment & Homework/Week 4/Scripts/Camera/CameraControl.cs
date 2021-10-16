using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class CameraControl : MonoBehaviour
    {
        private Transform player;
        private Camera cam;
        [SerializeField]
        private float lerpSpeed = 20;

        [SerializeField] private Vector2 cameraPositionXRange = new Vector2(0, 100);
        [SerializeField] private Vector2 cameraPositionYRange = new Vector2(0, 100);

        private bool dead = true;
        private Vector2 deadZonePlayerPosition;
        [SerializeField]private float deadZoneDistance=1;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            cam = GetComponent<Camera>();
        }

        private void Start() {
            SimpleEventSystem.OnPlayerStateUpdate += OnPlayerStateUpdate;
        }

        private void OnPlayerStateUpdate(PlayerState oldState, PlayerState newState) {
            if (oldState == PlayerState.Idle) {
                dead = true;
                deadZonePlayerPosition = player.position;
            }
        }

        private void Update()
        {
            if (!dead) {
                float targetX = transform.position.x;
                targetX = Mathf.Lerp(targetX, player.position.x, lerpSpeed * Time.deltaTime);
                targetX = Mathf.Clamp(targetX, cameraPositionXRange.x, cameraPositionXRange.y);

                float targetY = transform.position.y;
                targetY = Mathf.Lerp(targetY, player.position.y, lerpSpeed * Time.deltaTime);
                targetY = Mathf.Clamp(targetY, cameraPositionYRange.x, cameraPositionYRange.y);

                transform.position = new Vector3(targetX, targetY, transform.position.z);
            }
            else {
                float distance = Vector2.Distance(player.position, deadZonePlayerPosition);
                if (distance >= deadZoneDistance) {
                    dead = false;
                }
            }
            
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateUpdate;
        }
    }
}
