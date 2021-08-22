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
            int availablesteps = agent.AvailableSteps;
            float reward = agent.GetCumulativeReward();
            text.text = "Episode: " + episode + "\nAvailable Steps: " + availablesteps
                + "\nReward: " + reward;
        }
    }
}