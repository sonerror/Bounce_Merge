using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Ball : GameUnit
{
    public int scoreBall = 0;
    private Vector3 initialVelocity;
    [SerializeField]
    private float minVelocity = 10f;
    private Vector3 lastFrameVelocity;
    public Rigidbody rb;
    [SerializeField]
    PathType pathType;
    bool isMovePath = false;
    [SerializeField] LayerMask mask;
    float distance = 0;
    public int idMerge = 1;
    public Vector3[] pathArray;
    public TextMeshProUGUI text;
    public MeshRenderer mat;

    private void OnEnable()
    {
        rb.velocity = initialVelocity;
        isMovePath = false;
    }
    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ball"), LayerMask.NameToLayer("Ball"), true);
        pathArray = PathController.Ins.pathArray;
    }
    public void Oninit(int i)
    {
        idMerge = DataManager.Ins.playerData.idMerge[i];
        MatManager.Ins.ChangeMat(idMerge, mat);
    }
    private void Update()
    {
        scoreBall = InGameManager.Ins.ScoreBall(idMerge);
        text.text = scoreBall.ToString();
        lastFrameVelocity = rb.velocity;
        if (isMovePath == false)
        {
            Ray ray = new Ray(this.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, mask))
            {
                if (hit.collider.CompareTag("Wall_Down"))
                {
                    distance = Vector2.Distance(this.transform.position, hit.point);
                    if (distance < 2)
                    {
                        this.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePosition;
                        rb.velocity = Vector3.zero;
                        MovePointStart();
                    }
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Bounce(collision.contacts[0].normal);

        }
    }
    public void MovePointStart()
    {
        if (isMovePath == false)
        {
            if (PathController.Ins.pathArray.Length > 0)
            {
                this.transform.DOPath(PathController.Ins.pathArray, 3f, pathType).OnComplete(() =>
                {
                    BallQueueManager.Ins.ballsWaitTemp.Add(this);
                    InGameManager.Ins.countBall++;
                    this.rb.constraints = RigidbodyConstraints.FreezePosition;
                    DataManager.Ins.playerData.idMerge.Add(this.idMerge);
                });
                Array.Resize(ref PathController.Ins.pathArray, PathController.Ins.pathArray.Length - 1);
                isMovePath = true;
            }
            else
            {
                Vector3[] pathPos = new Vector3[2];
                pathPos[0] = this.transform.position;
                pathPos[1] = pathArray[0];
                this.transform.DOPath(pathPos, 1f, pathType).OnComplete(() =>
                {
                    BallQueueManager.Ins.ballsWaitTemp.Add(this);
                    InGameManager.Ins.countBall++;
                    this.rb.constraints = RigidbodyConstraints.FreezePosition;
                    DataManager.Ins.playerData.idMerge.Add(this.idMerge);
                });
                isMovePath = true;
            }

        }
    }
    private void Bounce(Vector3 collisionNormal)
    {
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        var newSpeed = Mathf.Clamp(lastFrameVelocity.magnitude * 2f, minVelocity, 50);
        rb.velocity = direction * newSpeed * 0.9f;
    }
}
