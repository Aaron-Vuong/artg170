using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public bool grounded = true;
    public float jumpTimeout = 1f;

    private void OnCollisionEnter()
    {
        grounded = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        //grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        StartCoroutine(RefreshJump());
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }

    IEnumerator RefreshJump()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(jumpTimeout);
        grounded = true;
    }
}
