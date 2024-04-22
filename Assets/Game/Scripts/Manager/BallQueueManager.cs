using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallQueueManager : Singleton<BallQueueManager>
{
    public List<Ball> ballsWait;
    public PathController pathController;
    public Vector3[] pathArray;
    [SerializeField]
    PathType pathType;
    private void Start()
    {
        pathArray = PathController.Ins.pathArray;
    }
    private void Update()
    {

    }
    public void MoveBallQueue(Transform tf, int _i)
    {
        Vector3[] pathPos = new Vector3[2];
        pathPos[0] = tf.position;
        pathPos[1] = pathArray[pathArray.Length - 1];
        tf.DOPath(pathPos, 0.5f, pathType);
    }

    public void MoveToStartPos(Transform tf, int _i)
    {
        StartCoroutine(IE_MoveToStartPos(tf, _i));
    }
    IEnumerator IE_MoveToStartPos(Transform tf, int _i)
    {
        if (_i <= 5)
        {
            Vector3[] pathPos = new Vector3[2];
            pathPos[0] = pathArray[pathArray.Length - 2];
            pathPos[1] = pathArray[pathArray.Length - 1];
            yield return tf.DOPath(pathPos, 0.3f, pathType);
        }
        else
        {
            Vector3[] pathPos = new Vector3[3];
            pathPos[0] = tf.position;
            pathPos[1] = pathArray[pathArray.Length - 7];
            pathPos[2] = pathArray[pathArray.Length - 1];
            yield return tf.DOPath(pathPos, 0.3f + _i / 10, pathType);
        }
    }
}
