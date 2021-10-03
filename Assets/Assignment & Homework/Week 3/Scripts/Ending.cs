using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour {
    public GameObject balls;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Ball")
        {
           balls.gameObject.SetActive(true);
        }
    }
}
