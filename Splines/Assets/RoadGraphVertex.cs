using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadGraphVertex
{
    public GameObject StartPoint;
    public GameObject EndPoint;
    public int RoadType = 0;
    public enum Direction
    {
        TwoWay,
        StartToEnd,
        EndToStart
    }
    //public Direction Direction;

    public RoadGraphVertex(GameObject start, GameObject end)
    {
        this.StartPoint = start;
        this.EndPoint = end;
    }

    public static bool Equals(RoadGraphVertex point1, RoadGraphVertex point2)
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

    public bool Equals(RoadGraphVertex roadSegment)
    {
        return RoadGraphVertex.Equals(this, roadSegment);
    }
}
