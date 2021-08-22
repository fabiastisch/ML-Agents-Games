using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Utils {
    public static float getDistanceBetweenGameObjects(GameObject g1, GameObject g2) {
        return Vector3.Magnitude(g1.transform.position - g2.transform.position);
    }

    public static int GetRandomInt(int min, int max) {
        return Random.Range(min, max + 1);
    }

    public static int GetRandomInt(int max) {
        return Random.Range(0, max + 1);
    }

    public static int CheckPositionBox(Vector3 center, Collider[] colliders, int layerMask) {
        return Physics.OverlapBoxNonAlloc(center, Vector3.one * 0.4f, colliders, Quaternion.identity, layerMask);
    }

    public static Collider[] CheckPosBox(Vector3 center) {
        return Physics.OverlapBox(center, Vector3.one * 0.4f, Quaternion.identity);
    }

    public static List<float> ArrayToList(bool[,] array) {
        List<float> list = new List<float>();
        for (int i = 0; i < array.GetLength(0); i++) {
            for (int j = 0; j < array.GetLength(1); j++) {
                list.Add(array[i, j] ? 1 : 0);
            }
        }
        return list;
    }

    public static Quaternion getRotationToLookAt(GameObject g1, GameObject target) {
        return Quaternion.LookRotation(target.transform.position - g1.transform.position);
    }
}