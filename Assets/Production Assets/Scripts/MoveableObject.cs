using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] private Vector2 moveTimeRange;
    //[SerializeField] private float moveTime;
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
        float moveTime = Random.Range(moveTimeRange.x, moveTimeRange.y);
        float d = startPos - endPos;
        float c = this.transform.position.x - endPos;
        float t = (c / d) * moveTime;
        //this.transform.DOLocalMoveX(endPos, t).SetEase(Ease.Linear).OnComplete(()=> NewMove());        
        this.transform.DOLocalMoveX(endPos, t).SetEase(Ease.Linear).OnComplete(() => UpdateSettings());
    }

    private void NewMove(float mt)
    {
        this.transform.localPosition = new Vector3(startPos, this.transform.localPosition.y);
        //this.transform.DOLocalMoveX(endPos, activeMoveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).OnStart(() => UpdateSettings());
        this.transform.DOLocalMoveX(endPos, mt).SetEase(Ease.Linear).OnComplete(() => UpdateSettings());
    }

    private void UpdateSettings()
    {
        spriteRend.flipX = Random.Range(0, 2) == 1;
        float moveTime = Random.Range(moveTimeRange.x, moveTimeRange.y);
        NewMove(moveTime);   
    }    
}
