using UnityEngine;
using System.Collections;

public class Collector: MonoBehaviour {

    [SerializeField]
    private GameObject m_placementTarget = null;

    void OnTriggerEnter(Collider other) {
        var collectible = other.GetComponentInParent<CollectibleInfo>();
        if (collectible == null)
            return;

        Collect(collectible);
    }

    void Collect(CollectibleInfo collectible) {
        var placementCollider = m_placementTarget.GetComponentInChildren<Collider>();
        var dir = Random.onUnitSphere;
        if (dir.y < 0)
            dir.y = -dir.y;
        var ray = new Ray(placementCollider.transform.position - dir * 10.0f, dir);
        RaycastHit hitinfo;
        if (!placementCollider.Raycast(ray, out hitinfo, 10.0f))
            return;

        var rigidBody = collectible.GetComponent<Rigidbody>();
        if (rigidBody != null) {
            Destroy(rigidBody);
        }

        var colTransf = collectible.transform;
        colTransf.parent = placementCollider.transform;
        colTransf.position = hitinfo.point;
        colTransf.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal) *
            Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }
}
