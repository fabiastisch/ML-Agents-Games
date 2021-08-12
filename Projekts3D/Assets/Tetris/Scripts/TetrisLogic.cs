using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Scripts {
    public class TetrisLogic : MonoBehaviour {
        private readonly int _maxX = TetrisStatics.maxX + 1;
        private readonly int _maxY = TetrisStatics.maxY + 1;
        private readonly List<Block> _tetrisBlocks = new List<Block>();
        private Transform[,] _blocks;
        private Block _currentBlock;

        public event Action OnCompleteRow;

        [SerializeField] private Spawner spawner;

        private void Start() {
            _blocks = new Transform[_maxX, _maxY];
            spawner.OnSpawnedBlock += SpawnerOnOnSpawnedBlock;
            spawner.OnGameOver += () => {
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
                spawner.SpawnNext();
            }

            AddBlockToBlocks(block);
            CheckRows();
        }

        private void CheckRows() {
            for (int i = _maxY - 1; i >= 0; i--) {
                if (IsCompleteRow(i)) {
                    DeleteRow(i);
                    RowsDown(i);
                    OnCompleteRow?.Invoke();
                }
            }
        }

        private void DeleteRow(int rowIndex) {
            for (int i = 0; i < _maxX; i++) {
                Destroy(_blocks[i, rowIndex].gameObject);
                _blocks[i, rowIndex] = null;
            }
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
            foreach (Transform child in block.transform) {
                var position = child.transform.position;
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                //Debug.Log("Add BLock to Blocks Y: " + x + " Y: " + y);
                _blocks[x, y] = child;
            }
        }

        public void ResetGame() {
            foreach (Block tetrisBlock in _tetrisBlocks) {
                if (tetrisBlock) {
                    Destroy(tetrisBlock.gameObject);
                }
            }
        }

        public List<float> GetBlocks() {
            List<float> blockList = new List<float>();
            for (int y = 0; y < _maxY; y++) {
                for (int x = 0; x < _maxX; x++) {
                    if (_blocks[x, y]) {
                        blockList.Add(1);
                    }
                    else {
                        blockList.Add(0);
                    }
                }
            }

            return blockList;
        }
    }
}