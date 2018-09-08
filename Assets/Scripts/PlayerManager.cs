/****************************************************************
// 文件名：PlayerManager
// 文件功能描述：控制角色行为
// 
// 创建者：Mogg
// 时间：2018/9/1 15:44:50
******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Staus
{
    Win,
    Lose,
    Gaming,
    Over
}


public class PlayerManager : MonoBehaviour
{
    //目标位置
    private Vector2 _target;
    //获取刚体以进行移动
    private Rigidbody2D _rigid;
    private BoxCollider2D _collider;
    //速度
    public int Speed;
    //控制移动间隔
    private float _curtime;
    private float _resttime;

    private Animator _animator;

    public ObsManager Obs;

    private int _hp;

    public Staus Staus;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        _curtime = 0;
        _resttime = 0.5f;
        _target = new Vector2(0, 1);
        _hp = 1;
        
        _animator = gameObject.GetComponent<Animator>();
        _rigid = gameObject.GetComponent<Rigidbody2D>();
        _collider = gameObject.GetComponent<BoxCollider2D>();
        Obs = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObsManager>();

        Staus = Staus.Gaming;
    }

    //键盘控制移动
    public void Move(int h , int v)
    {
        _rigid.MovePosition(Vector2.Lerp(transform.position, _target, Time.deltaTime * Speed));
        if (_curtime < _resttime)
        {
            _curtime += Time.deltaTime;
            return;
        }

        Vector2 tmp = new Vector2(h, v);

        if (h != 0 || v != 0)
        {
            if (_target.x + tmp.x < 0) return;
            _collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(_target, _target + tmp);
            _collider.enabled = true;
            if (hit.transform == null) _target += tmp;
            else
            {
                switch (hit.collider.tag)
                {
                    case "Wall":
                        break;
                    case "Obs":
                        _animator.SetTrigger("Atack");
                        Obs.Hit(_target + tmp);
                        _target += tmp;
                        break;
                    case "Exit":
                        //todo:进入结算，胜利
                        Staus = Staus.Win;
                        break;
                }
            }
            _curtime = 0;
        }
    }

    public void Injure()
    {
        _hp--;
        _animator.SetTrigger("Injure");
        if (_hp <= 0)
        {
            //todo:进入结算
            Staus = Staus.Lose;
        }
    }
}
