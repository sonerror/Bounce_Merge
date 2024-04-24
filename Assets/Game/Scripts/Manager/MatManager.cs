using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatManager : MonoBehaviour
{
    private static MatManager _ins;
    public static MatManager Ins => _ins;
    public MatAssetData matAssetData;
    private void Awake()
    {
        _ins = this;
    }
    public void ChangeMat(int _idMerge, MeshRenderer mat)
    {
        int materialIndex = (_idMerge - 1) % 11;
        mat.material = MatManager.Ins.matAssetData.GetMatWithID(materialIndex).mat;
    }
    public void ChangeMatList()
    {
        for(int i = 0;i< BallQueueManager.Ins.ballsWait.Count;i++)
        {
            int materialIndex = (BallQueueManager.Ins.ballsWait[i].idMerge - 1) % 11;
            BallQueueManager.Ins.ballsWait[i].mat.material = MatManager.Ins.matAssetData.GetMatWithID(materialIndex).mat;
        }
    }    
}
