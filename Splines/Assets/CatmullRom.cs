using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class CatmullRom : MonoBehaviour
{
    public GameObject[] Points = new GameObject[4];
    public int CurveResolution = 20;
    public Vector3[] CurveCoordinates;
    public Vector3[] Tangents;
    public bool ClosedLoop = false;
    public struct CatmullRomSpline
    {
        //
    }

    public enum Uniformity
    {
        Uniform,
        Centripetal,
        Chordal
    }

	void Start()
    {
	
	}

    public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
    {
        // Catmull-Rom splines are Hermite curves with special tangent values.
        // Hermite curve formula:
        // (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
        // For points p0 and p1 passing through points m0 and m1 interpolated over t = [0, 1]
        // Tangent M[k] = (P[k+1] - P[k-1]) / 2
        // With [] indicating subscript
        Vector3 position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
            + (t * t * t - 2.0f * t * t + t) * tanPoint1
            + (-2.0f * t * t * t + 3.0f * t * t) * end
            + (t * t * t - t * t) * tanPoint2;

        return position;
    }

    public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t, out Vector3 tangent)
    {
        // Calculate tangents
        // p'(t) = (6t² - 6t)p0 + (3t² - 4t + 1)m0 + (-6t² + 6t)p1 + (3t² - 2t)m1
        tangent = (6 * t * t - 6 * t) * start
            + (3 * t * t - 4 * t + 1) * tanPoint1
            + (-6 * t * t + 6 * t) * end
            + (3 * t * t - 2 * t) * tanPoint2;
        return Interpolate(start, end, tanPoint1, tanPoint2, t);
    }

    public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t, out Vector3 tangent, out Vector3 curvature)
    {
        // Calculate second derivative (curvature)
        // p''(t) = (12t - 6)p0 + (6t - 4)m0 + (-12t + 6)p1 + (6t - 2)m1
        curvature = (12 * t - 6) * start
            + (6 * t - 4) * tanPoint1
            + (-12 * t + 6) * end
            + (6 * t - 2) * tanPoint2;
        return Interpolate(start, end, tanPoint1, tanPoint2, t, out tangent);

    }

    public static float[] GetNonuniformT(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float alpha)
    {
        // See here: http://stackoverflow.com/a/23980479/837825
        // C'(t1) = (P1 - P0) / (t1 - t0) - (P2 - P0) / (t2 - t0) + (P2 - P1) / (t2 - t1)
        // C'(t2) = (P2 - P1) / (t2 - t1) - (P3 - P1) / (t3 - t1) + (P3 - P2) / (t3 - t2)
        float[] values = new float[4];
        for (int i = 0; i < 4; i++)
        {
            //values[i] = Mathf.Pow(Vector3.SqrMagnitude())
            break;
        }

        return values;
    }
	
	void Update()
    {
        Vector3 p0;
        Vector3 p1;
        Vector3 m0;
        Vector3 m1;
        int pointsToMake;

        if (ClosedLoop == true)
        {
            pointsToMake = (CurveResolution) * (Points.Length);
        }
        else
        {
            pointsToMake = (CurveResolution) * (Points.Length - 1);
        }

        CurveCoordinates = new Vector3[pointsToMake];
        Tangents = new Vector3[pointsToMake];

        int closedAdjustment = ClosedLoop ? 0 : 1;

        // First for loop goes through each individual control point and connects it to the next, so 0-1, 1-2, 2-3 and so on
        for (int i = 0; i < Points.Length - closedAdjustment; i++)
        {
            //if (Points[i] == null || Points[i + 1] == null || (i > 0 && Points[i - 1] == null) || (i < Points.Length - 2 && Points[i + 2] == null))
            //{
            //    return;
            //}

            p0 = Points[i].transform.position;
            p1 = (ClosedLoop == true && i == Points.Length - 1) ? Points[0].transform.position : Points[i + 1].transform.position;

            // Tangent calculation for each control point
            // Tangent M[k] = (P[k+1] - P[k-1]) / 2
            // With [] indicating subscript

            // m0
            if (i == 0)
            {
                m0 = ClosedLoop ? 0.5f * (p1 - Points[Points.Length - 1].transform.position) : p1 - p0;
            }
            else
            {
                m0 = 0.5f * (p1 - Points[i - 1].transform.position);
            }

            // m1
            if (ClosedLoop)
            {
                if (i == Points.Length - 1)
                {
                    m1 = 0.5f * (Points[(i + 2) % Points.Length].transform.position - p0);
                }
                else if (i == 0)
                {
                    m1 = 0.5f * (Points[i + 2].transform.position - p0);
                }
                else
                {
                    m1 = 0.5f * (Points[(i + 2) % Points.Length].transform.position - p0);
                }
            }
            else
            {
                if (i < Points.Length - 2)
                {
                    m1 = 0.5f * (Points[(i + 2) % Points.Length].transform.position - p0);
                }
                else
                {
                    m1 = p1 - p0;
                }
            }

            Vector3 position;
            float t;
            float pointStep = 1.0f / CurveResolution;

            if ((i == Points.Length - 2 && ClosedLoop == false) || (i == Points.Length - 1 && ClosedLoop))
            {
                pointStep = 1.0f / (CurveResolution - 1);
                // last point of last segment should reach p1
            }
            // Second for loop actually creates the spline for this particular segment
            for (int j = 0; j < CurveResolution; j++)
            {
                t = j * pointStep;
                Vector3 tangent;
                position = Interpolate(p0, p1, m0, m1, t, out tangent);
                CurveCoordinates[i * CurveResolution + j] = position;
                Tangents[i * CurveResolution + j] = tangent;
                //Debug.DrawRay(position, tangent.normalized * 2, Color.red);
                Debug.DrawLine(position + Vector3.Cross(tangent, Vector3.up).normalized, position - Vector3.Cross(tangent, Vector3.up).normalized, Color.red);
            }
        }

        for (int i = 0; i < CurveCoordinates.Length - 1; ++i)
        {
            Debug.DrawLine(CurveCoordinates[i], CurveCoordinates[i + 1]);
        }

	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < CurveCoordinates.Length; i++)
        {
            Gizmos.DrawWireCube(CurveCoordinates[i], new Vector3(.1f, .1f, .1f));
        }
    }
}
