using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode()]
public class RoadNetwork : MonoBehaviour
{
    public bool DisplayWireframe = true;

    List<RoadNode> nodes;
    public delegate void RoadModificationHandler(object sender, EventArgs e);
    public event RoadModificationHandler OnRoadModification;

	void Start()
    {
        nodes = new List<RoadNode>();

        // Remove this later
        RoadNode firstNode = CreateNode(new Vector3(5f, 0, 5f));
        RoadNode secondNode = CreateNode(new Vector3(10f, 0, 10f), firstNode);
        RoadNode thirdNode = CreateNode(new Vector3(10f, 0, -3f));
        thirdNode.AddConnection(firstNode);
        thirdNode.AddConnection(secondNode);
	}
	
	void Update()
    {
	
	}

    public RoadNode CreateNode(Vector3 position)
    {
        RoadNode newNode = new RoadNode(position);
        nodes.Add(newNode);
        return newNode;
    }

    public RoadNode CreateNode(Vector3 position, RoadNode connection)
    {
        RoadNode newNode = CreateNode(position);
        newNode.AddConnection(connection);

        return newNode;
    }

    // Currently draws a line for every connection.
    // TODO: Change this so that it doesn't draw lines over the same connection twice
    void OnDrawGizmos()
    {
        if (nodes.Count != 0)
        {
            foreach (RoadNode node in nodes)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(node.Position, Vector3.one * 0.5f);
                foreach (RoadNode connection in node.Connections)
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawLine(node.Position, connection.Position);
                }
            }
        }
    }
}
