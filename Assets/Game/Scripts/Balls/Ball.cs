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
        Debug.LogError(scoreBall + " score");
    }

    private void Update()
    {
        text.text = InGameManager.Ins.ScoreBall(idMerge).ToString();
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
            this.transform.DOPath(PathController.Ins.pathArray, 3f, pathType).OnComplete(() =>
            {
                BallQueueManager.Ins.ballsWait.Add(this);
                InGameManager.Ins.countBall++;
                this.rb.constraints = RigidbodyConstraints.FreezePosition;
                DataManager.Ins.playerData.idMerge.Add(this.idMerge);
            });
            Array.Resize(ref PathController.Ins.pathArray, PathController.Ins.pathArray.Length - 1);
            isMovePath = true;
        }
    }
   
    private void Bounce(Vector3 collisionNormal)
    {
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        var newSpeed = Mathf.Clamp(lastFrameVelocity.magnitude * 2f, minVelocity, 50);
        rb.velocity = direction * newSpeed;
    }
}
