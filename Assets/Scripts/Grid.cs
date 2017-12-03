using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class Grid<T>
{
    private GridNode<T>[,] _nodes;

    public int Width
    {
        get
        {
            if (_nodes == null)
            {
                Debug.LogWarning("Nodes are null");
                return 0;
            }

            return _nodes.GetLength(0);
        }
    }

    public int Height
    {
        get
        {
            if (_nodes == null)
            {
                Debug.LogWarning("Nodes are null");
                return 0;
            }

            return _nodes.GetLength(1);
        }
    }

    public Grid(int width, int height)
    {
        _nodes = new GridNode<T>[width, height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                _nodes[i, j] = new GridNode<T>();
                _nodes[i, j].Adyacents = new List<GridNode<T>>();
                
                if (i > 0)
                {
                    _nodes[i, j].Adyacents.Add(_nodes[i - 1, j]);
                }
                if (i < Width - 1)
                {
                    _nodes[i, j].Adyacents.Add(_nodes[i + 1, j]);
                }
                if (j > 0)
                {
                    _nodes[i, j].Adyacents.Add(_nodes[i, j - 1]);
                }
                if (j < Height - 1)
                {
                    _nodes[i, j].Adyacents.Add(_nodes[i, j + 1]);
                }

            }
        }
    }

    public GridNode<T>GetNodeAt(int x, int y)
    {
        return _nodes[x, y];
    }

    public void SetNode(int x, int y, T data)
    {
        _nodes[x, y].Data = data;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Height; j++)
            {
                sb.Append(_nodes[i, j].Data + " ");
            }
            sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }
}
