using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour {
    private Camera camera;
    private void Awake() {
        camera = Camera.main;
    }

    private void Update() {
        ClickCheck();
    }

    private void ClickCheck() {
        if (Input.GetMouseButtonDown(0)) { 
            RaycastHit2D ray = Physics2D.GetRayIntersection(camera.ScreenPointToRay(Input.mousePosition));

            GameObject collider = ray.collider.gameObject;
            if (collider != null && collider.CompareTag("Player")) {
                collider.GetComponent<CircleClick>().OnClicked(0);
            }

        }else if (Input.GetMouseButtonDown(1)) {

            RaycastHit2D ray = Physics2D.GetRayIntersection(camera.ScreenPointToRay(Input.mousePosition));

            GameObject collider = ray.collider.gameObject;
            if (collider != null && collider.CompareTag("Player"))
            {
                collider.GetComponent<CircleClick>().OnClicked(1);
            }
        }
    }
}
