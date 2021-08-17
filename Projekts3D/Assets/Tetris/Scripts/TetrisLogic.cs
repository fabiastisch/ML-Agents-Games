using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Scripts {
    public class TetrisLogic : MonoBehaviour {
        private readonly int _maxX = TetrisStatics.maxX;
        private readonly int _maxY = TetrisStatics.maxY;
        private readonly List<Block> _tetrisBlocks = new List<Block>();
        private Transform[,] _blocks;
        private Block _currentBlock;
        [SerializeField] private TetrisAgent _agent;
        private bool[,] boolBlocks {
            get => _agent.state;
            set => _agent.state = value;
        }

        private List<float> floatBlocks {
            get => _agent.floatState;
            set => _agent.floatState = value;
        }

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
                boolBlocks[rowIndex, i] = false;
                floatBlocks[rowIndex * TetrisStatics.maxY + i] = 0;
                
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
                    boolBlocks[y - 1, x] = boolBlocks[x, y];
                    floatBlocks[(y-1) * TetrisStatics.maxY + x] = floatBlocks[x * TetrisStatics.maxY + y] = 0;;
                    boolBlocks[y,x] = false;
                    floatBlocks[y * TetrisStatics.maxY + x] = 0;
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
                boolBlocks[y,x] = child != null;
                floatBlocks[y * TetrisStatics.maxY + x] = 0;
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
            floatBlocks = new List<float>();
        }

        public bool[,] GetBlocks() {
            Debug.Log("GetBlocks");
            var bools = boolBlocks;
            foreach (Transform child in _currentBlock.transform) {
                Vector3 position = child.position;
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                bools[y,x] = child != null;
            }

            return bools;
        }
    }
}