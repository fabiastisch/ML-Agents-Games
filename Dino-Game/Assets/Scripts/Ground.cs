using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {
    //private BoxCollider2D _collider;
    public Action OnEnterGround;
    public Action OnExitGround;

    // Start is called before the first frame update
    void Start() {
        //_collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            OnEnterGround.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            OnExitGround.Invoke();
        }
    }
}