using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCannon : MonoBehaviour
{
    public float rotateSpeed = 5f;
    private bool isRotatingToMouse = false;
    private Vector3 targetDirection;
    private Vector3 lastMousePosition;
    public Transform tfCannon;
    public GameObject ballsp;
    public LayerMask mask;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            targetDirection = transform.position - lastMousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;
            float rotateAmount = mouseDelta.x * rotateSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotateAmount);
        }
        else if (Input.GetMouseButtonUp(0)) // tha chuot thi ban
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 2f);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.LogError(hit.point);
               StartCoroutine(IE_ShootBall(hit.point));
            }
        }
    }
    IEnumerator IE_ShootBall(Vector3 targetPosition)
    {
        for (int i = 0; i < 1; i++)
        {
            Shoot(targetPosition);
            yield return new WaitForSeconds(0.5f);
        }
    }
    void Shoot(Vector3 targetPosition)
    {
        Debug.LogError("Shoot");
        Vector3 direction = (targetPosition - tfCannon.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Ball ballObject = SimplePool.Spawn<Ball>(PoolType.ball);
        ballObject.transform.position = tfCannon.position;
        ballObject.transform.rotation = rotation;
        Rigidbody ballRb = ballObject.GetComponent<Rigidbody>();
        ballRb.velocity = direction * 50f;
    }
}
