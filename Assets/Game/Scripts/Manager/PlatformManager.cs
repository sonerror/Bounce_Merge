using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private static PlatformManager _ins;
    public static PlatformManager Ins => _ins;

    public PlatformAssetData platformAssetData;
    public Transform tf;
    public List<Platform> platform;
    private void Awake()
    {
        _ins = this;
    }
    private void Start()
    {

    }
    public void Oninit()
    {
        LoadFirst();
    }
    public void LoadPlatform()
    {
        for (int i = 0; i < DataManager.Ins.playerData.platformInfo.Count; i++)
        {
            var platformInfo = DataManager.Ins.playerData.platformInfo;
            SpawnPlatform(1, platformInfo[i]);
        }
        StartCoroutine(IE_MoveP());
    }
    void LoadFirst()
    {
        if (DataManager.Ins.playerData.platformInfo.Count == 0)
        {
            Debug.LogError("first Load");
            for (int i = 0; i < 3; i++)
            {
                GetPlatformInfoData(i);
            }
        }
    }
    IEnumerator IE_MoveP()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform tfChid in tf)
        {
            yield return tfChid.transform.DOMoveY(tfChid.transform.position.y + 10f, 2f).SetEase(Ease.Linear);
        }
        yield return new WaitForEndOfFrame();
    }
    public void UpdatePos()
    {
        var platformInfo = DataManager.Ins.playerData.platformInfo;

        for (int i = 0; i < platformInfo.Count; i++)
        {
            platformInfo[i].position.y = platformInfo[i].position.y + 10f;
        }
    }
    public void GetPlatformInfoData(int id)
    {
        var platformInfo = DataManager.Ins.playerData.platformInfo;
        PlatformInfo platform = new PlatformInfo();
        platform.id = id;
        platform.position = GetTransform();
        platform.eulerAngle = GetAngleRanDom();
        platform.scale = GetScale();
        platform.scorePlatform = (int)Math.Pow(2, (GetScorePlatform() + 1));
        platformInfo.Add(platform);
    }
    public void SpawnPlatform(int id, PlatformInfo platformInfo)
    {
        Platform _platform = Instantiate(platformAssetData.GetPlatformWithID(id).platform, tf);
        _platform.transform.position = platformInfo.position;
        _platform.transform.rotation = Quaternion.Euler(platformInfo.eulerAngle);
        _platform.transform.localScale = platformInfo.scale;
        _platform.scorePlatform = platformInfo.scorePlatform;
        platform.Add( _platform );
    }
    public int GetScorePlatform()
    {
        List<int> idMerge = DataManager.Ins.playerData.idMerge;

        int maxNumber = idMerge[0];

        foreach (int number in idMerge)
        {
            if (number > maxNumber)
            {
                maxNumber = number;
            }
        }
        return maxNumber;
    }

    public Vector3 GetTransform()
    {
        float randomX = UnityEngine.Random.Range(-4.4f, 11.0f);
        float randomY = -1.1f;

        return new Vector3(randomX, randomY, 0);
    }
    public Vector3 GetAngleRanDom()
    {
        int angle = UnityEngine.Random.Range(0, 90);
        return new Vector3(0, 0, angle);
    }
    public Vector3 GetScale()
    {
        float scale = UnityEngine.Random.Range(4f, 6f);
        return new Vector3(scale, scale, 5);
    }
}
