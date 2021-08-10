using UnityEngine;

namespace Tetris.Scripts {
    public class Block : MonoBehaviour {
        private Rigidbody _rb;
        // Start is called before the first frame update
        void Start() {
            _rb = GetComponent<Rigidbody>();
            _rb.velocity = Vector3.down;
        }

        // Update is called once per frame
        void Update() {
        }
    }
}