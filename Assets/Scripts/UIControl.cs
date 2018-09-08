/****************************************************************
// 文件名：UIControl
// 文件功能描述：控制UI显示
// 
// 创建者：Mogg
// 时间：2018/9/4 13:59:48
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;

    public void Init()
    {
        Win.transform.localPosition = new Vector3(1920, 0, 0);
        Lose.transform.localPosition = new Vector3(1920, 0, 0);
    }

    public void WinShow(int s)
    {
        Win.transform.localPosition = new Vector3(0,0,0);
        Text score = GameObject.Find("Score").GetComponent<Text>();
        score.text = "You Score : " + s;
    }

    public void LoseShow()
    {
        Lose.transform.localPosition = new Vector3(0, 0, 0);
    }
}
