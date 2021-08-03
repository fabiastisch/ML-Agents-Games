using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private GameObject obstacle;

    [SerializeField] private float startTime;

    [SerializeField] private float repeatRate;

    // Start is called before the first frame update
    void Start() {
        InvokeRepeating(nameof(SpawnOstacles), startTime, repeatRate);
    }

    void SpawnOstacles() {
        Instantiate(this.obstacle, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {
    }
}