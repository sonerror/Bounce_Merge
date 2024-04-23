using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _ins;
    public static InGameManager Ins => _ins;
    public Transform tfCannon;
    public GameObject ballsp;
    public LayerMask mask;
    public PathController pathController;
    public int countBall = 0;
    public int ballSpawns = 0; // cu spawn ra la phai cong vao
    public bool isRoCannon = false;
    public bool isMerge = false;
    public bool isRo = false;
    public GameObject bomb;
    private void Awake()
    {
        _ins = this;
    }
    public void Start()
    {
        Oninit();
        isMerge = true;
        isRo = false;
        PlatformManager.Ins.LoadPlatform();
    }
    private void Update()
    {
        SetMergeBall();

    }
    void SetMergeBall()
    {
        if (isRotationCannon())
        {
            if (isMerge == false)
            {
                StartCoroutine(MergeBallAfterShoot());
                isMerge = true;
            }
            isRoCannon = true;
        }
    }
    IEnumerator MergeBallAfterShoot()
    {
        yield return new WaitForSeconds(1f);
        //MergeBall.Ins.MergeNumbers(DataManager.Ins.playerData.idMerge);
        MergeBall.Ins.MergeNumbers1(BallQueueManager.Ins.ballsWait);
        yield return new WaitForEndOfFrame();
        if (isRotationCannon() && PlatformManager.Ins.platform.Count == 0)
        {
            PlatformManager.Ins.LoadPlatform();
        }
        else if (isRotationCannon() && PlatformManager.Ins.platform.Count > 0)
        {
            PlatformManager.Ins.LoadPlatform();
        }
    }

    void MoveBall()
    {
        for (int i = 0; i < BallQueueManager.Ins.ballsWait.Count; i++)
        {
            BallQueueManager.Ins.ballsWait[i].transform.DOPath(PathController.Ins.pathArray, 3f, PathType.CatmullRom);
        }
    }
    public void Oninit()
    {
        for (int i = 0; i < DataManager.Ins.playerData.idMerge.Count; i++)
        {
            Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
            ball.transform.position = pathController.pathList[pathController.pathList.Count - i - 1].transform.position;
            ball.Oninit(i);
            ball.rb.velocity = Vector3.zero;
            ball.rb.constraints = RigidbodyConstraints.FreezePosition;
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
        countBall = 0;
        ballSpawns = 0;

        int countBall1 = BallQueueManager.Ins.ballsWait.Count;
        for (int i = 0; i < countBall1; i++)
        {
            Vector3 direction = (tfCannon.position - targetPosition).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
            ball.rb.constraints = RigidbodyConstraints.None;
            yield return new WaitForEndOfFrame();
            ball.rb.constraints =
                RigidbodyConstraints.FreezePositionZ
                | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationY;
            ballSpawns++;
            ball.transform.position = tfCannon.position;
            ball.transform.rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z);
            ball.Oninit(i);
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            ballRb.velocity = direction * 90f;
            Ball ballWait = BallQueueManager.Ins.ballsWait[i];
            if (i + 1 < countBall1)
            {
                Ball ballMove = BallQueueManager.Ins.ballsWait[i + 1];
                BallQueueManager.Ins.MoveToStartPos(ballMove.transform, i + 1);
            }
            SimplePool.Despawn(ballWait);
            yield return new WaitForSeconds(0.3f);
        }
        BallQueueManager.Ins.ballsWait.Clear();
        DataManager.Ins.playerData.idMerge.Clear();
        isMerge = false;
    }
    public int ScoreBall(int n)
    {
        return (int)Math.Pow(2, n);
    }
    public bool isRotationCannon()
    {
        return countBall >= ballSpawns;
    }
    public void SpawnBomb(Transform tf)
    {
        Instantiate(bomb, tf);
        Destroy(bomb, 2f);
    }
}
