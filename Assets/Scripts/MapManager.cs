/****************************************************************
// 文件名：MapManager
// 文件功能描述：控制游戏的地图生成
// 
// 创建者：Mogg
// 时间：2018/9/1 13:05:25
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //存放各种Prefab
    public GameObject[] Walls;
    public GameObject[] Floors;
    public GameObject[] Foods;
    public GameObject[] Numbers;
    public GameObject Exit;

    //地图大小
    public int MapSize { get; set; }
    //雷的数量
    public int MineNums { get; set; }
    //地图二维数组
    public int[,] Map;
    public bool[,] Path;

    private Transform _mapTs;

    // Use this for initialization
    void Awake()
    {
        _mapTs = new GameObject("Map").GetComponent<Transform>();
        _mapTs.SetParent(gameObject.transform);
    }

    //初始化地图，游戏开始时调用
    public void Init()
    {
        MapInit();
        MapDraw();
    }

    //生成二维数组地图
    private void MapInit()
    {
        Path = new bool[MapSize + 2, MapSize + 2];
        Map = new int[MapSize + 2, MapSize + 2];
        Map.Initialize();
        //生成一条通路
        int cx = 1, cy = 1;
        Path[cx, cy] = true;
        for (int i = 1; i < MapSize * 2; i++)
        {
            if (cx < MapSize && cy < MapSize)
            {
                int j = Random.Range(0, 2);
                if (j == 0) cx++;
                else cy++;
            }
            else if (cx == MapSize)
            {
                cy++;
            }
            else
            {
                cx++;
            }
            Path[cx, cy] = true;
        }

        //生成Minenums个雷
        for (int i = 0; i < MineNums; i++)
        {
            int indexX = Random.Range(1, MapSize + 1);
            int indexY = Random.Range(1, MapSize + 1);
            if (Map[indexX, indexY] == 0 && !Path[indexX,indexY])
            {
                Map[indexX, indexY] = 9;
            }
            else
            {
                i--;
            }
        }
        //没有雷的地方统计周围雷的数量
        int[] tmp = new[] { 1, 0, -1 };
        for (int i = 1; i < MapSize + 1; i++)
        {
            for (int j = 1; j < MapSize + 1; j++)
            {
                if (Map[i, j] == 9) continue;

                for (int dx = 0; dx < 3; dx++)
                {
                    for (int dy = 0; dy < 3; dy++)
                    {
                        int x = i + tmp[dx];
                        int y = j + tmp[dy];
                        if (Map[x, y] == 9)
                        {
                            Map[i, j]++;
                        }
                    }
                }
            }
        }
    }

    //地图绘制
    private void MapDraw()
    {
        for (int i = 0; i < MapSize + 2; i++)
        {
            for (int j = 0; j < MapSize + 2; j++)
            {
                //进出口
                if (i == 0 && j == 1 || i == MapSize + 1 && j == MapSize)
                {
                    RandomInstantiate(Floors, i, j, _mapTs);
                    continue;
                }
                //墙
                if (i == 0 || j == 0 || i == MapSize + 1 || j == MapSize + 1)
                {
                    RandomInstantiate(Walls, i, j, _mapTs);
                }
                //雷
                else
                {
                    RandomInstantiate(Floors, i, j, _mapTs);
                    if (Map[i, j] == 9)
                    {
                        //RandomInstantiate(Enemy, i, j, _mapTs);
                    }
                    else if (Map[i, j] != 0)
                    {
                        GameObject go = Instantiate(Numbers[Map[i, j]], new Vector2(i, j), Quaternion.identity);
                        go.transform.SetParent(_mapTs);
                    }
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            for (int j = 0; j < MapSize + 2; j++)
            {

                RandomInstantiate(Walls, -i, j, _mapTs);
                RandomInstantiate(Walls, MapSize + 1 + i, j, _mapTs);

            }
        }
        Instantiate(Exit, new Vector2(MapSize + 1, MapSize), Quaternion.identity);
    }

    //随机选择相应种类的Prefab进行绘制/实例化
    public static void RandomInstantiate(GameObject[] pre, int x, int y, Transform ts)
    {
        int index = Random.Range(0, pre.Length);
        GameObject go = Instantiate(pre[index], new Vector2(x, y), Quaternion.identity);
        go.transform.SetParent(ts);
    }

}
