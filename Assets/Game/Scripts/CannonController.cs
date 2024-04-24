using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public LayerMask mask;
    Vector3 mousePosition;
    public Transform trCannon;
    private void Start()
    {
        UIManager.Ins.GetUI<GamePlay>().imgTarget.SetActive(true);
    }
    private void Update()
    {
        if(InGameManager.Ins.isRotationCannon() && InGameManager.Ins.isRo == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePosition = Input.mousePosition;
                UIManager.Ins.GetUI<GamePlay>().imgTarget.transform.position = new Vector3(mousePosition.x, mousePosition.y + 100f, 0f);
                //UIManager.Ins.GetUI<GamePlay>().imgTarget.SetActive(true);
            }
            else if (Input.GetMouseButton(0))
            {
                // Vector3 moveAmount = new Vector3(Input.GetAxis("Mouse X")*6, Input.GetAxis("Mouse Y")*6, 0f) * 200 * Time.deltaTime;
                // UIManager.Ins.GetUI<GamePlay>().imgTarget.transform.position += moveAmount;
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Camera.main.nearClipPlane;
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 direction = transform.position - targetPosition;

                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 50 * Time.deltaTime);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 rayDirection = transform.TransformDirection(Vector3.forward);
                Ray ray = new Ray(transform.position, rayDirection);
                //UIManager.Ins.GetUI<GamePlay>().imgTarget.SetActive(false);
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
