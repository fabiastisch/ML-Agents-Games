using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
    private Rigidbody2D _rb;
    [SerializeField] private float speed;

    public event Action OnLeftGoal;
    public event Action OnRightGoal;

    private Vector2 prevVelocity;
    private Vector2 prev2Velocity;

    private Vector2 _velocity;


    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }

    private Vector2 getScalesSpeedVector2(Vector2 vector) {
        var normalized = vector.normalized;

        normalized.x = Utils.IsNearZero(normalized.x) ? 0.1f : normalized.x;
        normalized.y = Utils.IsNearZero(normalized.y) ? 0.1f : normalized.y;

        //float vectorLength = vector.magnitude;
        //float multi = speed / vectorLength;

        return normalized.normalized * speed;
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        _rb.velocity = getScalesSpeedVector2(_rb.velocity);
        _velocity = _rb.velocity;
        Debug.Log("FixedUpdate: normalized Velocity: " + _rb.velocity.normalized);
        CheckBallOutside();
    }

    private void CheckBallOutside() {
        var localPosition = transform.localPosition;
        var x = localPosition.x;
        var y = localPosition.y;
        if (x < -10 || x > 10 || y < -6 || y > 6) {
            ResetBall();
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("OnTriggerEnter2D: " + other.gameObject);
        if (other.CompareTag("LeftGoal")) {
            OnLeftGoal?.Invoke();
            ResetBall();
        }
        else if (other.CompareTag("RightGoal")) {
            OnRightGoal?.Invoke();
            ResetBall();
        }
    }

    private void ResetBall() {
        transform.localPosition = new Vector3(0, 0, 0);
        float x = Random.Range(-1, 1) == 0 ? 1 : -1;
        float y = Random.Range(-0.5f, 0.5f);
        _rb.velocity = getScalesSpeedVector2(new Vector2(x, y));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Paddle")) {
            float maxDist = other.transform.localScale.y * 1f * 0.5f + transform.localScale.y * 1f * 0.5f;
            float dist = transform.localPosition.y - other.transform.localPosition.y;
            float nDist = dist / maxDist;
            var temp = new Vector2(-_velocity.normalized.x, nDist);
            _rb.velocity = getScalesSpeedVector2(temp);
        }
        else if (other.gameObject.CompareTag("Wall")) {
            // var dir = transform.position - other.transform.position;
            // _rb.velocity = getScalesSpeedVector2(dir);
            var velocity = _velocity;
            //Debug.Log(_velocity);
            velocity = getScalesSpeedVector2(new Vector2(velocity.x, -velocity.y));
            _rb.velocity = velocity;
        }

        //Debug.Log("OnCollisionEnter2D" + other.gameObject);
    }
}