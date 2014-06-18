using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class CatmullRom : MonoBehaviour
{
    public GameObject[] Points = new GameObject[4];
    public int CurveResolution = 20;
    public Vector3[] CurveCoordinates;

	void Start()
    {
	
	}
	
	void Update()
    {
        Vector3 p0;
        Vector3 p1;
        Vector3 m0;
        Vector3 m1;

        CurveCoordinates = new Vector3[CurveResolution * (Points.Length - 1) + 1];

        for (int i = 0; i < Points.Length - 1; i++)
        {
            p0 = Points[i].transform.position;
            p1 = Points[i + 1].transform.position;
            if (i > 0)
            {
                m0 = 0.5f * (p1 - Points[i - 1].transform.position);
            }
            else
            {
                m0 = p1 - p0;
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

            for (int j = 0; j < CurveResolution; j++)
            {
                t = j * pointStep;
                position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
                    + (t * t * t - 2.0f * t * t + t) * m0
                    + (-2.0f * t * t * t + 3.0f * t * t) * p1
                    + (t * t * t - t * t) * m1;
                CurveCoordinates[j + i * Points.Length] = position;
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
