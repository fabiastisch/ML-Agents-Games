using System;
using System.Collections.Generic;
using UnityEngine;
using static Tetris.Scripts.TetrisStatics;

namespace Tetris.Scripts {
    public class TetrisLogic : MonoBehaviour {
        private readonly List<Block> _tetrisBlocks = new List<Block>();
        private Transform[,] _blocks;
        private Block _currentBlock;
        [SerializeField] private TetrisAgent _agent;
        private int completedLines = 0;
        public int currentMaxHeight = 0;

        /**
         * [y,x]
         * 1: Height (y)
         * 2: With (x)
         */
        public bool[,] boolBlocks = new bool[maxY, maxX];

        public event Action OnCompleteRow;

        [SerializeField] private Spawner spawner;

        private void Start() {
            _blocks = new Transform[maxX, maxY];
            spawner.OnSpawnedBlock += SpawnerOnOnSpawnedBlock;
            spawner.OnGameOver += () => {
                //Debug.Log("GameOver");
                _currentBlock = null;
                foreach (Block tetrisBlock in _tetrisBlocks) {
                    if (tetrisBlock) tetrisBlock.OnEnterGround -= BlockOnOnEnterGround;
                }
            };
        }

        private void SpawnerOnOnSpawnedBlock(Block block) {
            _currentBlock = block;
            block.OnEnterGround += BlockOnOnEnterGround;
            _tetrisBlocks.Add(block);
        }

        private void BlockOnOnEnterGround(Block block) {
            if (block.Equals(_currentBlock)) {
                //Debug.Log("BlockOnOnEnterGround");

                float baseReward = 0.1f;
                // 0 - 1, depending on the height
                float bonusReward = (maxY - currentMaxHeight) / maxY;
                _agent.AddReward(baseReward);


                int gapCounter = 0;
                IterateOverBlock(block, (x, y, objTransform) => {
                    //AddBlockToBlocks();
                    _blocks[x, y] = objTransform;
                    boolBlocks[y, x] = true;
                    // Update Max Height
                    currentMaxHeight = Math.Max(y, currentMaxHeight);
                    //CheckForGaps
                    if (boolBlocks[y - 1 >= 0 ? y - 1 : 0, x]) {
                        // One block below 
                        gapCounter++;
                    }
                });
                _agent.SetGapsReward(gapCounter);

                CheckRows();

                spawner.SpawnNext();
            }
        }


        private void CheckRows() {
            for (int i = maxY - 1; i >= 0; i--) {
                if (IsCompleteRow(i)) {
                    DeleteRow(i);
                    RowsDown(i);
                    _agent.AddReward(50f * ++completedLines);
                    OnCompleteRow?.Invoke();
                }
            }
        }

        private void DeleteRow(int height) {
            for (int i = 0; i < maxX; i++) {
                Destroy(_blocks[i, height].gameObject);
                _blocks[i, height] = null;
                boolBlocks[height, i] = false;
            }

            currentMaxHeight--;
        }

        private void RowsDown(int minRowIndex) {
            for (int y = minRowIndex; y < maxY; y++) {
                for (int x = 0; x < maxX; x++) {
                    if (_blocks[x, y] == null) {
                        continue;
                    }

                    _blocks[x, y - 1] = _blocks[x, y];
                    _blocks[x, y] = null;
                    _blocks[x, y - 1].transform.position += Vector3.down;

                    boolBlocks[y - 1, x] = boolBlocks[y, x];
                    boolBlocks[y, x] = false;
                }
            }
        }

        private bool IsCompleteRow(int rowIndex) {
            for (int i = 0; i < maxX; i++) {
                //Debug.Log(i + " | " + rowIndex);
                if (_blocks[i, rowIndex] == null) {
                    return false;
                }
            }

            return true;
        }

        private void AddBlockToBlocks(Block block) {
            //Debug.Log(block);
            Transform parent = transform.parent;

            foreach (Transform child in block.transform) {
                var position = parent.InverseTransformPoint(child.transform.position);
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                //Debug.Log("Add BLock to Blocks Y: " + x + " Y: " + y);
                _blocks[x, y] = child;
                boolBlocks[y, x] = true;
                currentMaxHeight = Math.Max(y, currentMaxHeight);
            }
        }

        public void ResetGame() {
            foreach (Block tetrisBlock in _tetrisBlocks) {
                if (tetrisBlock) {
                    Destroy(tetrisBlock.gameObject);
                }
            }

            _tetrisBlocks.Clear();
            _blocks = new Transform[maxX, maxY];
            boolBlocks = new bool[maxY, maxX];
            completedLines = 0;
        }

        public void IterateOverBlock(Block block, Action<int, int, Transform> callback) {
            Transform parent = transform.parent;

            foreach (Transform child in block.transform) {
                var position = parent.InverseTransformPoint(child.transform.position);
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                callback(x, y, child);
            }
        }
    }
}