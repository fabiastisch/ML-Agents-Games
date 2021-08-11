using UnityEngine;

namespace Tetris.Scripts {
    public class TetrisControl : MonoBehaviour {
        [SerializeField] private Spawner spawner;
        private Block _currentBlock;

        // Start is called before the first frame update
        void Start() {
            spawner.OnSpawnedBlock += block => _currentBlock = block;
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.X)) {
                spawner.TEMP_SpawnOther();
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                spawner.isEnabled = !spawner.isEnabled;
            }

            //WithoutAgent();
        }

        private void WithoutAgent() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                _currentBlock.TryToRotate();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                _currentBlock.MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                _currentBlock.MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                _currentBlock.MoveDown();
            }
        }
    }
}