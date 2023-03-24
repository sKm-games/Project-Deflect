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

    private void Start()
    {

    }

    void Update()
    {
        if (allowMove)
        {
            RotateBlocker();
        }
    }

    void RotateBlocker() //rotatet toward mouse
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
