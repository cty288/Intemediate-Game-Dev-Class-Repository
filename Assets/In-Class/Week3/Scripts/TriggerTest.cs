using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriggerTest : MonoBehaviour {
    private void Awake() {
        
    }

    

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name,gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
        other.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));
    }

}
