using UnityEngine;

public static class DebugUtils {
    public static void Draw2DGizmos(Vector3 startPos, bool[,] array) {
        Vector3 pos = startPos;
        Color filled = new Color(0, 1, 0, 0.5f);
        Color unfilled = new Color(0.2f, 0, 0, 0.5f);
        for (int i = 0; i < array.GetLength(0); i++) {
            for (int j = 0; j < array.GetLength(1); j++) {
                _DrawGizmosCube(pos, array[i,j] ? filled : unfilled);
                pos.x++;
            }
            pos.x = startPos.x;
            pos.y++;
        }
    }
    private static void _DrawGizmosCube(Vector3 pos, Color color) {
        Gizmos.color = color;
        Gizmos.DrawCube(pos, Vector3.one);
    }
}