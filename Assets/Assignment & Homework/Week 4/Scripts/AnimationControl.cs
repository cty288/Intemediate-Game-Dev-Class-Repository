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

        private PlayerState currentPlayerState;
        private PlayerState lastPlayerState;

        private void Awake() {
            player = GetComponent<PlayerControl>();
            currentPlayerState = player.PlayerState;
            lastPlayerState = currentPlayerState;
        }

        private void Update() {
            currentPlayerState = player.PlayerState;
            if (lastPlayerState != currentPlayerState) {
                timer = 0;
                currentSpriteIndex = 0;
                lastPlayerState = currentPlayerState;
            }
            else {
                timer += Time.deltaTime;
                if (timer >= frameLength) {
                    timer = 0;

                    currentSpriteIndex++;
                    currentSpriteIndex %= currentSprites.Length;
                }
            }

            UpdateSpriteAtlas();

            transform.localScale = faceRight ? new Vector3(6, 6, 1) : new Vector3(-6, 6, 1);
            player.gameObject.GetComponent<SpriteRenderer>().sprite = currentSprites[currentSpriteIndex];
        }

        private void UpdateSpriteAtlas() {
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
            }

            if (player.Speed > 0) {
                faceRight = true;
            }

            if (player.Speed < 0) {
                faceRight = false;
            }
        }
    }
}
