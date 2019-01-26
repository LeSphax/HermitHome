using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerationManager: MonoBehaviour {
    
    void Start() {
        ClearLevel();
        if (m_useRandomSeed)
            m_seed = Random.Range(int.MinValue, int.MaxValue);
        Simplex.Noise.Seed = m_seed;
    }

    // Update is called once per frame
    void Update() {

    }

    [SerializeField]
    private GameObject m_chunkPrefab = null;
    [SerializeField]
    private bool m_useRandomSeed = true;
    [SerializeField]
    private int m_seed = 0;
    public Dictionary<Vector2Int, LevelChunk> Chunks { get; } = new Dictionary<Vector2Int, LevelChunk>();

    [ContextMenu("Generate a small level!")]
    private void GenerateSmallLevel() {
        Start();
        const int levelSize = 5;
        for (int i = -levelSize / 2; i <= levelSize / 2; i++) {
            for (int j = -levelSize / 2; j <= levelSize / 2; j++) {
                GenerateChunk(new Vector2Int(i, j));
            }
        }
    }

    private void GenerateChunk(Vector2Int chunk) {
        if (Chunks.ContainsKey(chunk))
            Destroy(Chunks[chunk].gameObject);

        var prefabChunk = m_chunkPrefab.GetComponent<LevelChunk>();
        var levelChunkObject = Instantiate(m_chunkPrefab,
            new Vector3(
                chunk.x * prefabChunk.m_size - (prefabChunk.m_size / 2),
                0f,
                chunk.y * prefabChunk.m_size - (prefabChunk.m_size / 2)), 
            Quaternion.identity,
            transform);

        var levelChunk = levelChunkObject.GetComponent<LevelChunk>();
        levelChunk.GenerateMesh();
        Chunks.Add(chunk, levelChunk);
    }

    [ContextMenu("Clear level")]
    private void ClearLevel() {

#if UNITY_EDITOR
        for (var childIndex = transform.childCount - 1; childIndex >= 0; childIndex--)
            DestroyImmediate(transform.GetChild(childIndex).gameObject);
#else
        foreach (var chunk in Chunks.Values)
            Destroy(chunk);
#endif

        Chunks.Clear();
    }
}
