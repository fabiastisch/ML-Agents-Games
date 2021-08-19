using System;
using Unity.MLAgents.Sensors;

public class BasicSensorComponent : SensorComponent {
    public override ISensor[] CreateSensors() {
        return new ISensor[] {new BasicSensor()};
    }
}

public class BasicSensor : SensorBase {
    public override void WriteObservation(float[] output) {
    }

    public override ObservationSpec GetObservationSpec() {
        throw new NotImplementedException();
    }

    public override string GetName() {
        return "Basic";
    }
}