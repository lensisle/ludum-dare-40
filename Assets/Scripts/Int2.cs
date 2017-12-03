using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Int2
{
    [SerializeField]
    private int _x;
    public int X
    {
        get
        {
            return _x;
        }
    }

    [SerializeField]
    private int _y;
    public int Y
    {
        get
        {
            return _y;
        }
    }

    public Int2(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public void SetX(int x)
    {
        _x = x;
    }

    public void SetY(int y)
    {
        _y = y;
    }

    public void SetInt2(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public Vector2 GetAsVector()
    {
        return new Vector2(_x, _y);
    }

    public override string ToString()
    {
        return _x + " " + _y;
    }
}
