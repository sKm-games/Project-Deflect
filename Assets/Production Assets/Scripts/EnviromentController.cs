using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class EnviromentController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundRendere;
    [SerializeField] private SpriteRenderer groundRendere;
    [SerializeField] private SpriteRenderer moonRendere;

    [SerializeField] private Light2D mainLight;

    public void UpdateEnviroment(DifficultyDataClass data)
    {
        backgroundRendere.sprite = data.BackgroundSprite;
        groundRendere.sprite = data.GroundSprite;
        moonRendere.sprite = data.MoonSprite;
    }


}
