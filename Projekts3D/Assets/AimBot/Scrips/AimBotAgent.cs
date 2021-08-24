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
        private float _lastAngle;
        private Quaternion _optimalRotation;

        public override void Initialize() {
            _xRotation = 0;
            _yRotation = 0;
            _pistol = GetComponent<Pistol>();
            HardReset();
        }

        public override void CollectObservations(VectorSensor sensor) {
            Vector3 rotationDiff = _optimalRotation.eulerAngles - transform.rotation.eulerAngles;
            sensor.AddObservation(rotationDiff.x);
            sensor.AddObservation(rotationDiff.y);
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask) {
            float angle = Quaternion.Angle(transform.rotation, _optimalRotation);
            if (Mathf.Abs(angle) > 1e-1f) {
                actionMask.SetActionEnabled(0, 1, false);
            }
            else {
                //Debug.Log("WriteDiscreteActionMask: " + angle);
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut) {
            var continuousActions = actionsOut.ContinuousActions;
            continuousActions[0] = Input.GetAxis("Mouse X");
            continuousActions[1] = Input.GetAxis("Mouse Y");
            //continuousActions[2] = Input.GetButton("Fire1") ? 1 : 0;
            var discreteActions = actionsOut.DiscreteActions;
            discreteActions[0] = Input.GetButton("Fire1") ? 1 : 0;
        }

        public override void OnActionReceived(ActionBuffers actions) {
            var continuousActions = actions.ContinuousActions;
            var discreteActions = actions.DiscreteActions;
            //Debug.Log("continuousActions: " + continuousActions[0] + " | " + continuousActions[1]);
            //Debug.Log("discreteActions: " + discreteActions[0]);
            float x = continuousActions[0] * mouseSensitivity;
            float y = continuousActions[1] * mouseSensitivity;
            //Debug.Log("X: " + x + " | Y:" + y);
            _xRotation -= y;
            _yRotation += x;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            _yRotation = Mathf.Clamp(_yRotation, -180f, 180f);
            //Debug.Log("X: " + _xRotation + " | Y:" + _yRotation);
            transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
            CalculateDecisionReward();

            if (discreteActions[0] == 1) {
                if (_pistol.Fire()) {
                    AddReward(50f);
                    EndEpisode();
                }
                else AddReward(-5f);
            }

            /*if (continuousActions[2] > 0.5) {
                if (_pistol.Fire()) {
                    AddReward(50f);
                    EndEpisode();
                }
                else AddReward(-5f);
            }*/
        }

        private void CalculateDecisionReward() {
            float newAngle = Quaternion.Angle(transform.rotation, _optimalRotation);
            if (newAngle < _lastAngle) {
                //Debug.Log("Getting Closer");
                AddReward(0.1f);
            }
            else {
                //Debug.Log("Getting Away");
                AddReward(-0.1f);
            }

            AddReward(-0.01f); // per step, ensure to be fast

            _lastAngle = newAngle;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F)) {
                transform.rotation = _optimalRotation;
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                target.ChangePosition();
            }
        }

        public override void OnEpisodeBegin() {
            Debug.Log(this.GetCumulativeReward());
            target.ChangePosition();
            HardReset();
        }

        private void HardReset() {
            transform.localRotation = Quaternion.identity;
            _optimalRotation = Utils.getRotationToLookAt(gameObject, target.gameObject);
            _lastAngle = Quaternion.Angle(transform.rotation, _optimalRotation);
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