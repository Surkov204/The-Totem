using UnityEngine;

public class BasicAttackerSpawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GridManager grid;
    [SerializeField] private GameObject attackerPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        if (grid == null || attackerPrefab == null)
            return;

        int lane = Random.Range(0, grid.rows);

        Vector2Int cell = new Vector2Int(grid.cols - 1, lane);
        Vector3 pos = grid.CellToWorldCenter(cell);

        GameObject go = Instantiate(attackerPrefab, pos, Quaternion.Euler(0f,-90f,0f));

        Attacker attacker = go.GetComponent<Attacker>();
        if (attacker != null)
        {
            attacker.SetLane(lane);
        }
    }
}