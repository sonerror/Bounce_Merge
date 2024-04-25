using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CannonController : MonoBehaviour
{
    public LayerMask mask;
    Vector3 mousePosition;
    public Transform trCannon;
    public LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer.positionCount = 3;
        UIManager.Ins.GetUI<GamePlay>().imgTarget.SetActive(true);

    }
    private void Update()
    {
        if (InGameManager.Ins.isClickBtn == false)
        {
            if (InGameManager.Ins.isRotationCannon() && InGameManager.Ins.isRo == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                  
                    mousePosition = Input.mousePosition;
                    UIManager.Ins.GetUI<GamePlay>().imgTarget.transform.position = new Vector3(mousePosition.x, mousePosition.y + 100f, 0f);
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = Camera.main.nearClipPlane;
                    Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    Vector3 rayDirection = transform.TransformDirection(Vector3.down);


                    Ray ray = new Ray(transform.position, rayDirection);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, mask))
                    {
                        Vector3 startPosition = transform.position;
                        Vector3 endPosition = hitInfo.point;
                        Vector3 reflectDirection = Vector3.Reflect(endPosition - startPosition, hitInfo.normal);// diem thu 3 la goc phan xa
                        Vector3 reflectPosition = hitInfo.point + reflectDirection;
                        Vector3 finalPosition = (endPosition + reflectPosition) / 5f;
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, startPosition);
                        lineRenderer.SetPosition(1, endPosition);
                        lineRenderer.SetPosition(2, finalPosition);
                    }

                    Vector3 direction = transform.position - targetPosition;
                    Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 50 * Time.deltaTime);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    lineRenderer.enabled = false;
                    Vector3 rayDirection = transform.TransformDirection(Vector3.down);
                    Ray ray = new Ray(transform.position, rayDirection);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, mask))
                    {
                        InGameManager.Ins.ShootBall(hitInfo.point);
                    }
                    InGameManager.Ins.isRoCannon = false;
                    InGameManager.Ins.isRo = false;
                }
            }
        }
    }
}
