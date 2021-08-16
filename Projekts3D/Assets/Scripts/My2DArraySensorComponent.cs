using System;
using Unity.MLAgents.Sensors;
using UnityEngine;

[AddComponentMenu("Fabiastisch/2DArray")]
public class My2DArraySensorComponent : SensorComponent {
    [SerializeField] private Component obj;
    public My2DArrayObservable observable => obj as My2DArrayObservable;
    public int height, width;

    public override ISensor[] CreateSensors() {
        return new ISensor[] {new My2DArraySensor((observable as My2DArrayObservable), height, width)};
    }

    private void OnValidate() {
        if (!(obj is My2DArrayObservable)) {
            obj = null;
        }
    }
}