using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurve
{
    // Quadratic Bezier curve
    // 3 points: p0, p1, p2, starting and ending on p0 and p2 with p1 as control point.
    // (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
    // t between the range [0, 1]
    // Tangents
    // B'(t) = 2(1-t)(P1-P0) + 2t(P2-P1)

    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 controlPoint;
    Vector3[] curveNodes;

    int resolution;
    float curveWidth;

	void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}

    public static Vector3 Interpolate(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        Vector3 point = (((1 - t) * (1 - t)) * start) + ((2 * (1 - t) * t) * control) + (t * t * end);
        return point;
    }

    public static List<Vector3> GetCurvePoints(Vector3 start, Vector3 control, Vector3 end, int resolution)
    {
        if (resolution < 1)
            throw new System.ArgumentException("Please provide a positive, non-zero resolution.");
        return new List<Vector3>();
    }
}
