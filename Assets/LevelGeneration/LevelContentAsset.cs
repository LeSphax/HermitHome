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

    public int Sum { get; private set; }

    public void Initialize() {
        Sum = 
            (m_content?.Select(o => o.weight).Sum() ?? 0) + 
            (m_prefabs?.Select(o => o.weight).Sum() ?? 0);

        if (m_content != null)
            foreach (var content in m_content)
                content.content.Initialize();
    }

    public GameObject InstantiateObject(Transform parent, Vector3 worldPosition, Quaternion worldRotation) {
        var selector = Random.Range(0, Sum);
        if (m_prefabs != null) {
            foreach (var weightedPrefab in m_prefabs) {
                selector -= weightedPrefab.weight;
                if (selector <= 0)
                    return Instantiate(weightedPrefab.content, worldPosition, worldRotation, parent);
            }
        }
        if (m_content != null) {
            foreach (var weightedContent in m_content) {
                selector -= weightedContent.weight;
                if (selector <= 0)
                    return weightedContent.content.InstantiateObject(parent, worldPosition, worldRotation);
            }
        }
        return null;
    }
}
