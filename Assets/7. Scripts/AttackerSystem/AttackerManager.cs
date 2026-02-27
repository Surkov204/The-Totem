using System.Collections.Generic;
using UnityEngine;

public class AttackerManager : MonoBehaviour
{
    public static AttackerManager Instance { get; private set; }

    // lane -> list attackers
    private readonly Dictionary<int, List<Attacker>> _byLane = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(Attacker a)
    {
        if (a == null) return;
        int lane = a.Lane;
        if (lane < 0) return;

        if (!_byLane.TryGetValue(lane, out var list))
        {
            list = new List<Attacker>(16);
            _byLane[lane] = list;
        }

        if (!list.Contains(a))
            list.Add(a);
    }

    public void Unregister(Attacker a)
    {
        if (a == null) return;
        int lane = a.Lane;
        if (lane < 0) return;

        if (_byLane.TryGetValue(lane, out var list))
        {
            list.Remove(a);
        }
    }

    public IReadOnlyList<Attacker> GetLane(int lane)
    {
        if (_byLane.TryGetValue(lane, out var list)) return list;
        return null;
    }

    public IEnumerable<Attacker> GetAllAttackers()
    {
        foreach (var pair in _byLane)
        {
            var list = pair.Value;

            if (list == null) continue;

            for (int i = 0; i < list.Count; i++)
            {
                var attacker = list[i];
                if (attacker != null)
                    yield return attacker;
            }
        }
    }

    public int AliveCount()
    {
        int count = 0;

        foreach (var pair in _byLane)
        {
            var list = pair.Value;
            if (list == null) continue;

            for (int i = 0; i < list.Count; i++)
            {
                var a = list[i];
                if (a != null && !a.IsDead)
                    count++;
            }
        }

        return count;
    }
}