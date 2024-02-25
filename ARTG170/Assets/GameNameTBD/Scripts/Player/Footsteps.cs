using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepsSound;
   // public List<AudioClip> footsteps;
    [SerializeField] private PlayerController _controller;

    void Update()
    {
        //AudioClip clip= footsteps[Random.Range(0, footsteps.Count)];
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && _controller.grounded)
        {
            //footstepsSound.clip = clip;
            footstepsSound.enabled = true;
            
        }
        else
        {
            footstepsSound.enabled = false;
            
        }
    }
}