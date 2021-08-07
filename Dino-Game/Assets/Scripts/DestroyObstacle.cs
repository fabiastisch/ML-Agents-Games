using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstacle : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("OnTrigger");
        Destroy(other.gameObject);
    }
}