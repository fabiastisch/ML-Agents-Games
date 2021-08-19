using System.Collections.Generic;
using UnityEngine;

public static class DebugUtils {
    public static void Draw2DGizmos(Vector3 startPos, bool[,] array) {
        Vector3 pos = startPos;
        Color filled = new Color(0, 1, 0, 0.5f);
        Color unfilled = new Color(0.2f, 0, 0, 0.5f);
        for (int i = 0; i < array.GetLength(0); i++) {
            for (int j = 0; j < array.GetLength(1); j++) {
                _DrawGizmosCube(pos, array[i, j] ? filled : unfilled);
                pos.x++;
            }

            pos.x = startPos.x;
            pos.y++;
        }
    }

    public static void Draw2DListGizmos(Vector3 startPos, List<float> list, int lineLenth) {
        int listLength = list.Count;
        int vLength = listLength / lineLenth;
        bool[,] a = new bool[lineLenth, vLength];
        int counter = 0;
        for (int i = 0; i < a.GetLength(0); i++) {
            for (int j = 0; j < a.GetLength(1); j++) {
                a[i, j] = list[counter++] > 0f;
            }
        }

        Draw2DGizmos(startPos, a);
    }

    private static void _DrawGizmosCube(Vector3 pos, Color color) {
        Gizmos.color = color;
        Gizmos.DrawCube(pos, Vector3.one);
    }
}