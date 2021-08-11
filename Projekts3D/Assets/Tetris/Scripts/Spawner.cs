using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Scripts {
    public class Spawner : MonoBehaviour {
        [SerializeField] private List<GameObject> spawnableBlocks;

        [HideInInspector] public bool isEnabled = true;

        public event Action<Block> OnSpawnedBlock;
        public event Action OnGameOver;

        private GameObject lastSpawnedBlock;

        private void Start() {
        }

        public void TEMP_SpawnOther() {
            if (lastSpawnedBlock) {
                DestroyImmediate(lastSpawnedBlock);
            }

            SpawnNext();
        }

        public void SpawnNext() {
            if (!isEnabled) {
                return;
            }

            Debug.Log("SpawnNext");
            var col = Physics.OverlapBox(transform.position + Vector3.up * 0.3f,
                (Vector3.one * 0.1f) + Vector3.right * (TetrisStatics.maxX * 0.5f - 1));
            if (col.Length > 0) {
                Debug.Log("Game Over!");
                OnGameOver?.Invoke();
                return;
            }

            GameObject spawned = Instantiate(spawnableBlocks[Utils.GetRandomInt(spawnableBlocks.Count - 1)], transform.position,
                Quaternion.identity);
            lastSpawnedBlock = spawned;
            OnSpawnedBlock?.Invoke(spawned.GetComponent<Block>());
        }

        void OnDrawGizmos() {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Vector3 halfSize = (Vector3.one * 0.1f) + Vector3.right * (TetrisStatics.maxX * 0.5f - 1);
            Gizmos.DrawCube(transform.position + Vector3.up * 0.3f, halfSize * 2);
        }
    }
}