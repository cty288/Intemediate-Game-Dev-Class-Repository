using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {
    private Button startButton;
    private SpriteRenderer otter;
    private Image bg;

    [SerializeField]
    private Sprite[] otterSprites;

    private bool fade = false;
    private void Awake() {
        otter = GameObject.Find("Otter").GetComponent<SpriteRenderer>();
        startButton = GetComponentInChildren<Button>();
        bg = GetComponentInChildren<Image>();
    }

    private void Start() {
        otter.sprite = otterSprites[0];
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked() {
        GameManager.Singleton.GameState = GameState.Start;
        startButton.gameObject.SetActive(false);
        GetComponentInChildren<TMP_Text>().gameObject.SetActive(false);
        fade = true;
        otter.sprite = otterSprites[1];
    }

    private void Update() {
        if (fade) {
            bg.color = Color.Lerp(bg.color, new Color(bg.color.r, bg.color.g,
                bg.color.b, 0), 0.1f);

            if (bg.color.a <= 0.05f) {
                fade = false;
            }
        }
    }

    public void OnPointerDown() {
        otter.sprite = otterSprites[1];
    }

    public void OnPointerExit() {
        if (GameManager.Singleton.GameState == GameState.Menu) {
            otter.sprite = otterSprites[0];
        }
    }
}
