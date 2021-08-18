using System;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static Tetris.Scripts.TetrisStatics;

namespace Tetris.Scripts {
    public class TetrisLogic : MonoBehaviour {
        private readonly int _maxX = maxX;
        private readonly int _maxY = maxY;
        private readonly List<Block> _tetrisBlocks = new List<Block>();
        private Transform[,] _blocks;
        private Block _currentBlock;
        [SerializeField] private TetrisAgent _agent;
        private int completedLines = 0;
        public int maxHeight = 0;

        /**
         * [y,x]
         * 1: Height (y)
         * 2: With (x)
         */
        private bool[,] boolBlocks {
            get => _agent.state;
            set => _agent.state = value;
        }

        public event Action OnCompleteRow;

        [SerializeField] private Spawner spawner;

        private void Start() {
            _blocks = new Transform[_maxX, _maxY];
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
                float bonusReward = (maxY - maxHeight) / maxY;
                _agent.AddExternalReward(baseReward);
                
                
                int gapCounter = 0;
                IterateOverBlock(block, (x, y, objTransform) => {
                    //AddBlockToBlocks();
                    _blocks[x, y] = objTransform;
                    boolBlocks[y, x] = true;
                    // Update Max Height
                    maxHeight = Math.Max(y, maxHeight);
                    //CheckForGaps
                    if (boolBlocks[y - 1 >= 0 ? y - 1 : 0, x]) {
                        // One block below 
                        gapCounter++;
                    }
                  
                });
                _agent.SetGapsReward(gapCounter);
                //AddBlockToBlocks(block);
                //CheckForGaps(block);

                CheckRows();

                spawner.SpawnNext();
            }
        }

        private void CheckForGaps(Block block) {
            Transform parent = transform.parent;
            int gaps = 0;

            foreach (Transform child in block.transform) {
                var position = parent.InverseTransformPoint(child.transform.position);
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                //Debug.Log("Add BLock to Blocks Y: " + x + " Y: " + y);

                if (boolBlocks[y - 1 >= 0 ? y - 1 : 0, x]) {
                    // One block below 
                    gaps++;
                }
            }

            _agent.SetGapsReward(gaps);
        }

        private void CheckRows() {
            for (int i = _maxY - 1; i >= 0; i--) {
                if (IsCompleteRow(i)) {
                    DeleteRow(i);
                    RowsDown(i);
                    _agent.AddExternalReward(50f * ++completedLines);
                    OnCompleteRow?.Invoke();
                }
            }
        }

        private void DeleteRow(int height) {
            for (int i = 0; i < _maxX; i++) {
                Destroy(_blocks[i, height].gameObject);
                _blocks[i, height] = null;
                boolBlocks[height, i] = false;
            }

            maxHeight--;
        }

        private void RowsDown(int minRowIndex) {
            for (int y = minRowIndex; y < _maxY; y++) {
                for (int x = 0; x < _maxX; x++) {
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
            for (int i = 0; i < _maxX; i++) {
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
                maxHeight = Math.Max(y, maxHeight);
            }
        }

        public void ResetGame() {
            foreach (Block tetrisBlock in _tetrisBlocks) {
                if (tetrisBlock) {
                    Destroy(tetrisBlock.gameObject);
                }
            }

            _tetrisBlocks.Clear();
            _blocks = new Transform[_maxX, _maxY];
            boolBlocks = new bool[_maxY, _maxX];
            completedLines = 0;
        }

        private void IterateOverBlock(Block block, Action<int, int, Transform> callback) {
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