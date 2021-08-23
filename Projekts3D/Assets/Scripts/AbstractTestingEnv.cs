using Unity.MLAgents;
using UnityEngine;
using UnityEngine.UI;

public class AbstractTestingEnv : MonoBehaviour {
    [SerializeField] private Text text;
    [SerializeField] private GameObject agent;
    private Agent _agent;

    // Start is called before the first frame update
    void Start() {
        _agent = agent.GetComponent<Agent>();
    }

    // Update is called once per frame
    void Update() {
        if (!_agent) {
            Debug.LogWarning("agent need to have an Agent Script.");
            return;
        }

        int episode = _agent.CompletedEpisodes;
        int availableSteps = _agent.MaxStep - _agent.StepCount;
        float reward = _agent.GetCumulativeReward();
        text.text = "Episode: " + episode + "\nAvailable Steps: " + availableSteps
                    + "\nReward: " + reward;
    }
}