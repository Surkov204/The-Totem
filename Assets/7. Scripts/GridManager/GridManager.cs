using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid")]
    public Transform origin;     
    public int rows = 5;
    public int cols = 9;
    public float cellSize = 1f;

    private readonly Dictionary<Vector2Int, GameObject> _occupied = new();

    public bool IsInside(Vector2Int cell)
        => cell.x >= 0 && cell.x < cols && cell.y >= 0 && cell.y < rows;

    public bool IsOccupied(Vector2Int cell) => _occupied.ContainsKey(cell);

    public GameObject GetAt(Vector2Int cell)
        => _occupied.TryGetValue(cell, out var go) ? go : null;

    public void Occupy(Vector2Int cell, GameObject go) => _occupied[cell] = go;

    public void Unoccupy(Vector2Int cell) => _occupied.Remove(cell);

    public bool TryWorldToCell(Vector3 worldPos, out Vector2Int cell)
    {
        var o = origin.position;
        float dx = worldPos.x - o.x;
        float dz = worldPos.z - o.z;

        int c = Mathf.FloorToInt(dx / cellSize);
        int r = Mathf.FloorToInt(dz / cellSize);

        cell = new Vector2Int(c, r);
        return dx >= 0 && dz >= 0 && IsInside(cell);
    }

    public Vector3 CellToWorldCenter(Vector2Int cell)
    {
        var o = origin.position;
        float x = o.x + cell.x * cellSize + cellSize * 0.5f;
        float z = o.z + cell.y * cellSize + cellSize * 0.5f;

        return new Vector3(x, o.y, z);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!origin) return;
        Gizmos.color = Color.yellow;
        var o = origin.position;

        // draw outline
        Vector3 size = new Vector3(cols * cellSize, rows * cellSize, 0);
        Vector3 center = o + new Vector3(size.x / 2f, size.y / 2f, 0);
        Gizmos.DrawWireCube(center, size);

        // draw cell lines
        Gizmos.color = Color.gray;
        for (int c = 0; c <= cols; c++)
        {
            Vector3 a = o + new Vector3(c * cellSize, 0, 0);
            Vector3 b = o + new Vector3(c * cellSize, 0, rows * cellSize);
            Gizmos.DrawLine(a, b);
        }

        for (int r = 0; r <= rows; r++)
        {
            Vector3 a = o + new Vector3(0, 0, r * cellSize);
            Vector3 b = o + new Vector3(cols * cellSize, 0, r * cellSize);
            Gizmos.DrawLine(a, b);
        }
    }
#endif
}
