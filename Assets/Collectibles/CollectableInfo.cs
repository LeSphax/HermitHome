using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableInfo : MonoBehaviour
{
    public string m_infoText = "";

    private void Start() {
        Debug.Log("New " + gameObject.name + "!");
    }
}
