using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode()]
public class RoadNetwork : MonoBehaviour
{
    public bool DisplayWireframe = true;
    public GameObject RoadNodeType;
    public List<RoadSegment> Segments;

    public List<GameObject> nodes;
    public delegate void RoadModificationHandler(object sender, EventArgs e);
    public event RoadModificationHandler OnRoadModification;

	void Start()
    {
        nodes = new List<GameObject>();

        // Remove this later
        GameObject firstNode = CreateNode(new Vector3(5f, 0, 5f));
        GameObject secondNode = CreateNode(new Vector3(10f, 0, 10f), firstNode);
        GameObject thirdNode = CreateNode(new Vector3(10f, 0, -3f));
        thirdNode.GetComponent<RoadNode>().AddConnection(firstNode);
        thirdNode.GetComponent<RoadNode>().AddConnection(secondNode);

        Segments = new List<RoadSegment>();
	}
	
	void Update()
    {
        foreach (GameObject node in nodes)
        {
            if (node != null)
            {
                foreach (GameObject connection in node.GetComponent<RoadNode>().Connections)
                {
                    Debug.DrawLine(node.transform.position, connection.transform.position, Color.green);
                }
            }
        }
	}

    public GameObject CreateNode(Vector3 position)
    {
        //RoadNode newNode = new RoadNode(position);
        GameObject newNode = Instantiate(RoadNodeType) as GameObject;
        Debug.Log("Created new node: " + newNode);
        newNode.transform.position = position;
        nodes.Add(newNode);
        Debug.Log("Created point!");
        return newNode;
    }

    public GameObject CreateNode(Vector3 position, GameObject connection)
    {
        GameObject newNode = CreateNode(position);
        newNode.GetComponent<RoadNode>().AddConnection(connection);

        return newNode;
    }

    public void RebuildRoadData()
    {
        Segments = new List<RoadSegment>();
        List<GameObject> completedNodes = new List<GameObject>();

        // TODO: Make sure this algorithm isn't terribly slow
        foreach (GameObject node in nodes)
        {
            foreach (GameObject connection in node.GetComponent<RoadNode>().Connections)
            {
                RoadSegment segment = new RoadSegment(node.transform.position, connection.transform.position);
                if (!Segments.Contains(segment)) Segments.Add(segment);
            }
        }

    }

    // Currently draws a line for every connection.
    // TODO: Change this so that it doesn't draw lines over the same connection twice
    void OnDrawGizmos()
    {
        if (nodes.Count != 0)
        {
            foreach (GameObject node in nodes)
            {
                if (node == null)
                    break;
                Gizmos.color = Color.green;
                Gizmos.DrawCube(node.transform.position, Vector3.one * 0.5f);
                foreach (GameObject connection in node.GetComponent<RoadNode>().Connections)
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawLine(node.transform.position, connection.transform.position);
                }
            }
        }
    }
}
