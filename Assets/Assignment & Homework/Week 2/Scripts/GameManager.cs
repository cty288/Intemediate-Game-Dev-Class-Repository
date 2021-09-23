using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Menu,
    Start
}
public class GameManager : MonoBehaviour {
    public static GameManager Singleton;

    public float MinX = -9;
    public float MaxX = 46;

    public float cameraMinX =0 ;
    public float cameraMaxX = 37;

    public GameState GameState = GameState.Menu;
    
    private void Awake() {
        Singleton = this;
    }

    
}
