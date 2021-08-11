using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Scripts {
    public class Spawner : MonoBehaviour {
        [SerializeField] private List<GameObject> spawnableBlocks;

        [HideInInspector] public bool isEnabled = true;
        
        public event Action<Block> OnSpawnedBlock;

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

            var col = new Collider[1];
            if (Physics.OverlapBoxNonAlloc(transform.position, (Vector3.one * 0.4f) + Vector3.right * (TetrisStatics.maxX * 0.5f -1), col) > 0) {
                Debug.Log("Game Over!");
                return;
            }

            GameObject spawned = Instantiate(spawnableBlocks[Utils.GetRandomInt(spawnableBlocks.Count - 1)], transform.position,
                Quaternion.identity);
            lastSpawnedBlock = spawned;
            OnSpawnedBlock?.Invoke(spawned.GetComponent<Block>());
        }

    }
}