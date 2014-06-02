using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class Line : MonoBehaviour
{
    public Transform[] InputNodes;
    public int LineResolution = 10;
    public float CurveWidth = 1f;
    public Material RoadMaterial;
    public Vector3[] CurveNodes
    {
        get { return curveNodes; }
        set { }
    }

    Vector3[] curveNodes;
    Vector3[] tangents;
    Vector3[] vertices;
    Vector3[] normals;

    ProceduralMesh meshBuilder = new ProceduralMesh();

    void Awake()
    {
        curveNodes = new Vector3[LineResolution + 1];
        tangents = new Vector3[LineResolution + 1];
        vertices = new Vector3[(LineResolution + 1) * 2];
        normals = new Vector3[LineResolution + 1];

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

        BuildRoadMesh();
    }

    void Update()
    {
        for (int i = 0; i < curveNodes.Length - 1; ++i)
        {
            Debug.DrawLine(curveNodes[i], curveNodes[i + 1]);

            //int baseIndex = i * 2;
            //Debug.DrawLine(vertices[baseIndex], vertices[baseIndex + 3], Color.black);
            //Debug.Log("Drew line from " + vertices[baseIndex].ToString() + " to " + vertices[baseIndex + 1].ToString());
        }
    
    }

    void BuildRoadMesh()
    {

        // Get vertices for road edges

        for (int i = 0; i < curveNodes.Length; i++)
        {
            Vector3 cross = Vector3.Cross(Vector3.up, tangents[i]);
            cross = cross.normalized * CurveWidth / 2;
            vertices[i * 2] = curveNodes[i] + cross;
            vertices[i * 2 + 1] = curveNodes[i] - cross;

            //Vector2 uv1;
            //Vector2 uv2;
            //if (i != 0)
            //{
            //    uv1 = new Vector2(0, (float)(curveNodes.Length) / (float)i);
            //    uv2 = new Vector2(1, (float)(curveNodes.Length) / (float)i);
            //}
            //else
            //{
            //    uv1 = new Vector2(0, 0);
            //    uv2 = new Vector2(1, 0);
            //}

            //meshBuilder.UVs.Add(uv1);
            //meshBuilder.UVs.Add(uv2);
        }
        meshBuilder.Vertices.AddRange(vertices);

        // Build triangle list
        for (int i = 0; i < LineResolution - 1; i++)
        {
            int baseIndex = i * 2;
            meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);

            meshBuilder.AddTriangle(baseIndex + 1, baseIndex + 3, baseIndex + 2);
            Debug.Log("BI: " + baseIndex);
        }
        Mesh mesh = meshBuilder.CreateMesh();
        mesh.RecalculateNormals();
        mesh.name = "MESH";
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
        renderer.material = RoadMaterial;

        Debug.Log("Built mesh 1");

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < curveNodes.Length; i++)
        {
            Gizmos.DrawWireCube(curveNodes[i], new Vector3(.1f, .1f, .1f));
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < curveNodes.Length; i++)
		{
            Gizmos.DrawRay(curveNodes[i], vertices[i * 2] - curveNodes[i]);
            Gizmos.DrawRay(curveNodes[i], vertices[i * 2 + 1] - curveNodes[i]);
            Gizmos.DrawSphere(vertices[i * 2], 0.1f);
            Gizmos.DrawSphere(vertices[i * 2 + 1], 0.1f);
		}


            //vector3 cross = vector3.cross(vector3.up, tangents[i]);
            //cross = cross.normalized * curvewidth / 2;
            //gizmos.color = color.red;
            //gizmos.drawray(curvenodes[i], cross);
            //gizmos.drawray(curvenodes[i], -cross);
            //gizmos.drawsphere(curvenodes[i] + cross, .1f);
            //gizmos.drawsphere(curvenodes[i] - cross, .1f);
    }
}
