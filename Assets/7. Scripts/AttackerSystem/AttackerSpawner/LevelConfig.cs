using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnLaneMode
{
    Fixed,
    Random,
    WeakestLane
}

public enum SpawnGateMode
{
    None,
    WaitUntilClear,
    WaitUntilRemaining,
    WaitOrTimeout
}

public enum SpecialEventType
{
    SpawnTombstone,
    DropFromSky,
    RopeSwing,
    BossPhase
}

public enum WaveTriggerMode
{
    TimeOnly,
    WhenPreviousCleared,
    TimeOrCleared
}



//
// ==========================
// SPAWN ENTRY (1 loại zombie)
// ==========================
//

[Serializable]
public class SpawnEntry
{
    public GameObject prefab;

    [Min(1)] public int count = 1;

    [Min(0f)] public float interval = 0.2f;

    public SpawnLaneMode laneMode = SpawnLaneMode.Random;

    [Min(0)] public int fixedLane = 0;

    public bool overrideSpawnColumn = false;

    [Min(0)] public int spawnColumn = 0;
}

//
// ==========================
// SPAWN EVENT (1 phase trong wave)
// ==========================
//

[Serializable]
public class SpawnEvent
{
    [Min(0f)]
    public float delayFromWaveStart = 0f;

    public List<SpawnEntry> entries = new();

    public SpawnGateMode gateMode = SpawnGateMode.None;

    [Min(0)]
    public int remainingThreshold = 0;

    [Min(0f)]
    public float timeout = 8f;
}

//
// ==========================
// WAVE TIMELINE
// ==========================
//

[Serializable]
public class WaveTimeline
{
    [Min(0f)]
    public float startTime = 0f;

    public WaveTriggerMode triggerMode = WaveTriggerMode.TimeOnly;

    public bool isHugeWave = false;

    [Min(0f)]
    public float hugeWaveWarningLeadTime = 2.0f;

    [TextArea(2, 4)]
    public string waveTitle;  

    public List<SpawnEvent> spawnEvents = new();
}

//
// ==========================
// SPECIAL EVENT
// ==========================
//

[Serializable]
public class SpecialWorldEvent
{
    [Min(0f)]
    public float triggerTime = 0f;

    public SpecialEventType type;

    public int lane = 0;

    public int intParam = 0;
}

//
// ==========================
// LEVEL CONFIG (ScriptableObject)
// ==========================
//

[CreateAssetMenu(menuName = "PVZ/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Min(1f)]
    public float totalDuration = 120f;

    [TextArea(2, 4)]
    public string introText = "Let's Start!";

    public List<WaveTimeline> waves = new();

    public List<SpecialWorldEvent> specialEvents = new();
}
