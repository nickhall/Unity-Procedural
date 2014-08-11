using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class RoadGraphEdge
{
    public GameObject StartPoint;
    public GameObject EndPoint;
    public Vector3[] Waypoints;
    public int RoadType = 0;
    public Mesh SegmentMesh;
    public enum Direction
    {
        TwoWay,
        StartToEnd,
        EndToStart
    }
    //public Direction Direction;

    public RoadGraphEdge(GameObject start, GameObject end)
    {
        this.StartPoint = start;
        this.EndPoint = end;
    }

    public static bool Equals(RoadGraphEdge point1, RoadGraphEdge point2)
    {
        if (point1.StartPoint == point2.StartPoint && point1.EndPoint == point2.EndPoint)
        {
            return true;
        }
        else if (point1.StartPoint == point2.EndPoint && point1.EndPoint == point2.StartPoint)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Equals(RoadGraphEdge roadSegment)
    {
        return RoadGraphEdge.Equals(this, roadSegment);
    }
}
