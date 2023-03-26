using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void FixedUpdate()
    {
        if (allowMove)
        {
            RotateBlocker();
        }
    }

    public void Init(DifficultyDataClass data)
    {
        blockerObject.transform.localScale = data.BlockerScale;
        blockerObject.color = data.BlockerColor;
    }

    private void RotateBlocker() //rotatet toward mouse
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
        allowMove = b;
        blockerPivot.gameObject.SetActive(b);
    }
}
