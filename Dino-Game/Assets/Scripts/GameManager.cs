using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] public Counter counter;

    public void ResetGame() {
        counter.ResetCounter();
    }
}