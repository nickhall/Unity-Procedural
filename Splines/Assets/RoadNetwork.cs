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
	}
	
	void Update()
    {
	
	}

    public void CreateNode(Vector3 position)
    {
        //
    }

    public void CreateNode(Vector3 position, RoadNode connection)
    {
        RoadNode newNode = new RoadNode(position);
        newNode.AddConnection(connection);
        nodes.Add(newNode);
    }

    // Currently draws a line for every connection.
    // TODO: Change this so that it doesn't draw lines over the same connection twice
    void OnDrawGizmos()
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
