using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PaddleAgent : Agent {
    private Paddle _paddle;

    private bool isLeftPaddle;

    // Start is called before the first frame update
    void Start() {
        _paddle = GetComponent<Paddle>();
        _paddle.ball.OnLeftGoal += BallOnOnLeftGoal;
        _paddle.ball.OnRightGoal += BallOnOnRightGoal;
        isLeftPaddle = _paddle.isLeftPaddle;
    }

    private void BallOnOnRightGoal() {
        if (isLeftPaddle) {
            AddReward(1);
        }
        else {
            AddReward(-1);
            EndEpisode();
        }
    }

    private void BallOnOnLeftGoal() {
        if (isLeftPaddle) {
            AddReward(-1);
            EndEpisode();
        }
        else {
            AddReward(1);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Vertical");
    }

    public override void CollectObservations(VectorSensor sensor) {
        // 5 observations
        // position self
        sensor.AddObservation(transform.position.y); // Vector 1
        // ball distance
        sensor.AddObservation(Utils.getDistanceVector2(_paddle.ball.gameObject, gameObject)); // Vector 2
        // ball direction
        sensor.AddObservation(_paddle.ball.GetComponent<Rigidbody2D>().velocity); // Vector 2
    }

    public override void OnActionReceived(ActionBuffers actions) {
        //Debug.Log("OnActionReceived: " + actions.ContinuousActions[0]);
        _paddle.Move(actions.ContinuousActions[0]);
    }
}