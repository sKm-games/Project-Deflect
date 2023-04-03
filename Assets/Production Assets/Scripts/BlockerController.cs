using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockerController : MonoBehaviour
{
    [SerializeField] private bool allowMove;
    public bool AllowMove
    {
        set
        {
            allowMove = value;
        }
    }

    [SerializeField] private Transform blockerPivot;
    [SerializeField] private float rotationLimit;
       

    [SerializeField] private SpriteRenderer blockerObject;


    [SerializeField] private Slider senseSliderPause;
    private TextMeshProUGUI sensSliderPauseText;
    [SerializeField] private Slider senseSliderSettings;
    private TextMeshProUGUI sensSliderSettingsText;
    [SerializeField] private float sensetivity;

    public float Sensetivity
    {
        get
        {
            return sensetivity;
        }
        set
        {
            sensetivity = value;
        }
    }
    [SerializeField] private float androidOffset;
    [SerializeField] private float editorOffset;
    private float offset;

    private void Awake()
    {
        sensSliderPauseText = senseSliderPause.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        sensSliderSettingsText = senseSliderSettings.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
#if UNITY_ANDROID
        offset = androidOffset;
#endif
#if UNITY_EDITOR
        offset = editorOffset;
#endif
    }

    private void FixedUpdate()
    {
        if (!allowMove)
        {
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            AddativeRotation();
        }

        /*if (allowMove)
        {
            DirectRotateBlocker();
        }*/
    }

    public void Init(DifficultyDataClass data)
    {
        blockerObject.transform.localScale = data.BlockerScale;
        blockerObject.color = data.BlockerColor;
    }

    private void AddativeRotation()
    {
        float mouse = Input.GetAxis("Mouse X");        
        float z = (-mouse * offset) * sensetivity;

        if ((blockerPivot.rotation.z <= -rotationLimit && z < 0) || (blockerPivot.rotation.z >= rotationLimit && z > 0))
        {
            return;
        }
        blockerPivot.Rotate(new Vector3(0, 0, z));
    }

    private void DirectRotateBlocker() //rotatet toward mouse
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = new Vector2(mousePos.x - blockerPivot.position.x, mousePos.y - blockerPivot.position.y);

        if (direction.x > -rotationLimit && direction.x < rotationLimit)
        {
            blockerPivot.up = direction;
        }        
    }

    public void ToggleBlocker(bool b)
    {
        blockerPivot.rotation = Quaternion.Euler(0, 0, 0);
        allowMove = b;
        blockerPivot.gameObject.SetActive(b);
    }

    public void UpdateSensetivity(Slider s)
    {
        sensetivity = (float)decimal.Round((decimal)s.value, 1);
        senseSliderPause.SetValueWithoutNotify(sensetivity);
        sensSliderPauseText.text = $"Sensetivity: {sensetivity}";
        senseSliderSettings.SetValueWithoutNotify(sensetivity);
        sensSliderSettingsText.text = $"Sensetivity: {sensetivity}";
    }

    public void SetSaveInfo(float s)
    {
        sensetivity = s;
        senseSliderPause.SetValueWithoutNotify(sensetivity);
        sensSliderPauseText.text = $"Sensetivity: {sensetivity}";
        senseSliderSettings.SetValueWithoutNotify(sensetivity);
        sensSliderSettingsText.text = $"Sensetivity: {sensetivity}";
    }

}
