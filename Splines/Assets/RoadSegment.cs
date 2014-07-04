using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadSegment
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
    public int RoadType = 0;
    public enum Direction
    {
        TwoWay,
        StartToEnd,
        EndToStart
    }
    //public Direction Direction;

    public RoadSegment(Vector3 start, Vector3 end)
    {
        this.StartPoint = start;
        this.EndPoint = end;
    }
}
