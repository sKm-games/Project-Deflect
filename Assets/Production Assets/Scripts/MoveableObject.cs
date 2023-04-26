using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] private float moveTime;
    [SerializeField] private float startPos;
    [SerializeField] private float endPos;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        spriteRend = this.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        FirstMove();
    }

    private void FirstMove()
    {
        float d = startPos - endPos;
        float c = this.transform.position.x - endPos;
        float t = (c / d) * moveTime;
        this.transform.DOLocalMoveX(endPos, t).SetEase(Ease.Linear).OnComplete(()=> NewMove());        
    }

    private void NewMove()
    {
        this.transform.localPosition = new Vector3(startPos, this.transform.localPosition.y);
        this.transform.DOLocalMoveX(endPos, moveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).OnStart(() => CheckFlip());
    }

    private void CheckFlip()
    {
        Debug.Log("Check Flip");
        spriteRend.flipX = Random.Range(0, 2) == 1;
    }
}
