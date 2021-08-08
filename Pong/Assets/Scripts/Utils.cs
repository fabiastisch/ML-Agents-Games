using UnityEngine;

public class Utils {
    public static float getDistanceBetweenGameObjects(GameObject g1, GameObject g2) {
        return Vector3.Magnitude(g1.transform.position - g2.transform.position);
    }

    public static Vector2 getDistanceVector2(GameObject g1, GameObject g2) {
        var v3 = getDistanceVector3(g1, g2);
        return new Vector2(v3.x, v3.y);
    }

    public static Vector3 getDistanceVector3(GameObject g1, GameObject g2) {
        return g1.transform.position - g2.transform.position;
    }
}