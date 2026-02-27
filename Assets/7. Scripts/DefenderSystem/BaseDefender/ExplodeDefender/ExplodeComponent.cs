using UnityEngine;
using DG.Tweening;

public class ExplodeComponent : BaseDefender
{
    public enum Shape { Tile3x3, Tile5x5, Lane, Column }

    [SerializeField] private Shape shape = Shape.Tile3x3;
    [SerializeField] private GameObject explosionVFX;
    private bool exploded;

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayDropAndExplode(Vector3 targetPos)
    {
        float groundY = targetPos.y;
        float bounceY = 1f;

        Vector3 startPos = targetPos + Vector3.up * 5f;

        transform.position = startPos;
        transform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(targetPos, 0.4f)
            .SetEase(Ease.InQuad));

        seq.Append(transform.DOScale(
            new Vector3(1.3f, 0.6f, 1.3f), 0.08f));

        seq.Append(transform.DOMoveY(bounceY, 0.2f)
            .SetEase(Ease.OutQuad));

        seq.Join(transform.DOScale(
            new Vector3(0.9f, 1.2f, 0.9f), 0.2f));

        seq.AppendInterval(0.1f);

        seq.Append(transform.DOScale(2.2f, 0.15f)
            .SetEase(Ease.OutExpo));

        seq.AppendCallback(() =>
        {
            ApplyAoE(data.damage);
            SpawnExplosion();
        });

        seq.Append(transform.DOScale(0f, 0.2f)
            .SetEase(Ease.InBack));

        seq.OnComplete(Die);
    }

    private void ApplyAoE(int damage)
    {
        var grid = PlacementManager.Instance.grid;

        if (!grid.TryWorldToCell(transform.position, out Vector2Int center))
            return;

        int range = shape switch
        {
            Shape.Tile3x3 => 1,
            Shape.Tile5x5 => 2,
            _ => 1
        };

        int minRow = center.y - range;
        int maxRow = center.y + range;

        var victims = new System.Collections.Generic.List<Attacker>(32);

        for (int row = minRow; row <= maxRow; row++)
        {
            var lane = AttackerManager.Instance.GetLane(row);
            if (lane == null) continue;

            for (int i = 0; i < lane.Count; i++)
            {
                var attacker = lane[i];
                if (attacker == null) continue;

                if (!grid.TryWorldToCell(attacker.transform.position, out Vector2Int cell))
                    continue;

                if (Mathf.Abs(cell.x - center.x) <= range)
                    victims.Add(attacker);
            }
        }

        for (int i = 0; i < victims.Count; i++)
        {
            var a = victims[i];
            if (a != null)
                a.TakeDamage(damage);
        }
    }

    private void SpawnExplosion()
    {
        if (explosionVFX == null) return;

        GameObject vfx = Instantiate(
            explosionVFX,
            transform.position,
            Quaternion.identity
        );

        Destroy(vfx, 2f);
    }

    private bool IsInShape(Vector2Int center, Vector2Int cell)
    {
        int dx = Mathf.Abs(cell.x - center.x);
        int dy = Mathf.Abs(cell.y - center.y);

        return shape switch
        {
            Shape.Tile3x3 => dx <= 1 && dy <= 1,
            Shape.Tile5x5 => dx <= 2 && dy <= 2,
            Shape.Lane => dy == 0,
            Shape.Column => dx == 0,
            _ => false
        };
    }
}