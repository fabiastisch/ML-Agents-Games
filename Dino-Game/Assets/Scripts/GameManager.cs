using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Counter counter;

    public void ResetGame() {
        counter.ResetCounter();
    }
}