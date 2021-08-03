using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] private float force;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(Vector3.left * force, ForceMode2D.Impulse);
        rb.velocity = Vector3.left * force;
    }

    private void FixedUpdate() {
        
    }
}