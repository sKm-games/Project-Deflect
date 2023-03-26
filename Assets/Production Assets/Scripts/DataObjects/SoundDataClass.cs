using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataObject", menuName = "ScriptableObjects/Sounds", order = 1)]
public class SoundDataClass : ScriptableObject
{
    public string ID;
    public AudioClip Clip;
    public Vector2 pitchRange;
    public float maxVolume;
    public bool loop;
}
