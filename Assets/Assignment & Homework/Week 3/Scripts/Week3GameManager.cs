using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Week3GameManager : MonoBehaviour {
    public enum GameState {
        Menu,
        Start
    }
    public static Week3GameManager Singleton;

    public float TimeSpeed = 1;
    public int Stage = 1;
    public GameState state=GameState.Menu;

    private void Awake() {
        Singleton = this;
    }

    private void Update() {
        Stage = Mathf.Clamp(Stage, 1, 5);
        if (Input.GetKeyDown(KeyCode.Space)) {
            state = GameState.Start;
        }

        if (state == GameState.Menu) {
            Time.timeScale = 0;
        }

        if (state == GameState.Start) {
            Time.timeScale = TimeSpeed;
        }
    }
}
