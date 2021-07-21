using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMovement : MonoBehaviour {
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;
    private bool jump = false;
    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
            Debug.Log("Update Jump");
            Jump();
        }

    }

    public void Jump() {
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void FixedUpdate() {
        
    }
}