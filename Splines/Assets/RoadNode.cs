using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class RoadNode
{
    Vector3 position;

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    List<RoadNode> connections;

    public List<RoadNode> Connections
    {
        get { return connections; }
        set { connections = value; }
    }

	public RoadNode(Vector3 position)
	{
        connections = new List<RoadNode>();
        this.position = position;
	}

    public void AddConnection(RoadNode node, bool oneWay = false)
    {
        if (connections.Contains(node))
        {
            throw new ArgumentException("Duplicate node connection added");
        }

        connections.Add(node);
        if (!oneWay)
        {
            node.Connections.Add(this);
        }
    }
}