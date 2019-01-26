using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class LevelChunk: MonoBehaviour {

    public Mesh m_mesh = null;

    public float Timestamp { get; set; } = 0f;

    public Vector2Int m_edgeCount;
    public float m_size = 10.0f;
    public float m_heightScalar = 5.0f;

    public Vector2 m_chunkOffset = new Vector2(10000f, 10000f);

    public int m_octaves = 8;
    public float m_frequency = 0.1f;
    public float m_lacunarity = 2.85f;
    public float m_persistance = 0.45f;

    public float m_scalarOctaves = 2;
    public float m_scalarFrequency = 0.001f;
    public float m_scalarLacunarity = 2.85f;
    public float m_scalarPersistance = 0.45f;

    private float GenerateHeight(Vector2 localPos) {
        float x = transform.position.x + m_chunkOffset.x + localPos.x;
        float y = transform.position.z + m_chunkOffset.y + localPos.y;

        float frequency = m_frequency;
        float amplitude = 1.0f;
        float result = 0.0f;
        
        for (int i = 0; i < m_octaves; i++) {
            frequency *= m_lacunarity;
            amplitude *= m_persistance;
            
            result += Simplex.Noise.Generate(x * frequency, y * frequency) * amplitude;
        }
        float scalar = m_heightScalar;
        frequency = m_scalarFrequency;
        amplitude = 1.0f;
        for (int i = 0; i < m_scalarOctaves; i++) {
            frequency *= m_scalarLacunarity;
            amplitude *= m_scalarPersistance;

            scalar += Simplex.Noise.Generate(x * frequency, y * frequency);
        }
        

        return result * scalar;

    }

    [ContextMenu("Generate Mesh!")]
    public void GenerateMesh() {
        m_mesh = new Mesh();
        
        var step = new Vector2(m_size / (m_edgeCount.x - 1), m_size / (m_edgeCount.y - 1));
        
        // For some reason, SetVertices only takes a list and not an array?
        var vertices = new List<Vector3>(m_edgeCount.x * m_edgeCount.y);
        for (int x = 0; x < m_edgeCount.x; x++) {
            for (int y = 0; y < m_edgeCount.y; y++) {
                vertices.Add(new Vector3(
                    step.x * x,
                    GenerateHeight(new Vector2(step.x * x, step.y * y)), 
                    step.y * y));
            }
        }

        var triangles = new int[(m_edgeCount.x - 1) * (m_edgeCount.y) * 6];
        int offset = 0;
        for (int x = 0; x < m_edgeCount.x - 1; x++) {
            for (int y = 0; y < m_edgeCount.y - 1; y++) {

                int firstRowBegin = x * m_edgeCount.y + y;
                int lastRowBegin = (x + 1) * m_edgeCount.y + y;

                triangles[offset++] = firstRowBegin + 0;
                triangles[offset++] = firstRowBegin + 1;
                triangles[offset++] = lastRowBegin + 0;

                triangles[offset++] = lastRowBegin + 1;
                triangles[offset++] = lastRowBegin + 0;
                triangles[offset++] = firstRowBegin + 1;
            }
        }
        
        m_mesh.SetVertices(vertices);
        m_mesh.SetTriangles(triangles, 0, true);
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateTangents();
        m_mesh.UploadMeshData(true);

        GetComponent<MeshFilter>().mesh = m_mesh;
        if (GetComponent<MeshCollider>() != null)
            GetComponent<MeshCollider>().sharedMesh = m_mesh;
    }
    
    public void GenerateContent(LevelContentAsset levelContent) {
        int contentNum = Mathf.RoundToInt(m_size * m_size * Random.Range(0.005f, 0.01f));
        for (int i = 0; i < contentNum; i++) {
            var localPos = new Vector2(
                Random.value * m_size,
                Random.value * m_size);
            var y = GenerateHeight(localPos);
            var levelObject = levelContent.InstantiateObject(
                transform, 
                transform.position + new Vector3(localPos.x, y, localPos.y), 
                Quaternion.Euler(0, Random.Range(0f, 360f), 0));
        }
    }
}