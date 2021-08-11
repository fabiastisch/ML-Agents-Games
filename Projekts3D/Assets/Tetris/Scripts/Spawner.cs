using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Scripts {
    public class Spawner : MonoBehaviour {
        [SerializeField] private List<GameObject> blocks;

        private GameObject spawnedBlock;
        [HideInInspector] public bool isEnabled = true;

        // Start is called before the first frame update
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }

        public void SpawnNext() {
            if (spawnedBlock) {
                Destroy(spawnedBlock);
            }

            GameObject spawned = Instantiate(blocks[Utils.GetRandomInt(blocks.Count - 1)], transform.position, Quaternion.identity);
            spawned.GetComponent<Block>().OnEnterGround += CurrentBlockOnEnterGround;
            spawnedBlock = spawned;
        }

        private void CurrentBlockOnEnterGround() {
            CheckRows();
            spawnedBlock.GetComponent<Block>().OnEnterGround -= CurrentBlockOnEnterGround;
            spawnedBlock = null;
            if (isEnabled) {
                SpawnNext();
            }
        }

        private void CheckRows() {
            Collider[] colliders = new Collider[1];
            var completeRows = new List<int>();
            foreach (Transform child in spawnedBlock.transform) {
                if (IsRowComplete(child.position, colliders) && !completeRows.Contains((int) child.position.y)) {
                    completeRows.Add((int) child.position.y);
                }
            }

            // TODO: Implement full filled Rows logic... and Test
            Debug.Log("completeRows: " + completeRows.Count);
        }

        private bool IsRowComplete(Vector3 pos, Collider[] col) {
            return CheckRowSide(pos, col, Vector3.left) &&
                   CheckRowSide(pos, col, Vector3.right);
        }

        private bool CheckRowSide(Vector3 pos, Collider[] col, Vector3 side) {
            // temp to avoid inf loop
            int max = 50;
            int counter = 0;

            int offsetPos = 0;
            do {
                col = Utils.CheckPosBox(pos + side * offsetPos);
                offsetPos++;
                var x = (pos + side * offsetPos);
                //Debug.Log("Check Pos: " + x + " colliders: " + col.Length + " : ");
                if (col.Length < 1) {
                    return false;
                }

                counter++;
            } while (!col[0].CompareTag("Wall") && counter <= max);

            if (counter >= max) {
                Debug.LogError("Pls fix Do While loop, count: " + counter);
            }

            return true;
        }

        public GameObject GetCurrentBlock() {
            return spawnedBlock;
        }
    }
}