using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class Line : MonoBehaviour
{
    public Transform[] InputNodes;
    public int LineResolution = 10;
    public float CurveWidth = 1f;
    public Vector3[] CurveNodes
    {
        get { return curveNodes; }
        set { }
    }

    Vector3[] curveNodes;
    Vector3[] tangents;

    void Update()
    {
        curveNodes = new Vector3[LineResolution + 1];
        tangents = new Vector3[LineResolution + 1];

        // Quadratic Bezier curve
        // 3 points: p0, p1, p2, starting and ending on p0 and p2 with p1 as control point.
        // (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
        // t between the range [0, 1]
        // Calculate tangents
        // B'(t) = 2(1-t)(P1-P0) + 2t(P2-P1)

        for (int i = 0; i <= LineResolution; ++i)
        {
            float t = i == 0 ? 0 : i / (float)LineResolution;
            Vector3 point = (((1 - t) * (1 - t)) * InputNodes[0].position) + ((2 * (1 - t) * t) * InputNodes[1].position) + (t * t * InputNodes[2].position);
            curveNodes[i] = point;

            tangents[i] = 2 * (1 - t) * (InputNodes[1].position - InputNodes[0].position) + 2 * t * (InputNodes[2].position - InputNodes[1].position);
        }

        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.SetVertexCount(curveNodes.Length);
        //for (int i = 0; i < curveNodes.Length; i++)
        //{
        //    lineRenderer.SetPosition(i, curveNodes[i]);
        //}

        for (int i = 0; i < curveNodes.Length - 1; ++i)
        {
            Debug.DrawLine(curveNodes[i], curveNodes[i + 1]);
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < curveNodes.Length; i++)
        {
            Gizmos.DrawWireCube(curveNodes[i], new Vector3(.1f, .1f, .1f));


            Vector3 cross = Vector3.Cross(Vector3.up, tangents[i]);
            cross = cross.normalized * CurveWidth / 2;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(curveNodes[i], cross);
            Gizmos.DrawRay(curveNodes[i], -cross);
            Gizmos.DrawSphere(curveNodes[i] + cross, .1f);
            Gizmos.DrawSphere(curveNodes[i] - cross, .1f);
        }
    }
}
