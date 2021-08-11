using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Scripts {
    public class Spawner : MonoBehaviour {
        [SerializeField] private List<GameObject> blocks;

        private GameObject spawnedBlock;
        [HideInInspector] public bool isEnabled = true;

        public event Action<Vector3> OnRowGotDestroyed;

        // Start is called before the first frame update
        void Start() {
            OnRowGotDestroyed += OnOnRowGotDestroyed;
        }

        private void OnOnRowGotDestroyed(Vector3 row) {
            row.y++;
            foo(row, Vector3.up);
            //foo(row, Vector3.right);
        }

        private void foo(Vector3 pos, Vector3 dir) {
            //Debug.Log(pos +" , with dir: " + dir);
            Collider[] colliders = Utils.CheckPosBox(pos);
            if (!(colliders.Length < 1)) {
                if (colliders[0].CompareTag("Wall")) {
                    return;
                }

                Debug.Log(colliders[0]);
                Debug.Log(colliders[0].transform.parent.GetComponent<Block>());
                colliders[0].transform.parent.GetComponent<Block>()?.EnableGravity();
            }

            if (dir == Vector3.up) {
                foo(pos + Vector3.left, Vector3.left);
                foo(pos + Vector3.right, Vector3.right);
                //foo(pos + dir, dir);
            }

            foo(pos + dir, dir);
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
            var completeRows = new List<int>();
            foreach (Transform child in spawnedBlock.transform) {
                if (IsRowComplete(child.position) && !completeRows.Contains((int) child.position.y)) {
                    //completeRows.Add((int) child.position.y);
                    DestroyRow(child.position);
                }
            }

            // TODO: Implement full filled Rows logic... and Test
            Debug.Log("completeRows: " + completeRows.Count);
        }

        private void DestroyRow(Vector3 row) {
            DestroyRowSide(row, Vector3.left);
            DestroyRowSide(row, Vector3.right);
            OnRowGotDestroyed?.Invoke(row);
        }

        private void DestroyRowSide(Vector3 pos, Vector3 side) {
            Collider[] colliders;
            colliders = Utils.CheckPosBox(pos);
            if (colliders.Length > 0 && !colliders[0].CompareTag("Wall")) {
                Destroy(colliders[0].gameObject);
                DestroyRowSide(pos + side * 1, side);
            }
        }

        private bool IsRowComplete(Vector3 pos) {
            return CheckRowSide(pos, Vector3.left) &&
                   CheckRowSide(pos, Vector3.right);
        }

        private bool CheckRowSide(Vector3 pos, Vector3 side) {
            Collider[] colliders;
            // temp to avoid inf loop
            int max = 50;
            int counter = 0;

            int offsetPos = 0;
            do {
                colliders = Utils.CheckPosBox(pos + side * offsetPos);
                offsetPos++;
                var x = (pos + side * offsetPos);
                //Debug.Log("Check Pos: " + x + " colliders: " + col.Length + " : ");
                if (colliders.Length < 1) {
                    return false;
                }

                counter++;
            } while (!colliders[0].CompareTag("Wall") && counter <= max);

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