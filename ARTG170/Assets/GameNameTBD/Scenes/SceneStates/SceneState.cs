using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneState : ScriptableObject
{
    public SceneChangeManager.Scene scene;
    public Dictionary<Transform, GameObject> changeableEntities;
}
