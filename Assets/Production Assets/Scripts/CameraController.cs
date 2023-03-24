using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cam shake: https://gist.github.com/ftvs/5822103
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    [SerializeField] private Transform camTransform;

    // How long the object should shake for.
    [SerializeField] private float shakeDuration = 0f;
    [SerializeField] private float startDuration;

    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] private float shakeAmount = 0.7f;

    private Vector3 originalPos;

    void Awake()
    {

    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && shakeDuration == 0) //for testing
        {
            StartShake();
        }

        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void StartShake()
    {
        shakeDuration = startDuration;
    }
}
