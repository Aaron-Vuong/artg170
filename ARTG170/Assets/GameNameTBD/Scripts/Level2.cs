using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;

    // TODO: Fix controller not being picked up properly?
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OTHER OBJECT TAGGED AS {other.tag} {other.name}");
        if (other.tag == "PlayerCollider")
        {
            _controller = other.transform.parent.gameObject.GetComponent<PlayerController>();
            Debug.Log("Found PlayerController for Level transition!");
            _controller.loadLevel2();

        }
    }
}
