using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDPrototype
{
    public class LifeTime_script : MonoBehaviour
    {
        [SerializeField] float lifeTime;

        private void Update()
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
