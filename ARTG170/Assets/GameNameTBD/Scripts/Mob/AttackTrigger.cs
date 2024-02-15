using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private float explosionRadius = 20f;
    private float explosionForce = 500f;

    [SerializeField] private bool canAttack = true;
    [SerializeField] private int cooldown = 3;

    private void OnTriggerEnter(Collider other)
    {
        Attack(other);
    }
    private void OnTriggerStay(Collider other)
    {
        Attack(other);
    }

    private void Attack(Collider other)
    {
        if (other.gameObject.tag == "PlayerCollider" && canAttack)
        {
            other.gameObject.transform.parent.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
            other.gameObject.transform.parent.GetComponent<PlayerController>().SubtractHealth();
            // Start cooling down for the next attack.
            Debug.Log("ATTACKED!");
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
