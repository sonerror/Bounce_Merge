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
    public bool isMergeList = false;

    public GameObject vfx;
    public GameObject bomb;
    

    public int scoreCombo = 0;
    public bool isShootBomb = false;
    public Transform tfBomb;

    public bool isClickBtn = false;

    GameObject bombIns;
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
            MergeBall.Ins.MergeNumbers(DataManager.Ins.playerData.idMerge);
            MergeBall.Ins.MergeNumbers1(BallQueueManager.Ins.ballsWait);
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
        yield return new WaitForEndOfFrame();
        Time.timeScale = 1;
        BallQueueManager.Ins.Oninit();
        DataManager.Ins.playerData.totalScore += scoreCombo;
        scoreCombo = 0;
        MatManager.Ins.ChangeMatList();
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
        isMergeList = false;
        isShootBomb = true;
        isClickBtn = false;
        DataManager dataManager = DataManager.Ins;
        if (dataManager.playerData.idMerge.Count == 0)
        {
            PlayerPrefs.DeleteAll();
            dataManager.LoadData();
        }
        int count = dataManager.playerData.idMerge.Count;

        for (int i = 0; i < count; i++)
        {
            Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
            ball.transform.position = pathController.pathList[pathController.pathList.Count - i - 1].transform.position;
            ball.Oninit(i);
            Rigidbody rb = ball.rb;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePosition;
            BallQueueManager.Ins.ballsWait.Add(ball);
        }
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

        if (!isShootBomb)
        {
            Destroy(bombIns.gameObject);
            ShootBomb(targetPosition);
            yield return new WaitForSeconds(0.15f);
        }

        int countBall1 = BallQueueManager.Ins.ballsWait.Count;
        for (int i = 0; i < countBall1; i++)
        {
            ShootBall(targetPosition, i);
            yield return new WaitForSeconds(0.15f);
        }

        BallQueueManager.Ins.ballsWait.Clear();
        DataManager.Ins.playerData.idMerge.Clear();
        isMerge = false;
        isMergeList = true;
    }

    void ShootBomb(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - tfCannon.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Bomb bombSpawn = SimplePool.Spawn<Bomb>(PoolType.bomb);
        bombSpawn.transform.position = tfCannon.position;
        bombSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z);
        Rigidbody bombRb = bombSpawn.GetComponent<Rigidbody>();
        bombRb.velocity = direction * 90f;
        isShootBomb = true;
    }

    void ShootBall(Vector3 targetPosition, int index)
    {
        Vector3 direction = (targetPosition - tfCannon.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
        ball.rb.constraints = RigidbodyConstraints.None;
        ball.rb.constraints =
            RigidbodyConstraints.FreezePositionZ
            | RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationY;
        ballSpawns++;
        ball.transform.position = tfCannon.position;
        ball.transform.rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z);
        ball.Oninit(index);
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        ballRb.velocity = direction * 90f;
        Ball ballWait = BallQueueManager.Ins.ballsWait[index];
        if (index + 1 < BallQueueManager.Ins.ballsWait.Count)
        {
            Ball ballMove = BallQueueManager.Ins.ballsWait[index + 1];
            BallQueueManager.Ins.MoveToStartPos(ballMove.transform, index + 1);
        }
        SimplePool.Despawn(ballWait);
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
    public void ShootBomb()
    {

    }
    public void MoveBomb()
    {
        StartCoroutine(IE_MoveBomb());
    }

    IEnumerator IE_MoveBomb()
    {
        yield return new WaitForEndOfFrame();
        bombIns = Instantiate(bomb, this.transform);
        bombIns.transform.position = tfBomb.position;
        if(bombIns != null)
        {
            bombIns.transform.DOMove(pathController.pathList[pathController.pathList.Count - 1].position + new Vector3(0,0,-0.5f), 2f);
            isClickBtn = false;
        }
    }
    public void VFX_Bomb(Collision collision)
    {
        if (vfx != null)
        {
            GameObject vfxObj = Instantiate(vfx, this.transform);
            vfxObj.transform.position = collision.collider.transform.position;
            Destroy(vfxObj, 0.5f);
        }
    }    
}
