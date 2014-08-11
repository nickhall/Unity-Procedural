using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode()]
public class CurveImplementation : MonoBehaviour
{

    //public GameObject[] Points = new GameObject[4];
    public List<GameObject> Points = new List<GameObject>();
    public int CurveResolution = 10;
    public Vector3[] CurveCoordinates;
    public Vector3[] Tangents;
    public bool ClosedLoop = false;

	void Start()
    {
	
	}

    void Update()
    {
        Vector3 p0;
        Vector3 p1;
        Vector3 m0;
        Vector3 m1;
        int pointsToMake;
        //Debug.Log("This is a test " + Points[0]);

        if (ClosedLoop == true)
        {
            pointsToMake = (CurveResolution) * (Points.Count);
        }
        else
        {
            pointsToMake = (CurveResolution) * (Points.Count - 1);
        }

        CurveCoordinates = new Vector3[pointsToMake];
        Tangents = new Vector3[pointsToMake];

        int closedAdjustment = ClosedLoop ? 0 : 1;

        // First for loop goes through each individual control point and connects it to the next, so 0-1, 1-2, 2-3 and so on
        for (int i = 0; i < Points.Count - closedAdjustment; i++)
        {
            //if (Points[i] == null || Points[i + 1] == null || (i > 0 && Points[i - 1] == null) || (i < Points.Count - 2 && Points[i + 2] == null))
            //{
            //    return;
            //}

            p0 = Points[i].transform.position;
            p1 = (ClosedLoop == true && i == Points.Count - 1) ? Points[0].transform.position : Points[i + 1].transform.position;

            // Tangent calculation for each control point
            // Tangent M[k] = (P[k+1] - P[k-1]) / 2
            // With [] indicating subscript

            // m0
            if (i == 0)
            {
                m0 = ClosedLoop ? 0.5f * (p1 - Points[Points.Count - 1].transform.position) : p1 - p0;
            }
            else
            {
                m0 = 0.5f * (p1 - Points[i - 1].transform.position);
            }

            // m1
            if (ClosedLoop)
            {
                if (i == Points.Count - 1)
                {
                    m1 = 0.5f * (Points[(i + 2) % Points.Count].transform.position - p0);
                }
                else if (i == 0)
                {
                    m1 = 0.5f * (Points[i + 2].transform.position - p0);
                }
                else
                {
                    m1 = 0.5f * (Points[(i + 2) % Points.Count].transform.position - p0);
                }
            }
            else
            {
                if (i < Points.Count - 2)
                {
                    m1 = 0.5f * (Points[(i + 2) % Points.Count].transform.position - p0);
                }
                else
                {
                    m1 = p1 - p0;
                }
            }

            Vector3 position;
            float t;
            float pointStep = 1.0f / CurveResolution;

            if ((i == Points.Count - 2 && ClosedLoop == false) || (i == Points.Count - 1 && ClosedLoop))
            {
                pointStep = 1.0f / (CurveResolution - 1);
                // last point of last segment should reach p1
            }
            // Second for loop actually creates the spline for this particular segment
            for (int j = 0; j < CurveResolution; j++)
            {
                t = j * pointStep;
                Vector3 tangent;
                position = CatmullRom.Interpolate(p0, p1, m0, m1, t, out tangent);
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
