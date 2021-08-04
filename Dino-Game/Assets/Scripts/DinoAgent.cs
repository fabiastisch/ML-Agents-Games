using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DinoAgent : Agent {
    [SerializeField] private GameManager _gameManager;
    private Dino _dino;

    // Start is called before the first frame update
    void Start() {
        _dino = GetComponent<Dino>();
    }
    
    public override void OnEpisodeBegin() {
        _gameManager.ResetGame();
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        Debug.Log("Heuristic");
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if ( Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) {
            discreteActions[0] = 1;
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Debug.Log("CollectObservations");
        sensor.AddObservation(_dino._isGrounded);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        Debug.Log(actions.DiscreteActions[0]);
        switch (actions.DiscreteActions[0]) {
            case 1:
                //Debug.Log("Go - Jump");
                _dino.Jump();
                //direction = Vector2.up;
                break;
            default:
                break;
        }

        //AddReward(-0.1f);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("DIno On Trigger");
        AddReward(-10000);
        AddReward(_gameManager.counter.getCounts());
        EndEpisode();
    }

    // Update is called once per frame
    void Update() {
        //if (_dino.canJump) {
            
        //}
    }
}