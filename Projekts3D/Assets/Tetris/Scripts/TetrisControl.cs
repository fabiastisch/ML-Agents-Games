using UnityEngine;

namespace Tetris.Scripts {
    public class TetrisControl : MonoBehaviour {
        [SerializeField] private Spawner spawner;
        // Start is called before the first frame update
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.X)) {
                spawner.SpawnNext();
            }
        }
    }
}