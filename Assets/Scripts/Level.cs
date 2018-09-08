/****************************************************************
// 文件名：Level
// 文件功能描述：
// 
// 创建者：Mogg
// 时间：2018/9/7 10:57:54
******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public int L = 1;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
