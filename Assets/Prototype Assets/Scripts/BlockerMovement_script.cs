using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDPrototype
{
    public class BlockerMovement_script : MonoBehaviour
    {
        [SerializeField] private bool allowMove;
        public bool AllowMove
        {
            set
            {
                allowMove = value;
            }
        }

        private GameObject blocker;

        private void Start()
        {
            blocker = this.transform.GetChild(0).gameObject;
            ToggleBlocker(false);
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
            Vector2 direction = new Vector2(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y);

            transform.up = direction;
        }

        public void ToggleBlocker(bool b)
        {
            allowMove = b;
            blocker.SetActive(b);
        }
    }
}