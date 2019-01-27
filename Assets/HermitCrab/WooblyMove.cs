using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WooblyMove : MonoBehaviour
{
    public float speed = 1;
    public float distance = 0.1f;

    private Vector3 localRelativePos;

    private void Start() {
        localRelativePos = transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = localRelativePos + Vector3.up * distance * Simplex.Noise.Generate(Time.time * speed);
    }
}
