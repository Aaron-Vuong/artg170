using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    public float masterVolume;
    public float musicVolume;
    public float effectVolume;
    public float cursorSize;
    public Color cursorColor;
    public bool isFullScreen;
}
