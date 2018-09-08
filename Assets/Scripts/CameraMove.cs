/****************************************************************
// 文件名：CameraMove
// 文件功能描述：地图较大时控制摄像机随着主角进行移动
// 
// 创建者：Mogg
// 时间：2018/9/3 14:06:18
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public int MapSize;

    private int _l, _r, _up, _down;
    private Transform _camera;
    //目标位置
    private Vector2 _target;
    private Rigidbody2D _rigid;
    private float _curtime;
    private float _resttime;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        _curtime = 0;
        _resttime = 0.5f;
        _l = _down = 0;
        _r = 7;
        _up = 7;

        _camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _rigid = _camera.GetComponent<Rigidbody2D>();
        _camera.position = new Vector3(4.5f, 4.5f,-10);
        _target = _camera.position;
    }

    //控制摄像头的移动
    public void Move(int h, int v)
    {
        _rigid.MovePosition(Vector2.Lerp(transform.position, _target, Time.deltaTime * 5));
        if (_curtime < _resttime)
        {
            _curtime += Time.deltaTime;
            return;
        }

        Vector2 tmp = new Vector2(h, v);

        if (h != 0)
        {
            if (_l + h < 0 || _r + h >= MapSize) return;
            _l += h;
            _r += h;
        }

        if (v != 0)
        {
            if (_down + v < 0 || _up + v >= MapSize) return;
            _down += v;
            _up += v;
        }

        if (h != 0 || v != 0)
        {
            _target += tmp;

            _curtime = 0;
        }
    }
}