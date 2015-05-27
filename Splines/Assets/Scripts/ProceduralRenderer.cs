using UnityEngine;
using System.Collections;

public class ProceduralRenderer : MonoBehaviour
{
    MeshBuilder meshBuilder = new MeshBuilder();
    public float Length = 2f;
    public float Width = 2f;
    public float Height = 2f;
    public int SegmentCount = 10;
    public float TilingX = 1f;
    public float TilingY = 1f;

    public Material MeshMaterial;

	// Use this for initialization
	void Start()
    {
        //GenerateGridMesh();
        //GenerateBoxMesh();
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
            float v = (1.0f / SegmentCount * TilingY) * i;

            for (int j = 0; j <= SegmentCount; j++)
            {
                float x = Width * j;
                float u = (1.0f / SegmentCount * TilingX) * j;

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
        GetComponent<Renderer>().material = MeshMaterial;
    }

    void GenerateBoxMesh()
    {
        Vector3 upDir = Vector3.up * Height;
        Vector3 rightDir = Vector3.right * Width;
        Vector3 forwardDir = Vector3.forward * Length;

        Vector3 nearCorner = Vector3.zero;
        Vector3 farCorner = upDir + rightDir + forwardDir;

        BuildQuad(nearCorner, forwardDir, rightDir);
        BuildQuad(nearCorner, rightDir, upDir);
        BuildQuad(nearCorner, upDir, forwardDir);

        BuildQuad(farCorner, -rightDir, -forwardDir);
        BuildQuad(farCorner, -upDir, -rightDir);
        BuildQuad(farCorner, -forwardDir, -upDir);

        Mesh mesh = meshBuilder.CreateMesh();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
        GetComponent<Renderer>().material = MeshMaterial;
    }

    void BuildQuad(Vector3 offset, Vector3 widthDir, Vector3 lengthDir)
    {
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;
        meshBuilder.Vertices.Add(offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(normal);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }

    void BuildQuad(Vector3 offset)
    {
        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, Length) + offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(Width, 0.0f, Length) + offset);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(Width, 0.0f, 0.0f) + offset);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
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
        meshBuilder.UVs.Add(uv);

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
