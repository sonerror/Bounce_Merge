using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : Singleton<PathController>
{
    [SerializeField]
    Transform tFPath;
    public Vector3[] pathArray;
    private void Start()
    {
        pathArray = new Vector3[tFPath.childCount];
        for (int i = 0; i < pathArray.Length; i++)
        {
            pathArray[i] = tFPath.GetChild(i).position;
        }
    }
}
