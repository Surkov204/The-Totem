using UnityEngine;

public class LaneCombatSystem : MonoBehaviour
{
    public static LaneCombatSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Trả về attacker gần nhất phía trước trong cùng lane, trong khoảng range.
    /// forwardSign = +1: phía trước là target.x > shooter.x
    /// forwardSign = -1: phía trước là target.x < shooter.x
    /// </summary>
    public Attacker FindNearestAheadInRange(int lane, float shooterX, float range, int forwardSign = +1)
    {
        var list = AttackerManager.Instance != null ? AttackerManager.Instance.GetLane(lane) : null;
        if (list == null || list.Count == 0) return null;

        Attacker best = null;
        float bestDx = float.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {
            var a = list[i];
            if (a == null) continue;

            float dx = (a.transform.position.x - shooterX) * forwardSign;

            if (dx <= 0f) continue;

            if (dx > range) continue;

            if (dx < bestDx)
            {
                bestDx = dx;
                best = a;
            }
        }

        return best;
    }
}