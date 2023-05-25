using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointObject : MonoBehaviour
{
    [SerializeField] private bool isReady;
    public bool IsReady
    {
        get
        {
            return isReady;
        }
        set
        {
            isReady = value;
        }
    }
    private LineRenderer lineRenderer;

    [SerializeField] private List<TargetObject> invalidTarget;

    public void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        isReady = true;
    }

    public void SetAimInfo(float s, Color c, Vector3 t)
    {
        isReady = false;
        lineRenderer.enabled = true;
        //c = new Color(c.r, c.g, c.b, 0);

        lineRenderer.startWidth = s;
        lineRenderer.startColor = c;
        lineRenderer.SetPosition(0, this.transform.position);

        lineRenderer.endWidth = s;
        lineRenderer.endColor = c;        
        lineRenderer.SetPosition(1, t);

        Vector2 direction =  t - this.transform.position;
        this.transform.up = direction;
    }

    public void UpdateLaserAlpha(float a)
    {
        Color c = lineRenderer.startColor;
        c = new Color(c.r, c.g, c.b, a);
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;
    }

    public void SetShootInfo(float s, Color c)
    {
        lineRenderer.startWidth = s;
        //lineRenderer.startColor = new Color(c.r, c.g, c.b , 0);
        lineRenderer.startColor = c;
        
        lineRenderer.endWidth = s;
        lineRenderer.endColor = c;        
    }

    public void StopProgress()
    {
        lineRenderer.enabled = false;
        isReady = false;
    }

    public bool CheckValidTarget(TargetObject to)
    {
        return !invalidTarget.Contains(to);
    }
}
