using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 direction = new Vector2(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y);

            this.transform.up = direction;
        }
    }
}
