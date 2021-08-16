using Unity.MLAgents.Sensors;
using UnityEngine;

public class My2DArraySensor : ISensor {
    public int height, width;
    public int channels;
    public bool[,] array;
    private My2DArrayObservable observable;

    public My2DArraySensor(My2DArrayObservable observable, int height, int width) {
        this.observable = observable;
        this.height = height;
        this.width = width;
        this.channels = 1;
    }


    public ObservationSpec GetObservationSpec() {
        return ObservationSpec.Visual(height, width, channels);
    }

    public int Write(ObservationWriter writer) {
        int index = 0;
        for (var h = 0; h < height; h++) {
            for (var w = 0; w < width; w++) {
                writer[h, w, 0] = observable.get2DArray()[h, w] ? 1 : 0;
                index++;
            }
        }
        return index;
    }

    public byte[] GetCompressedObservation() {
        return null;
    }

    public void Update() {
    }

    public void Reset() {
    }

    public CompressionSpec GetCompressionSpec() {
        return CompressionSpec.Default();
    }

    public string GetName() {
        return "My2DArraySensor";
    }
}