using System.Collections.Generic;
using UnityEngine;

public class GridHoverController : MonoBehaviour
{
    public GridManager grid;
    public GameObject hoverPrefab;

    private Dictionary<Vector2Int, GameObject> hoverCells = new();
    private Vector2Int currentCell = new(-1, -1);

    private void Start()
    {
        CreateAllHoverCells();
        HideAll();
    }

    void CreateAllHoverCells()
    {
        for (int x = 0; x < grid.cols; x++)
        {
            for (int y = 0; y < grid.rows; y++)
            {
                Vector2Int cell = new(x, y);
                Vector3 pos = grid.CellToWorldCenter(cell);

                GameObject h = Instantiate(hoverPrefab, pos + Vector3.up * 0.02f, Quaternion.Euler(90f, 0f, 0f));
                h.transform.localScale = new Vector3(grid.cellSize, 1.8f, grid.cellSize);
                h.SetActive(false);

                hoverCells[cell] = h;
            }
        }
    }

    public void Show(Vector2Int cell)
    {
        if (cell == currentCell) return;

        HideAll();

        currentCell = cell;

        hoverCells[cell].SetActive(true);

        for (int x = 0; x < grid.cols; x++)
        {
            Vector2Int c = new(x, cell.y);
            if (c != cell)
                hoverCells[c].SetActive(true);
        }

        for (int y = 0; y < grid.rows; y++)
        {
            Vector2Int c = new(cell.x, y);
            if (c != cell)
                hoverCells[c].SetActive(true);
        }
    }

    public void HideAll()
    {
        foreach (var h in hoverCells.Values)
            h.SetActive(false);

        currentCell = new Vector2Int(-1, -1);
    }
}
