using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InGameManager : Singleton<InGameManager>
{
    public Transform tfCannon;
    public GameObject ballsp;
    public LayerMask mask;
    public PathController pathController;
    public int countBall = 0;
    public int ballSpawns = 0; // cu spawn ra la phai cong vao
    public void Start()
    {
        Oninit();
    }
    public void Oninit()
    {
        for (int i = 0; i < 10; i++)
        {
            Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
            ball.transform.position = pathController.pathList[pathController.pathList.Count - (i + 1)].transform.position;
            ball.rb.velocity = Vector3.zero;
            BallQueueManager.Ins.ballsWait.Add(ball);
        }
    }
    IEnumerator I_InitGame()
    {
        yield return null;
    }
    public void ShootBall(Vector3 targetPosition)
    {
        StartCoroutine(IE_ShootBall(targetPosition));
    }
    IEnumerator IE_ShootBall(Vector3 targetPosition)
    {
        pathController.Oninit();
        InGameManager.Ins.countBall = 0;
        InGameManager.Ins.ballSpawns = 0;
        int countBall1 = BallQueueManager.Ins.ballsWait.Count;
        for (int i = 0; i < countBall1; i++)
        {
            Vector3 direction = (tfCannon.position - targetPosition).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
            ballSpawns++;
            ball.transform.position = tfCannon.position;
            ball.transform.rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z);
            ball.Oninit();
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            ballRb.velocity = direction * 80f;
            Ball ball1 = BallQueueManager.Ins.ballsWait[i];
           
            if (i+1 < countBall1)
            {
                Ball ball2 = BallQueueManager.Ins.ballsWait[i+1];
                ball2.MoveToStartPos(ball2.transform);
            }
            SimplePool.Despawn(ball1);
            yield return new WaitForSeconds(0.3f);
        }
        BallQueueManager.Ins.ballsWait.Clear();
    }
    public int ScoreBall(int n)
    {
        return (int)Math.Pow(2, n);
    }
    public bool isRotationCannon()
    {
        return countBall >= ballSpawns;
    }
}
