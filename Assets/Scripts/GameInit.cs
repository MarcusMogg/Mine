/****************************************************************
// 文件名：GameInit
// 文件功能描述：初始化游戏
// 
// 创建者：Mogg
// 时间：2018/9/2 14:22:02
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject gm;
    //单例模式
    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("GameController") == null)
        {
            Instantiate(gm);
        }
    }
}
