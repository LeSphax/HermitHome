using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelContent", menuName = "HermitHome/LevelContentAsset")]
public class LevelContentAsset: ScriptableObject {
    
    [System.Serializable]
    public struct WeightedContent {
        [SerializeField]
        public int weight;
        [SerializeField]
        public LevelContentAsset content;
    }

    [System.Serializable]
    public struct WeightedPrefab {
        [SerializeField]
        public int weight;
        [SerializeField]
        public GameObject content;
    }
    [SerializeField]
    public WeightedPrefab[] m_prefabs = null;
    [SerializeField]
    public WeightedContent[] m_content = null;

    public float m_uprightStrength = 0.0f;
    public float m_normalLip = 0.0f;

    public int Sum { get; private set; }

    public void Initialize() {
        Sum = 
            (m_content?.Select(o => o.weight).Sum() ?? 0) + 
            (m_prefabs?.Select(o => o.weight).Sum() ?? 0);

        if (m_content != null)
            foreach (var content in m_content)
                content.content.Initialize();
    }

    public virtual GameObject SpawnObject(Transform parent, Vector3 worldPosition, Vector3 worldNormal) {
        var selector = Random.Range(0, Sum);
        if (m_prefabs != null) {
            foreach (var weightedPrefab in m_prefabs) {
                selector -= weightedPrefab.weight;
                if (selector < 0) {
                    var worldRotation = Quaternion.FromToRotation(Vector3.up, Vector3.up * m_uprightStrength + worldNormal)
                        * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    return Instantiate(weightedPrefab.content, worldPosition - worldNormal * m_normalLip, worldRotation, parent);
                }
            }
        }
        if (m_content != null) {
            foreach (var weightedContent in m_content) {
                selector -= weightedContent.weight;
                if (selector < 0)
                    return weightedContent.content.SpawnObject(parent, worldPosition, worldNormal);
            }
        }
        return null;
    }
}
