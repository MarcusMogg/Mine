/****************************************************************
// 文件名：ObsManager
// 文件功能描述：障碍物的生成与解除
// 
// 创建者：Mogg
// 时间：2018/9/2 13:09:23
******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObsManager : MonoBehaviour
{
    public int[,] Map;
    public GameObject[,] ObsGameObjects;
    public GameObject[,] FlagGameObjects;
    public GameObject[] ObsPrefab;
    public GameObject FlagPrefab;
    private Transform _obsTs;
    private bool[,] _visited;
    public bool[,] Flaged;

    public EnemyManager Enemy;

    void Awake()
    {
        _obsTs = new GameObject("Obs").transform;
        _obsTs.SetParent(gameObject.transform);
        Enemy = gameObject.GetComponent<EnemyManager>();
    }

    //障碍绘制
    public void ObsDraw()
    {
        int len = Map.GetLength(0);
        ObsGameObjects = new GameObject[len, len];
        FlagGameObjects = new GameObject[len, len];
        _visited = new bool[len, len];
        _visited.Initialize();
        Flaged = new bool[len, len];
        Flaged.Initialize();
        for (int i = 1; i + 1 < len; i++)
        {
            for (int j = 1; j + 1 < len; j++)
            {
                int index = Random.Range(0, ObsPrefab.Length);
                GameObject go = Instantiate(ObsPrefab[index], new Vector2(i, j), Quaternion.identity);
                go.name = String.Format("{0},{1}", i, j);
                go.transform.SetParent(_obsTs);
                ObsGameObjects[i, j] = go;
            }
        }
    }

    //碰到障碍时
    public void Hit(Vector2 pos)
    {
        int len = Map.GetLength(0);
        int i = (int)pos.x, j = (int)pos.y;
        Queue<KeyValuePair<int, int>> q = new Queue<KeyValuePair<int, int>>();
        q.Enqueue(new KeyValuePair<int, int>(i, j));
        //bfs
        while (q.Count != 0)
        {
            i = q.Peek().Key;
            j = q.Peek().Value;
            q.Dequeue();
            Destroy(ObsGameObjects[i, j]);
            if (Flaged[i, j])
            {
                Destroy(FlagGameObjects[i, j]);
                Flaged[i, j] = !Flaged[i, j];
            }
            _visited[i, j] = true;

            if (Map[i, j] != 0) continue;

            for (int di = -1; di < 2; di++)
            {
                for (int dj = -1; dj < 2; dj++)
                {
                    int x = i + di;
                    int y = j + dj;
                    if (x > 0 && x + 1 < len && y > 0 && y + 1 < len)
                    {
                        if (Map[x, y] != 9 && !_visited[x, y])
                        {
                            q.Enqueue(new KeyValuePair<int, int>(x, y));
                        }
                    }
                }
            }
        }

        i = (int)pos.x; j = (int)pos.y;

        if (Map[i, j] == 9)
        {
            Enemy.Atack(i, j);
        }
    }

    //插拔旗子
    public void Flag(string s)
    {
        var ss = s.Split(',');
        int i = Int32.Parse(ss[0]);
        int j = Int32.Parse(ss[1]);
        if (!Flaged[i, j])
        {
            GameObject go = Instantiate(FlagPrefab, new Vector2(i, j), Quaternion.identity);
            go.transform.SetParent(_obsTs);
            FlagGameObjects[i, j] = go;
        }
        else
        {
            Destroy(FlagGameObjects[i, j]);
        }

        Flaged[i, j] = !Flaged[i, j];
    }

    //计算插旗得分
    public int ComputeScore()
    {
        int len = Map.GetLength(0);
        int res = 0;

        for (int i = 1; i + 1 < len; i++)
        {
            for (int j = 1; j + 1 < len; j++)
            {
                if (Flaged[i, j] == true && Map[i, j] == 9)
                {
                    res++;
                }
            }
        }

        return res;
    }

    //游戏结束时揭开所有的雷
    public void Over()
    {
        int len = Map.GetLength(0);

        for (int i = 1; i + 1 < len; i++)
        {
            for (int j = 1; j + 1 < len; j++)
            {
                if (Map[i, j] == 9 && ObsGameObjects[i, j] != null)
                {
                    Destroy(ObsGameObjects[i, j]);
                }
            }
        }
    }
}
