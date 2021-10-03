using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChangeColor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        string tag = other.collider.tag;
        if (tag == "Player" || tag == "SmallBall") {
            Color randomColor = new Color(Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f));

            GetComponent<SpriteRenderer>().color = randomColor;
        }
    }
}
