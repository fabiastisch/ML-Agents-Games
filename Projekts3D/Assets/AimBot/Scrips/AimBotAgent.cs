using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace AimBot.Scrips {
    public class AimBotAgent : Agent {
        [SerializeField] private Target target;
        private float _xRotation;
        private float _yRotation;
        private float mouseSensitivity = 10f;
        private Pistol _pistol;
        private Vector3 _lastRotation;

        public int AvailableSteps => MaxStep - StepCount;

        public override void Initialize() {
            _xRotation = 0;
            _yRotation = 0;
            _lastRotation = transform.localEulerAngles;
            _pistol = GetComponent<Pistol>();
        }

        public override void CollectObservations(VectorSensor sensor) {
            Quaternion rotation = Utils.getRotationToLookAt(gameObject, target.gameObject);
            Vector3 _rotationDiff = rotation.eulerAngles - transform.rotation.eulerAngles;
            sensor.AddObservation(_rotationDiff.x);
            sensor.AddObservation(_rotationDiff.y);
        }

        public override void Heuristic(in ActionBuffers actionsOut) {
            var continuousActions = actionsOut.ContinuousActions;
            continuousActions[0] = Input.GetAxis("Mouse X");
            continuousActions[1] = Input.GetAxis("Mouse Y");
            continuousActions[2] = Input.GetButton("Fire1") ? 1 : 0;
        }

        public override void OnActionReceived(ActionBuffers actions) {
            var continuousActions = actions.ContinuousActions;
            //Debug.Log(continuousActions[0] + " | " + continuousActions[1] + " | " + continuousActions[2]);
            float x = continuousActions[0] * mouseSensitivity;
            float y = continuousActions[1] * mouseSensitivity;
            _xRotation -= y;
            _yRotation += x;
            transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
            CalculateDecisionReward();

            if (continuousActions[2] > 0.5) {
                if (_pistol.Fire()) {
                    AddReward(10f);
                    EndEpisode();
                }
                else AddReward(-2f);
            }
        }

        private void CalculateDecisionReward() {
            Quaternion rotation = Utils.getRotationToLookAt(gameObject, target.gameObject);
            Vector3 newRotation = transform.localEulerAngles;
            Vector3 lastDiff = rotation.eulerAngles - _lastRotation;
            Vector3 newDiff = rotation.eulerAngles - newRotation;
            Debug.Log(newDiff);
            if (newDiff.magnitude < lastDiff.magnitude) {
                //Debug.Log("Getting Closer");
                AddReward(0.1f);
            }
            else {
                //Debug.Log("Getting Away");
                AddReward(-0.1f);
            }

            _lastRotation = transform.localEulerAngles;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F)) {
                transform.rotation = Utils.getRotationToLookAt(gameObject, target.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                target.ChangePosition();
            }
        }

        public override void OnEpisodeBegin() {
            Debug.Log(this.GetCumulativeReward());
            target.ChangePosition();
            transform.localRotation = Quaternion.identity;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 20);
        }

        public new void AddReward(float value) {
            Debug.Log(value);
            base.AddReward(value);
        }
    }
}