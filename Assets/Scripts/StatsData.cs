using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsData", menuName = "Data/Stats", order = 1)]
public class StatsData : ScriptableObject
{
    public List<Stat> InitialStatsData;
}
