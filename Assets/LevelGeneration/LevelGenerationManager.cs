using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerationManager: MonoBehaviour {

    private void Start() {
        Initialize();
        OnTargetChunkChanged();
        StartCoroutine(ChunkLoader());
    }

    void Initialize() {
        ClearLevel();

        var prefabChunk = m_chunkPrefab.GetComponent<LevelChunk>();
        m_chunkSize = prefabChunk.m_size;
        m_levelContent?.Initialize();
        if (m_useRandomSeed)
            m_seed = Random.Range(int.MinValue, int.MaxValue);
        Simplex.Noise.Seed = m_seed;
    }

    // Update is called once per frame
    void Update() {
        if (m_target == null)
            return;

        var targetPos = m_target.transform.position;
        TargetChunk = new Vector2Int(
            Mathf.RoundToInt(targetPos.x / m_chunkSize),
            Mathf.RoundToInt(targetPos.z / m_chunkSize));
    }

    Queue<Vector2Int> ChunkLoadQueue { get; } = new Queue<Vector2Int>();
    public int m_maxLoadedChunks = 32;
    public int m_chunkDistance = 3;
    IEnumerator ChunkLoader() {
        var eof = new WaitForEndOfFrame();
        while (gameObject.activeInHierarchy) {
            while (ChunkLoadQueue.Count > 0) {
                var chunk = ChunkLoadQueue.Dequeue();
                if (!Chunks.ContainsKey(chunk)) {
                    GenerateChunk(chunk);
                    break;
                } else {
                    Chunks[chunk].Timestamp = Time.time;
                }
            }

            if (Chunks.Count > m_maxLoadedChunks) {
                foreach (var chunk in Chunks
                    .OrderBy(pair => pair.Value.Timestamp)
                    .Take(Chunks.Count - m_maxLoadedChunks)
                    .ToArray()) {
                    Destroy(chunk.Value.gameObject);
                    Chunks.Remove(chunk.Key);
                }
            }

            yield return eof;
        }
    }

    
    
    void OnTargetChunkChanged() {
        Vector2Int chunk = Vector2Int.zero;
        ChunkLoadQueue.Enqueue(chunk);
        for (chunk.x = TargetChunk.x - m_chunkDistance; chunk.x <= TargetChunk.x + m_chunkDistance; chunk.x++)
            for (chunk.y = TargetChunk.y - m_chunkDistance; chunk.y <= TargetChunk.y + m_chunkDistance; chunk.y++)
                ChunkLoadQueue.Enqueue(chunk);
    }

    [SerializeField]
    private GameObject m_target = null;
    private Vector2Int m_targetChunk = new Vector2Int();
    public Vector2Int TargetChunk {
        get { return m_targetChunk; }
        set {
            if (m_targetChunk == value)
                return;

            m_targetChunk = value;
            OnTargetChunkChanged();
        }
    }

    [SerializeField]
    private GameObject m_chunkPrefab = null;
    private float m_chunkSize;
    [SerializeField]
    private LevelContentAsset m_levelContent = null;

    [SerializeField]
    private bool m_useRandomSeed = true;
    [SerializeField]
    private int m_seed = 0;
    public Dictionary<Vector2Int, LevelChunk> Chunks { get; } = new Dictionary<Vector2Int, LevelChunk>();

    [ContextMenu("Generate a small level!")]
    private void GenerateSmallLevel() {
        Initialize();
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
        if (m_levelContent != null)
            levelChunk.GenerateContent(m_levelContent);
        levelChunk.Timestamp = Time.time;
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
