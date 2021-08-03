using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour {
    [SerializeField] private int counter;

    private Text _text;
    // Start is called before the first frame update
    void Start() {
        _text = GetComponent<Text>();
        InvokeRepeating(nameof(UpdateCounter), 0,0.1f);
    }

    void UpdateCounter() {
        _text.text = (++counter).ToString();
    }

}