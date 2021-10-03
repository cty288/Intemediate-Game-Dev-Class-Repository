using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    private float timer = 0;

    private GameObject info;

    private void Awake() {
        info = transform.Find("Info").gameObject;
    }

    private void Update() {
        if (Week3GameManager.Singleton.state == Week3GameManager.GameState.Start) {
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
            timer += Time.deltaTime;
            //info.SetActive(true);
            if (timer >= 0.5) {
                timer = 0;
                info.SetActive(!info.activeInHierarchy);
            }
        }
    }
}
