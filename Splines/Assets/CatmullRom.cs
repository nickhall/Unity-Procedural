using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class CatmullRom : MonoBehaviour
{
    public GameObject[] Points = new GameObject[4];
    public int CurveResolution = 20;
    public Vector3[] CurveCoordinates;
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

        if (ClosedLoop == true)
        {
            pointsToMake = (CurveResolution) * (Points.Length);
        }
        else
        {
            pointsToMake = (CurveResolution) * (Points.Length - 1);
        }

        CurveCoordinates = new Vector3[pointsToMake];

        for (int i = 0; i < Points.Length - 1; i++)
        {
            if (Points[i] == null || Points[i + 1] == null || (i > 0 && Points[i - 1] == null) || (i < Points.Length - 2 && Points[i + 2] == null))
            {
                return;
            }

            p0 = Points[i].transform.position;
            p1 = Points[i + 1].transform.position;
            if (i > 0)
            {
                m0 = 0.5f * (p1 - Points[i - 1].transform.position);
            }
            else
            {
                //m0 = p1 - p0;
                m0 = 0.5f * (p1 - Points[Points.Length - 1].transform.position);
            }
            if (i < Points.Length - 2)
            {
                m1 = 0.5f * (Points[i + 2].transform.position - p0);
            }
            else
            {
                m1 = p1 - p0;
            }

            Vector3 position;
            float t;
            float pointStep = 1.0f / CurveResolution;

            if (i == Points.Length - 2)
            {
                pointStep = 1.0f / (CurveResolution - 1);
                // last point of last segment should reach p1
            }  

            for (int j = 0; j < CurveResolution; j++)
            {
                // Catmull-Rom splines are Hermite curves with special tangent values.
                // Hermite curve formula:
                // (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
                // For points p0 and p1 passing through points m0 and m1 interpolated over t = [0, 1]
                // Tangent M[k] = (P[k+1] - P[k-1]) / 2
                // With [] indicating subscript
                t = j * pointStep;
                position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
                    + (t * t * t - 2.0f * t * t + t) * m0
                    + (-2.0f * t * t * t + 3.0f * t * t) * p1
                    + (t * t * t - t * t) * m1;
                CurveCoordinates[i * CurveResolution + j] = position;
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
