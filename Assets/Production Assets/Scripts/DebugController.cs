using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField] private bool infiLife;
    public static bool infiniteLife;

    private void Awake()
    {
        infiniteLife = infiLife;
    }
}
