using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface IGraph<T>
{
    List<T> Nodes
    {
        get;
        set;
    }


    void AddNode(T node);

}
