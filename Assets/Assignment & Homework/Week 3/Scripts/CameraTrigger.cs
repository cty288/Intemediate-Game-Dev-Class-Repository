using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {
    public Vector2 TargetPosition;
    public float lerp = 0.5f;

    public float size = 5f;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Ball") {
            Week3.CameraControl cameraControl = Camera.main.GetComponent<Week3.CameraControl>();

            cameraControl.TargetPosition = TargetPosition;
            cameraControl.lerp = lerp;
            cameraControl.targetSize = size;

            Week3GameManager.Singleton.Stage++;
        }
    }
}
