using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionCollider : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;
    [SerializeField] public SceneChangeManager.Scene target_scene;

    // TODO: Fix controller not being picked up properly?
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OTHER OBJECT TAGGED AS {other.tag} {other.name}");
        if (other.tag == "PlayerCollider")
        {
            _controller = other.transform.parent.gameObject.GetComponent<PlayerController>();
            Debug.Log("Found PlayerController for Level transition!");
            if (target_scene == SceneChangeManager.Scene.Level2)
            {
                _controller.loadLevel2();
            }
            else if (target_scene == SceneChangeManager.Scene.Level3)
            {
                _controller.loadLevel3();
            }

        }
    }
}
