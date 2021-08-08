using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Paddle : MonoBehaviour {
    private Rigidbody2D _rb;
    [SerializeField] float _speedMultiplier = 50f;

    public Ball ball;

    private float _movement;

    [SerializeField] public bool isLeftPaddle;

    // Start is called before the first frame update
    void Start() {
        _rb = this.GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update() {
        // _movement = Input.GetAxis("Vertical");
    }

    public void Move(float movement) {
        _movement = movement;
    }

    private void FixedUpdate() {
        _rb.velocity = new Vector2(0.0f, _movement) * _speedMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ball")) {
        }
        else {
            _rb.velocity = Vector2.zero;
        }

        //Debug.Log("PADDLE: OnTriggerEnter2D: " + other.gameObject);
    }
}