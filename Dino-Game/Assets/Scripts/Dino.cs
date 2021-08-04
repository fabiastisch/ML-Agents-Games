using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : MonoBehaviour {
    [SerializeField] private float jumpForce; // 700 at Gravity Scale 2
    private Rigidbody2D _rb;
    private BoxCollider2D _collider2D;
    [SerializeField] private bool canJump = true;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        CheckJump();

        if (canJump && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
            Jump();
        }
    }

    private void CheckJump() {
        var colliders = new List<Collider2D>();
        if (_collider2D.GetContacts(colliders) >= 1 && colliders[0].CompareTag("Ground")) {
                canJump = true;
        }
        else {
            canJump = false;
        }
    }

    public void Jump() {
        canJump = false;
        _rb.AddForce(Vector2.up * jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("DIno On Trigger");
    }
}