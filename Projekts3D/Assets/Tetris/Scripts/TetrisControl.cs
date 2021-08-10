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

            /*
             * <- Move Left
             * -> Move Right
             * up Rotate
             * down Move down
             */
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                spawner.GetCurrentBlock()?.GetComponent<Block>().Rotate();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                spawner.GetCurrentBlock()?.GetComponent<Block>().MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                spawner.GetCurrentBlock()?.GetComponent<Block>().MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                spawner.GetCurrentBlock()?.GetComponent<Block>().MoveDown();
            }
        }
    }
}