/****************************************************************
// 文件名：EnemyManager
// 文件功能描述：控制敌人的生成和动画
// 
// 创建者：Mogg
// 时间：2018/9/3 11:19:51
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    public int[,] Map;
    public GameObject[] EnemyPrefabs;
    public Dictionary<KeyValuePair<int, int>, GameObject> EnemyGameObjects;
    private Transform _enemyTs;
    private Animator _animator;

    public PlayerManager Player;

    void Awake()
    {
        _enemyTs = new GameObject("Enemy").GetComponent<Transform>();
        _enemyTs.SetParent(gameObject.transform);
        EnemyGameObjects = new Dictionary<KeyValuePair<int, int>, GameObject>();
    }

    public void EnemyDraw()
    {
        int len = Map.GetLength(0);
        for (int i = 1; i + 1 < len; i++)
        {
            for (int j = 1; j + 1 < len; j++)
            {
                if (Map[i, j] == 9)
                {
                    int index = Random.Range(0, EnemyPrefabs.Length);
                    GameObject go = Instantiate(EnemyPrefabs[index], new Vector2(i, j), Quaternion.identity);
                    go.transform.SetParent(_enemyTs);
                    EnemyGameObjects[new KeyValuePair<int, int>(i,j)] = go;
                }
            }
        }
    }

    public void Atack(int i , int j)
    {
        _animator = EnemyGameObjects[new KeyValuePair<int, int>(i, j)].GetComponent<Animator>();
        _animator.SetTrigger("Atack");
        Player.Injure();
    }
}

