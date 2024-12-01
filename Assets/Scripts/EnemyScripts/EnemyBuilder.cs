using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilder
{
    public static T BuildEnemy<T>(Vector3 postion, Transform parent = null) where T : Enemy
    {
        GameObject enemyPrefab = Resources.Load<GameObject>("EnemyPrefabs/" + typeof(T).Name);
        GameObject enemy = GameObject.Instantiate(enemyPrefab, postion, Quaternion.identity, parent);
        return enemy.GetComponent<T>();
    }
}
