using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{

    public class AnimationControl : MonoBehaviour {
        private PlayerControl player;

        [SerializeField] private Sprite[] idleSprites;
        [SerializeField] private Sprite[] runSprites;
        [SerializeField] private Sprite[] jumpSprites;
        [SerializeField] private float frameLength = 0.3f;

        private float timer = 0;
        private bool faceRight = true;
        private int currentSpriteIndex = 0;
        private Sprite[] currentSprites;

        

        private void Awake() {
            player = GetComponent<PlayerControl>();
            SimpleEventSystem.OnPlayerStateUpdate += OnPlayerStateChanged;
        }

        private void Start() {
            currentSprites = idleSprites;
        }

        private void OnPlayerStateChanged(PlayerState oldState, PlayerState newState) {
            timer = 0;
            currentSpriteIndex = 0;
            UpdateSpriteAtlas(newState);
        }

        private void Update() {
            
            timer += Time.deltaTime;
            if (timer >= frameLength) {
                timer = 0;

                currentSpriteIndex++;
                currentSpriteIndex %= currentSprites.Length;
            }

            if (player.Speed > 0)
            {
                faceRight = true;
            }

            if (player.Speed < 0)
            {
                faceRight = false;
            }


            transform.localScale = faceRight ? new Vector3(6, 6, 1) : new Vector3(-6, 6, 1);
            player.gameObject.GetComponent<SpriteRenderer>().sprite = currentSprites[currentSpriteIndex];
        }

        private void UpdateSpriteAtlas(PlayerState currentPlayerState) {
            switch (currentPlayerState) {
                case PlayerState.Idle:
                    currentSprites = idleSprites;
                    break;
                case PlayerState.Run:
                    currentSprites = runSprites;
                    break;
                case PlayerState.Jump:
                    currentSprites = jumpSprites;
                    break;
                case PlayerState.Dead:
                    break;
                case PlayerState.Talking:
                    currentSprites = idleSprites;
                    break;
                
            }

           
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateChanged;
        }
    }
}
