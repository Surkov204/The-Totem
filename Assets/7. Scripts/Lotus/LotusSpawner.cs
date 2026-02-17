using UnityEngine;
using System.Collections;

public class LotusSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lotusPrefab;
    [SerializeField] private float firstSpawnDelay = 3f;
    [SerializeField] private float spawnInterval = 5f;

    private float timer;
    private GridManager grid;

    private void Start()
    {
        grid = FindObjectOfType<GridManager>();
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(firstSpawnDelay);
        SpawnLotus();

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnLotus();
        }
    }

    private void SpawnLotus()
    {
        for (int i = 0; i < 20; i++)
        {
            int col = Random.Range(0, grid.cols);
            int row = Random.Range(0, grid.rows);

            Vector2Int cell = new Vector2Int(col, row);

            if (!grid.IsOccupied(cell))
            {
                Vector3 pos = grid.CellToWorldCenter(cell);
                pos.y = 3f;

                GameObject lotus = Instantiate(lotusPrefab, pos, Quaternion.identity);

                lotus.GetComponent<Lotus>()
                     .SetTargetUI(SunManager.Instance.GetSunTarget());

                return;
            }
        }
    }
}
