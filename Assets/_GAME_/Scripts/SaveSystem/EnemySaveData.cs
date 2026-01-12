using UnityEngine;

[System.Serializable]
public class EnemySaveData : MonoBehaviour
{
    public string enemyId;
    public string enemyDataId;
    public Vector3 position;
    public float health;
    public EnemyStateType state;

    public enum EnemyStateType
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

}
