using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Tetris.Scripts {
    public class TetrisAgent : Agent {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TetrisLogic tetrisLogic;
        private Block _currentBlock;
        public bool[,] state = new bool[TetrisStatics.maxY, TetrisStatics.maxX];

        public override void OnEpisodeBegin() {
            tetrisLogic.ResetGame();
            spawner.StartTryToSpawnNext();
        }

        private void Start() {
            spawner.OnSpawnedBlock += SpawnerOnOnSpawnedBlock;
            spawner.OnGameOver += SpawnerOnOnGameOver;
            tetrisLogic.OnCompleteRow += TetrisLogicOnOnCompleteRow;
        }

        private void SpawnerOnOnSpawnedBlock(Block block) {
            _currentBlock = block;
            AddReward(0.1f);
        }

        private void TetrisLogicOnOnCompleteRow() {
            AddReward(2);
        }

        private void SpawnerOnOnGameOver() {
            EndEpisode();
        }

        public override void CollectObservations(VectorSensor sensor) {
            sensor.AddObservation(Utils.ArrayToList(AddCurrentBlock(state)));
        }

        public override void Heuristic(in ActionBuffers actionsOut) {
            Debug.Log("Heuristic");
            ActionSegment<int> actions = actionsOut.DiscreteActions;
            if (Input.GetKey(KeyCode.UpArrow)) {
                actions[0] = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                actions[0] = 2;
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                actions[0] = 3;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                actions[0] = 4;
            }
        }

        public override void OnActionReceived(ActionBuffers actions) {
            Debug.Log("OnActionReceived: " + actions.DiscreteActions[0]);
            //Debug.Log("OnActionReceived: " + actions.ContinuousActions[0]);
            int action = actions.DiscreteActions[0];
            if (!_currentBlock) {
                return;
            }

            switch (action) {
                case 1:
                    _currentBlock.TryToRotate();
                    break;
                case 2:
                    _currentBlock.MoveLeft();
                    break;
                case 3:
                    _currentBlock.MoveRight();
                    break;
                case 4:
                    _currentBlock.MoveDown();
                    break;
            }
        }

        private void OnDrawGizmos() {
            Vector3 startPos = transform.position + Vector3.down * 9 + Vector3.right * 3;
            DebugUtils.Draw2DListGizmos(startPos, Utils.ArrayToList(AddCurrentBlock(state)), TetrisStatics.maxY);
        }

        private bool[,] AddCurrentBlock(bool[,] array) {
            if (!_currentBlock) {
                return array;
            }
            bool[,] copy = array.Clone() as bool[,];
            
            foreach (Transform child in _currentBlock.transform) {
                var position = child.transform.position;
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                //Debug.Log("Add BLock to Blocks Y: " + x + " Y: " + y);

                System.Diagnostics.Debug.Assert(copy != null, nameof(copy) + " != null");
                copy[y, x] = true;
            }

            return copy;
        }
    }
}