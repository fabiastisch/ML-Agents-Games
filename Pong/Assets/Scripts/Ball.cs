using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
    private Rigidbody2D _rb;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start() { 
        _rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }
    
    private Vector2 getScalesSpeedVector2(Vector2 vector)
    {
        float vectorLength = vector.magnitude;
        float multi = speed / vectorLength;
        return vector * multi;
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        _rb.velocity = getScalesSpeedVector2(_rb.velocity);
    }
    
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("LeftGoal")) {
            
        }else if (other.CompareTag("RightGoal")) {
            
        }

        ResetBall();
    }

    private void ResetBall() {
        transform.localPosition = new Vector3(0, 0, 0);
        float x = Random.Range(-1,1) == 0 ? 1 : -1;
        float y = Random.Range(-0.5f, 0.5f);
        _rb.velocity = getScalesSpeedVector2(new Vector2(x, y));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("OnCollisionEnter2D" + other.gameObject);
    }
}