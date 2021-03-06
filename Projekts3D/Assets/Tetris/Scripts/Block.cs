using System;
using UnityEngine;

namespace Tetris.Scripts {
    public class Block : MonoBehaviour {
        public event Action<Block> OnEnterGround;
        private Rigidbody _rb;

        public TetrisBlockType type;
        [SerializeField] private Vector3 rotatePosition;
        [SerializeField] private bool hasOnly2Rotations = false;
        private bool _isRotated = false;
        private bool _isGrounded = false;
        public int rotation = 0;

        // Start is called before the first frame update
        void Start() {
            _rb = GetComponent<Rigidbody>();
            _rb.velocity = Vector3.down * 10;
        }

        // Update is called once per frame
        void Update() {
        }

        public void TryToRotate() {
            if (_isGrounded) {
                return;
            }

            if (hasOnly2Rotations) {
                RotateInternal(_isRotated ? 90 : -90);
                _isRotated = !_isRotated;
                return;
            }

            RotateInternal(90);
        }

        private void RotateInternal(float angle) {
            transform.RotateAround(transform.TransformPoint(rotatePosition), Vector3.forward, -angle);
            if (!IsValidMove(Vector3.zero)) {
                transform.RotateAround(transform.TransformPoint(rotatePosition), Vector3.forward, angle);
            }
            else {
                rotation = (rotation + 1) % 4;
            }
        }

        public void TryToMove(Vector3 dir) {
            if (_isGrounded) return;
            if (IsValidMove(dir)) {
                transform.position += dir;
            }
        }

        private void setChildLayerMask(string maskName) {
            foreach (Transform child in transform) {
                child.gameObject.layer = LayerMask.NameToLayer(maskName);
            }
        }

        private bool IsValidMove(Vector3 dir) {
            setChildLayerMask("Temp");

            int customLayerMask = LayerMask.GetMask("Default");

            // Physics.OverlapBox()
            foreach (Transform child in transform) {
                // We only need one collision. 
                Collider[] colliders = new Collider[1];
                int collisions;
                if (dir == Vector3.down) {
                    var pos = child.position + dir;
                    pos.y = Mathf.RoundToInt(pos.y);
                    //colliders = Physics.OverlapBox(child.position + dir, Vector3.one * 0.4f, Quaternion.identity, customLayerMask);
                    collisions = Utils.CheckPositionBox(pos, colliders, customLayerMask);
                    //Debug.Log(collisions);
                    if (collisions > 0) {
                        setChildLayerMask("Default");
                        return false;
                    }
                }
                else {
                    collisions = Utils.CheckPositionBox(child.position + dir, colliders, customLayerMask);
                    //Physics.OverlapBoxNonAlloc(child.position + dir, Vector3.one * 0.4f, colliders, Quaternion.identity, customLayerMask);
                    //Debug.Log(collisions);

                    if (collisions > 0) {
                        setChildLayerMask("Default");
                        return false;
                    }
                }
            }

            setChildLayerMask("Default");
            return true;
        }

        public void MoveLeft() {
            TryToMove(Vector3.left);
        }

        public void MoveRight() {
            TryToMove(Vector3.right);
        }

        public void MoveDown() {
            TryToMove(Vector3.down);
        }

        private void OnCollisionEnter(Collision other) {
            if (!_isGrounded) {
                //Debug.Log(other.contacts[0].normal + " Point: " + other.contacts[0].point);
                OnEnterGround?.Invoke(this);
                var position = transform.position;
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                position = new Vector3(x, y, position.z);
                transform.position = position;
            }

            _isGrounded = true;
            _rb.isKinematic = true;
        }
    }

    public enum TetrisBlockType {
        I,
        L,
        O,
        S,
        T,
        Z
    }
}