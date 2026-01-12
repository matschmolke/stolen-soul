using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : MonoBehaviour
{
    public List<ChestData> chests;
    public List<EnemySaveData> enemies;
}
