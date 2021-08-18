using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Tetris.Scripts {
    public class TetrisAgent : Agent {
        [SerializeField] private Spawner spawner;
        [SerializeField] private TetrisLogic tetrisLogic;
        private Block _currentBlock;

        public override void OnEpisodeBegin() {
            tetrisLogic.ResetGame();
            spawner.StartTryToSpawnNext();
        }

        private void Start() {
            spawner.OnSpawnedBlock += SpawnerOnOnSpawnedBlock;
            spawner.OnGameOver += EndEpisode;
            //tetrisLogic.OnCompleteRow += TetrisLogicOnOnCompleteRow;
        }

        private void SpawnerOnOnSpawnedBlock(Block block) {
            _currentBlock = block;
        }

        public override void CollectObservations(VectorSensor sensor) {
            sensor.AddObservation(tetrisLogic.currentMaxHeight);
            sensor.AddObservation(Utils.ArrayToList(AddCurrentBlock(tetrisLogic.boolBlocks)));
        }

        public override void Heuristic(in ActionBuffers actionsOut) {
            //Debug.Log("Heuristic");
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
            //Debug.Log("OnActionReceived: " + actions.DiscreteActions[0]);
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
                case 4: // currently disabled
                    _currentBlock.MoveDown();
                    break;
            }
        }

        private void OnDrawGizmosSelected() {
            Vector3 startPos = transform.position + Vector3.down * 9 + Vector3.right * 3;
            DebugUtils.Draw2DListGizmos(startPos, Utils.ArrayToList(AddCurrentBlock(tetrisLogic.boolBlocks)), TetrisStatics.maxY);
        }

        private bool[,] AddCurrentBlock(bool[,] array) {
            if (!_currentBlock) {
                return array;
            }

            bool[,] copy = array.Clone() as bool[,];

            tetrisLogic.IterateOverBlock(_currentBlock, (x, y, arg3) => {
                if (copy != null) copy[y, x] = true;
            });
            
            return copy;
        }

        public void SetGapsReward(int gaps) {
            if (gaps == 0) {
                AddReward(1);
            }
            else {
                AddReward(-(gaps / 20));
            }
        }
    }
}