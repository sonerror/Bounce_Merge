using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private static PathController _ins;
    public static PathController Ins => _ins;
    [SerializeField]
    Transform tFPath;
    public Vector3[] pathArray;
    public List<Transform> pathList;
    public List<QueueBall> queueBalls;

    private void Awake()
    {
        _ins = this;
    }
    private void Start()
    {
        Oninit();
    }
    public void Oninit()
    {
        pathArray = null;

        pathArray = new Vector3[tFPath.childCount];
        for (int i = 0; i < pathArray.Length; i++)
        {
            pathArray[i] = tFPath.GetChild(i).position;
        }
    }
    public void GetQueueBall()
    {
        for (int i = 0; i < tFPath.childCount; i++)
        {
            queueBalls[i].posSort = tFPath.GetChild(i).transform; 
        }
    }
}
[System.Serializable]
public class QueueBall
{
    public bool isSort;
    public Transform posSort;
}
