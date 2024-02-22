using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepsSound;
    [SerializeField] private PlayerController _controller;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && _controller.grounded)
        {
            footstepsSound.enabled = true;
        }
        else
        {
            footstepsSound.enabled = false;
            
        }
    }
}