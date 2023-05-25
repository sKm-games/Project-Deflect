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

    private float sensitivity;
    [SerializeField] private float androidOffset;
    [SerializeField] private float editorOffset;
    private float offset;

    [SerializeField] private bool horMove;

    private void Awake()
    {
#if UNITY_ANDROID
        offset = androidOffset;
#endif
#if UNITY_EDITOR
        offset = editorOffset;
#endif
    }

    private void FixedUpdate()
    {
        if (!allowMove && ReferencesController.GetGameController.GameIsRunning)
        {
            return;
        }

        if (!horMove)
        {

            if (Input.GetButton("Fire1"))
            {
                AddativeRotation();
            }

            CheckValidRotation();

            /*if (allowMove)
        {
            DirectRotateBlocker();
        }*/

        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                HorizontalMove();
            }

            CheckHorizontalPos();
        }
    }

    public void Init(DifficultyDataClass data)
    {
        blockerObject.transform.localScale = data.BlockerScale;
        blockerObject.color = data.BlockerColor;
    }

    private void AddativeRotation()
    {
        float mouse = Input.GetAxis("Mouse X");        
        float z = (-mouse * offset) * sensitivity;        
        blockerPivot.Rotate(new Vector3(0, 0, z));        
    }

    private void CheckValidRotation()
    {
        if (blockerPivot.rotation.z <= -rotationLimit) //reset to maximum roation
        {            
            Quaternion q = blockerPivot.rotation;
            blockerPivot.rotation = new Quaternion(q.x, q.y, -rotationLimit, q.w);
            return;
        }
        else if (blockerPivot.rotation.z >= rotationLimit) //reset to maximum rotation
        {         
            Quaternion q = blockerPivot.rotation;
            blockerPivot.rotation = new Quaternion(q.x, q.y, rotationLimit, q.w);
        }
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

    private void HorizontalMove()
    {
        Vector3 newPos = blockerPivot.transform.localPosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float diff = mousePos.x - newPos.x;
        float newX = (newPos.x + diff) * sensitivity;
        
        newPos = new Vector3(newX, newPos.y, newPos.z);

        blockerPivot.transform.localPosition = newPos;
    }

    private void CheckHorizontalPos()
    {
        Vector3 newPos = blockerPivot.transform.localPosition;
        if (newPos.x < -2.5f)
        {

            blockerPivot.transform.localPosition = new Vector3(-2.5f, newPos.y, newPos.z);
        }
        else if (newPos.x > 2.5f)
        {
            blockerPivot.transform.localPosition = new Vector3(2.5f, newPos.y, newPos.z);
        }
    }

    public void ToggleBlocker(bool b)
    {
        blockerPivot.rotation = Quaternion.Euler(0, 0, 0);
        blockerPivot.transform.localPosition = new Vector3(0, blockerPivot.transform.localPosition.y, 0);
        allowMove = b;
        blockerPivot.gameObject.SetActive(b);
    }

    public void UpdateSensitivity(float s)
    {
        sensitivity = s;        
    }
}
