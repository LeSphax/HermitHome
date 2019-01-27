using UnityEngine;
using System.Collections;

public class Collector: MonoBehaviour {

    [SerializeField]
    private GameObject m_placementTarget = null;

    void OnTriggerEnter(Collider other) {
        var collectible = other.GetComponentInParent<CollectibleInfo>();
        Debug.Log(collectible?.name ?? "No collectible :(");
        if (collectible == null)
            return;

        Collect(collectible);
    }
    void Collect(CollectibleInfo collectible) {
        var placementCollider = m_placementTarget.GetComponentInChildren<Collider>();
        Vector3 dir = Vector3.zero;

        while (dir.y < 0.5)
        {
            dir = Random.onUnitSphere;
        }
        

        Vector3 direction = placementCollider.transform.position - placementCollider.transform.position + dir * 10f;
        var ray2 = new Ray(placementCollider.transform.position + dir * 10f, -direction);

        RaycastHit hitinfo;

        if (!Physics.Raycast(ray2, out hitinfo, Mathf.Infinity, 1 << 9))
        {
            Debug.LogError("Don't knwo where to put the collectible");
            return;
        }

        var rigidBody = collectible.GetComponent<Rigidbody>();
        if (rigidBody != null) {
            Destroy(rigidBody);
        }

        var colTransf = collectible.transform;
        colTransf.parent = placementCollider.transform;
        colTransf.position = hitinfo.point;
        colTransf.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal) *
            Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        TopScreenText.SetText(collectible.m_infoText);
    }
}
