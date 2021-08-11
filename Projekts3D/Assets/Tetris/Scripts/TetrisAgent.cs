﻿using Unity.MLAgents;
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
            spawner.SpawnNext();
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

        public override void CollectObservations(VectorSensor sensor) {
            // Block Position, Typ and Rotation
            sensor.AddObservation(_currentBlock.transform.position.x);
            sensor.AddObservation(_currentBlock.transform.position.y);
            sensor.AddObservation((int) _currentBlock.type);
            sensor.AddObservation(_currentBlock.rotation);
            // Filled Grid
            sensor.AddObservation(tetrisLogic.GetBlocks());
        }

        public override void OnActionReceived(ActionBuffers actions) {
            //Debug.Log("OnActionReceived: " + actions.DiscreteActions[0]);
            //Debug.Log("OnActionReceived: " + actions.ContinuousActions[0]);
            int action = actions.DiscreteActions[0];
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
    }
}