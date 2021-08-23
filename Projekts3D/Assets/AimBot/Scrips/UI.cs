using UnityEngine;
using UnityEngine.UI;

namespace AimBot.Scrips {
    public class UI : MonoBehaviour {
        [SerializeField] private Text text;
        [SerializeField] private AimBotAgent agent;

        // Start is called before the first frame update
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            int episode = agent.CompletedEpisodes;
            int availableSteps = agent.MaxStep - agent.StepCount;
            float reward = agent.GetCumulativeReward();
            text.text = "Episode: " + episode + "\nAvailable Steps: " + availableSteps
                        + "\nReward: " + reward;
        }
    }
}