using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class RoadNode : MonoBehaviour
{
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }
    public List<GameObject> Connections;
    public List<int> RoadType;
    public Vector3 Direction;
    public Vector3 Normal
    {
        get;
        private set;
    }

    void Update()
    {

    }

    public void AddConnection(GameObject node, bool oneWay = false)
    {
        if (Connections.Contains(node))
        {
            throw new ArgumentException("Duplicate node connection added");
        }

        Connections.Add(node);
        if (!oneWay)
        {
            node.GetComponent<RoadNode>().Connections.Add(gameObject);
        }
    }
}