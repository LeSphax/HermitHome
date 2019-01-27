using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMovement : MonoBehaviour
{
    public float m_force = 5000f;
    public float m_steeringForce = 500f;

    public Collider m_groundedCollider = null;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);

        if (!IsGrounded.isGrounded)
            return;

        HandleForward();
        HandleSteering();
    }

    private void HandleForward()
    {
        var forwardInput =
            // note: model is backwards.
            -Vector3.forward * Input.GetAxis("Vertical");

        if (forwardInput.sqrMagnitude < 0.00001f)
            return;

        forwardInput += Vector3.up * 0.25f;
        if (forwardInput.sqrMagnitude > 1)
            forwardInput = forwardInput.normalized;

        forwardInput *= m_force * Time.deltaTime;
        GetComponent<Rigidbody>().AddRelativeForce(forwardInput, ForceMode.Force);
    }

    private void HandleSteering()
    {
        var steeringInput = Input.GetAxis("Horizontal");

        if (Mathf.Approximately(steeringInput, 0))
            return;

        steeringInput *= m_steeringForce * Time.deltaTime;
        //Debug.Log(Vector3.up * steeringInput);
        GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * steeringInput, ForceMode.Force);
        // Input.GetAxis("Horizontal"):
    }
}
