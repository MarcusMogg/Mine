/****************************************************************
// 文件名：GameManager
// 文件功能描述：游戏进程控制
// 
// 创建者：Mogg
// 时间：2018/9/1 16:22:42
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MapManager Map;
    public ObsManager Obs;
    public PlayerManager Player;
    public EnemyManager Enemy;
    public CameraMove CameraMove;

    public Level Le;
    public UIControl Ui;
    public int Score;

    void Awake()
    {
        Map = gameObject.GetComponent<MapManager>();
        Obs = gameObject.GetComponent<ObsManager>();
        Enemy = gameObject.GetComponent<EnemyManager>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        CameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        Ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIControl>();
        Le = GameObject.Find("Level").GetComponent<Level>();
    }
    // Use this for initialization
    void Start()
    {
        MapInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Staus == Staus.Win)
        {
            Win();
        }
        else if (Player.Staus == Staus.Gaming)
        {
            int h = 0, v = 0;


            //处理位移矢量
            {
                if (Input.GetKeyDown(KeyCode.D)) h = 1;
                else if (Input.GetKeyDown("a")) h = -1;
                else if (Input.GetKeyDown("w")) v = 1;
                else if (Input.GetKeyDown("s")) v = -1;
            }

            Player.Move(h, v);
            CameraMove.Move(h, v);

            //检测鼠标点击
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.down);
                
                if (hit && hit.transform.tag == "Obs")
                {
                    var item = hit.transform.name;
                    Obs.Flag(item);
                }
            }
        }
        else if (Player.Staus == Staus.Lose)
        {
            Lose();
        }
    }

    public void MapInit()
    {
        switch (Le.L)
        {
            case 1:
                Map.MapSize = 8;
                Map.MineNums = 10;
                break;
            case 2:
                Map.MapSize = 16;
                Map.MineNums = 40;
                break;
            case 3:
                Map.MapSize = 24;
                Map.MineNums = 99;
                break;
        }
        Map.Init();

        Score = Map.MapSize * Map.MapSize;

        CameraMove.MapSize = Map.MapSize;

        Enemy.Map = Map.Map;
        Enemy.Player = Player;
        Enemy.EnemyDraw();

        Obs.Map = Map.Map;
        Obs.ObsDraw();

        Ui.Init();
    }

    public void Win()
    {
        Score += Obs.ComputeScore();
        Ui.WinShow(Score);
        Player.Staus = Staus.Over;
        Obs.Over();
    }

    public void Lose()
    {
        Ui.LoseShow();
        Player.Staus = Staus.Over;
        Obs.Over();
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("BeginScenes");
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScenes");
    }
}
