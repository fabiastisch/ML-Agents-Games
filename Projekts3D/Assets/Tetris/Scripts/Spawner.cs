using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Scripts {
    public class Spawner : MonoBehaviour {
        [SerializeField] private List<GameObject> blocks;

        private GameObject spawnedBlock;

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
            spawnedBlock = spawned;
        }

        public GameObject GetCurrentBlock() {
            return spawnedBlock;
        }
    }
}