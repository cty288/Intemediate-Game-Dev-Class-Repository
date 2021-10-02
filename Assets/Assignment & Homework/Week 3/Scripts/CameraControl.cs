using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week3 {
    public class CameraControl : MonoBehaviour {
        public Vector2 TargetPosition;

        public float lerp = 0.5f;
        public float targetSize = 5f;

        private Camera camera;

        private void Awake() {
            camera = GetComponent<Camera>();
        }

        private void Update() {
            Vector3 target = new Vector3(TargetPosition.x, TargetPosition.y, -10);
            transform.position = Vector3.Lerp(transform.position, target, lerp*Time.deltaTime);

            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, lerp * Time.deltaTime);
        }
    }

}
