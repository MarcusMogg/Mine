/****************************************************************
// 文件名：GameBegin
// 文件功能描述：
// 
// 创建者：Mogg
// 时间：2018/9/5 9:48:40
******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBegin : MonoBehaviour
{
    public Level L;
    public Text T;
    public GameObject Choose;

    void Start()
    {
        L = GameObject.Find("Level").GetComponent<Level>();
        T = GameObject.Find("ShowLevel").GetComponent<Text>();
        Choose = GameObject.Find("Choose");
        Choose.transform.localPosition = new Vector3(1920, 0, 0);
    }

    public void ChooseShow()
    {
        Choose.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Begin()
    {
        SceneManager.LoadScene("MainScenes");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Plus()
    {
        L.L = Math.Min(L.L + 1, 3);
        T.text = "" + L.L;
    }
    public void Jian()
    {
        L.L = Math.Max(L.L - 1, 1);
        T.text = "" + L.L;
    }
}

