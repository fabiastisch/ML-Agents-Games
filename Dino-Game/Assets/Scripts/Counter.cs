using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour {
    [SerializeField] private int counter;

    private Text _text;
    // Start is called before the first frame update
    void Start() {
        _text = GetComponent<Text>();
        InvokeRepeating(nameof(UpdateText), 0,0.1f);
    }

    void UpdateText() {
        _text.text = (++counter).ToString();
    }

    public void ResetCounter() {
        counter = 0;
    }

    public float getCounts() {
        return counter;
    }
}