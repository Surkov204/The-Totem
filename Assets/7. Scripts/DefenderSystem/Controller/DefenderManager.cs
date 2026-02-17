using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    public static DefenderManager Instance { get; private set; }

    private readonly Dictionary<int, List<BaseDefender>> _byLane = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(BaseDefender defender, int lane)
    {
        if (!_byLane.TryGetValue(lane, out var list))
        {
            list = new List<BaseDefender>();
            _byLane[lane] = list;
        }

        list.Add(defender);
    }

    public void Unregister(BaseDefender defender, int lane)
    {
        if (_byLane.TryGetValue(lane, out var list))
        {
            list.Remove(defender);
        }
    }

    public List<BaseDefender> GetLane(int lane)
    {
        if (_byLane.TryGetValue(lane, out var list))
            return list;

        return null;
    }
}