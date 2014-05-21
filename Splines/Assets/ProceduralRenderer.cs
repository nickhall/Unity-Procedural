using UnityEngine;
using System.Collections;

public class ProceduralRenderer : MonoBehaviour
{
    ProceduralMesh meshBuilder = new ProceduralMesh();
    public float Length = 2f;
    public float Width = 2f;
    public float Height = 2f;
    public int SegmentCount = 10;

	// Use this for initialization
	void Start()
    {
        GenerateGridMesh();
	}
	
	// Update is called once per frame
	void Update()
    {

	}

    void GenerateQuadMesh()
    {
        for (int i = 0; i < SegmentCount; i++)
        {
            float z = Length * i;

            for (int j = 0; j < SegmentCount; j++)
            {
                float x = Width * j;
                Vector3 offset = new Vector3(x, Random.Range(0f, Height), z);

                BuildQuad(offset);
            }
        }
    }

    void GenerateGridMesh()
    {
        for (int i = 0; i <= SegmentCount; i++)
        {
            float z = Length * i;
            float v = (1.0f / SegmentCount) * i;

            for (int j = 0; j <= SegmentCount; j++)
            {
                float x = Width * j;
                float u = (1.0f / SegmentCount) * j;

                Vector3 offset = new Vector3(x, Random.Range(0f, Height), z);

                Vector2 uv = new Vector2(u, v);
                bool buildTriangles = i > 0 && j > 0;

                BuildQuadForGrid(offset, uv, buildTriangles, SegmentCount + 1);
            }
        }

        Mesh mesh = meshBuilder.CreateMesh();
        mesh.RecalculateNormals();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
    }

    void BuildQuad(Vector3 offset)
    {
        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
        meshBuilder.UV.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, Length) + offset);
        meshBuilder.UV.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(Width, 0.0f, Length) + offset);
        meshBuilder.UV.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(Width, 0.0f, 0.0f) + offset);
        meshBuilder.UV.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);

        //Create the mesh:
        MeshFilter filter = GetComponent<MeshFilter>();

        if (filter != null)
        {
            filter.sharedMesh = meshBuilder.CreateMesh();
        }
    }

    void BuildQuadForGrid(Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow)
    {
        meshBuilder.Vertices.Add(position);
        meshBuilder.UV.Add(uv);

        if (buildTriangles)
        {
            int baseIndex = meshBuilder.Vertices.Count - 1;
            int index0 = baseIndex;
            int index1 = baseIndex - 1;
            int index2 = baseIndex - vertsPerRow;
            int index3 = baseIndex - vertsPerRow - 1;

            meshBuilder.AddTriangle(index0, index2, index1);
            meshBuilder.AddTriangle(index2, index3, index1);
        }
    }
}
