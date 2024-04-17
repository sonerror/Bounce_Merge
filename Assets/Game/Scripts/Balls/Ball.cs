using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : GameUnit
{
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

    private void OnEnable()
    {
        rb.velocity = initialVelocity;
    }
    private void Update()
    {
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
                    Debug.Log(1);
                    if (distance < 2)
                    {

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
            var speed = lastFrameVelocity.magnitude;
            Bounce(collision.contacts[0].normal, speed * 2 / 3);
        }
    }


    public void MovePointStart()
    {
        if (isMovePath == false)
        {
            this.transform.DOPath(PathController.Ins.pathArray, 3f, pathType);
            isMovePath = true;
        }
    }
    private void Bounce(Vector3 collisionNormal, float speed)
    {
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        rb.velocity = direction * Mathf.Max(speed, minVelocity);
    }
}
