using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleClick : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    [SerializeField] 
    private Color color1= Color.red, color2 = Color.blue;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color1;
    }

    public void OnClicked(int index) {
        spriteRenderer.color = index == 0 ? spriteRenderer.color = color1 : spriteRenderer.color = color2;
    }
}
