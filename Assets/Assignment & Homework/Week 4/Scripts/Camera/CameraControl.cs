using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class CameraControl : MonoBehaviour
    {
        private PlayerControl player;
        private Camera cam;
        [SerializeField]
        private float lerpSpeed = 20;

        [SerializeField] private Vector2 cameraPositionXRange = new Vector2(0, 100);
        [SerializeField] private Vector2 cameraPositionYRange = new Vector2(0, 100);

        private bool dead = true;
        private Vector2 deadZonePlayerPosition;
        [SerializeField]private float deadZoneDistance=1;

        private float currentLerpSpeed;

        private bool gameCamera = false;
        private void Awake() {
            
            cam = GetComponent<Camera>();
        }

        private void Start() {
            player = GameManager.Singleton.GetPlayer();
            SimpleEventSystem.OnPlayerStateUpdate += OnPlayerStateUpdate;
            SimpleEventSystem.OnGameStart += OnGameStart;
        }

        private void OnGameStart() {
            StartCoroutine(FromMenuToGame());
        }

        private void OnPlayerStateUpdate(PlayerState oldState, PlayerState newState) {
            if (oldState == PlayerState.Idle) {
                dead = true;
                deadZonePlayerPosition = player.transform.position;
            }
        }


        private IEnumerator FromMenuToGame() {
            yield return new WaitForSeconds(1);
            gameCamera = true;
        }

        private void Update()
        {
            if (gameCamera) {
                if (!dead) {
                    currentLerpSpeed = Mathf.Lerp(currentLerpSpeed, lerpSpeed, 0.05f * Time.deltaTime);
                    float targetX = transform.position.x;
                    targetX = Mathf.Lerp(targetX, player.transform.position.x, currentLerpSpeed * Time.deltaTime);
                    targetX = Mathf.Clamp(targetX, cameraPositionXRange.x, cameraPositionXRange.y);

                    float targetY = transform.position.y;
                    targetY = Mathf.Lerp(targetY, player.transform.position.y, currentLerpSpeed * Time.deltaTime);
                    targetY = Mathf.Clamp(targetY, cameraPositionYRange.x, cameraPositionYRange.y);

                    transform.position = new Vector3(targetX, targetY, transform.position.z);
                }
                else {
                    float distance = Vector2.Distance(player.transform.position, deadZonePlayerPosition);
                    if (distance >= deadZoneDistance)
                    {
                        dead = false;
                    }
                }
            }
            else {
                currentLerpSpeed = 0.5f;
            }

        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateUpdate;
            SimpleEventSystem.OnGameStart -= OnGameStart;
        }
    }
}
