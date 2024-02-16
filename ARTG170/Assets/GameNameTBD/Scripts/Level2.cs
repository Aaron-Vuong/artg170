using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        SceneChangeManager.Load(SceneChangeManager.Scene.Level2);
    }
}
