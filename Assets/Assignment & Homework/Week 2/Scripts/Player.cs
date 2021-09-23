using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float speed = 4;
    [SerializeField] private float lerp = 0.5f;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float animationSpeed = 0.3f;

    private SpriteRenderer spriteRenderer;
    private float timer;
    private int currentSpriteIndex = 0;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.Singleton.GameState == GameState.Start) {
            Vector3 targetPos = transform.position;
            if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D)))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    //transform.Translate(Vector3.left*Time.deltaTime);
                    targetPos += Vector3.left * speed * Time.deltaTime;
                    transform.localScale = new Vector3(0.3525435f, 0.3525435f, 0.3525435f);
                }
                else
                {
                    targetPos += Vector3.right * speed * Time.deltaTime;
                    transform.localScale = new Vector3(-0.3525435f, 0.3525435f, 0.3525435f);
                }

                timer += Time.deltaTime;
                if (timer >= animationSpeed)
                {
                    timer = 0;
                    currentSpriteIndex++;
                    currentSpriteIndex %= sprites.Length;
                }
            }
            else
            {
                timer = 0;
                currentSpriteIndex = 0;
            }


            transform.position = Vector3.Lerp(transform.position, targetPos, lerp);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, GameManager.Singleton.MinX,
                GameManager.Singleton.MaxX), transform.position.y);

            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
        
    }
}
