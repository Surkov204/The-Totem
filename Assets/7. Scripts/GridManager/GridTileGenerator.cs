using UnityEngine;

[ExecuteAlways]
public class GridTileGenerator : MonoBehaviour
{
    [Header("Grid Size")]
    public int rows = 5;
    public int cols = 9;
    public float cellSize = 1f;

    [Header("Prefabs")]
    public GameObject tilePrefabA;   // kéo cube A
    public GameObject tilePrefabB;   // kéo cube B

    [Header("Parent")]
    public Transform tileRoot;

    [ContextMenu("Generate")]
    public void Generate()
    {
        if (!tilePrefabA || !tilePrefabB)
        {
            Debug.LogError("GridTileGenerator: Missing tile prefabs");
            return;
        }

        if (!tileRoot)
        {
            tileRoot = new GameObject("GridTiles").transform;
            tileRoot.SetParent(transform);
        }

        Clear();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                bool useA = (r + c) % 2 == 0;
                GameObject prefab = useA ? tilePrefabA : tilePrefabB;

                Vector3 pos = new Vector3(
                    transform.position.x + c * cellSize + cellSize * 0.5f,
                    transform.position.y,
                    transform.position.z + r * cellSize + cellSize * 0.5f
                );

                GameObject tile = Instantiate(prefab, pos, Quaternion.identity, tileRoot);
                tile.name = $"Tile_{r}_{c}";
                tile.transform.localScale = new Vector3(cellSize, tile.transform.localScale.y, cellSize);
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        if (!tileRoot) return;

        for (int i = tileRoot.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(tileRoot.GetChild(i).gameObject);
        }
    }
}
