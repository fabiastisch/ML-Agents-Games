using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : MonoBehaviour {
    [SerializeField] private float jumpForce; // 700 at Gravity Scale 2
    [SerializeField] private GameManager _gameManager;
    private Rigidbody2D _rb;
    private BoxCollider2D _collider2D;
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private float _jumpHeight = 5f;

    private Vector2 _velocity;
    public bool _isGrounded;
    [SerializeField] private GameObject _ground;
    private Ground _groundScript;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _groundScript = _ground.GetComponent<Ground>();
        _groundScript.OnEnterGround += () => _isGrounded = true;
        _groundScript.OnExitGround += () => _isGrounded = false;
    }

    // Update is called once per frame
    void Update() {

        //  var overlapCircle = Physics2D.OverlapCircleAll(_ground.transform.position, 10, LayerMask.NameToLayer("Ground"));
        //Debug.Log(Physics2D.OverlapCircle(transform.position, 10f, 8));

        if (_isGrounded && _velocity.y < 0) {
            _velocity.y = -2f;
        }
        //CheckJump();

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
            //Jump();
        }

        _velocity.y += _gravity * Time.deltaTime; // Movement Y Direction (Jump & Gravity)
        _rb.velocity = _velocity;
    }

    public void Jump() {
        if (!_isGrounded) {
            return;
        }
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //_gameManager.ResetGame();
    }
}