using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode<T>
{
    public T Data;
    public List<GridNode<T>> Adyacents;
}
