using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadNetwork : MonoBehaviour
{
    public bool DisplayWireframe = true;
    public GameObject RoadNodeType;
    public List<RoadGraphVertex> Segments;

    public List<GameObject> nodes;
    public delegate void RoadModificationHandler(object sender, EventArgs e);
    public event RoadModificationHandler OnRoadModification;

    List<GameObject> nodeBuffer;

	void Start()
    {
        nodes = new List<GameObject>();
        nodeBuffer = new List<GameObject>();
        Segments = new List<RoadGraphVertex>();
	}
	
	void Update()
    {
        foreach (GameObject node in nodes)
        {
            if (node != null)
            {
                foreach (GameObject connection in node.GetComponent<RoadNode>().Connections)
                {
                    //Debug.DrawLine(node.transform.position, connection.transform.position, Color.green);
                }
            }
        }
	}

    public GameObject CreateNode(Vector3 position)
    {
        GameObject newNode = Instantiate(RoadNodeType) as GameObject;
        newNode.transform.position = position;
        nodes.Add(newNode);

        return newNode;
    }

    public GameObject CreateNode(Vector3 position, GameObject connection)
    {
        GameObject newNode = CreateNode(position);
        newNode.GetComponent<RoadNode>().AddConnection(connection);

        return newNode;
    }

    public GameObject CreateBufferedNode(Vector3 position)
    {
        GameObject newNode = Instantiate(RoadNodeType) as GameObject;
        newNode.transform.position = position;
        nodeBuffer.Add(newNode);

        return newNode;
    }

    public void ApplyBuffer()
    {
        foreach (GameObject node in nodeBuffer)
        {
            nodes.Add(node);
        }

        ClearBuffer();
    }

    public void ClearBuffer()
    {
        nodeBuffer.Clear();
    }

    public void DestroyBuffer()
    {
        foreach (GameObject node in nodeBuffer)
        {
            Destroy(node);
        }
        ClearBuffer();
    }

    public void RebuildRoadData()
    {
        Segments = new List<RoadGraphVertex>();
        List<GameObject> completedNodes = new List<GameObject>();

        // TODO: Make sure this algorithm isn't terribly slow
        foreach (GameObject node in nodes)
        {
            foreach (GameObject connection in node.GetComponent<RoadNode>().Connections)
            {
                RoadGraphVertex segment = new RoadGraphVertex(node, connection);
                if (!Segments.Contains(segment))
                {
                    Segments.Add(segment);
                    Debug.Log("Segment added");
                }
            }
        }

    }

    public static bool IsValidConnection(GameObject start, GameObject end)
    {
        if (RoadNode.Equals(start, end))
        {
            return false;
        }
        if (start.GetComponent<RoadNode>().Connections.Contains(end))
        {
            return false;
        }

        return true;
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
