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
    private void Update()
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
            SpawnPlatform(3, platformInfo[i]);
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
                GetPlatformInfoData(i, i);
            }
        }
    }
    IEnumerator IE_MoveP()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform tfChid in tf)
        {
            yield return tfChid.transform.DOMoveY(tfChid.transform.position.y + 10f, 2f).OnComplete(() =>
            {
                InGameManager.Ins.isRo = true;
            }).SetEase(Ease.Linear);
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
    public void GetPlatformInfoData(int id, int i)
    {
        var platformInfo = DataManager.Ins.playerData.platformInfo;
        PlatformInfo platform = new PlatformInfo();
        List<float> pos = new List<float> { 5.5f, 2.4f, 11.0f };
        float randomY = UnityEngine.Random.Range(1, 5);
        platform.id = id;
        platform.position = new Vector3(pos[i], 1.1f + randomY, 0);
        platform.eulerAngle = GetAngleRanDom();
        platform.scale = GetScale();
        if (GetScorePlatform() > 6)
        {
            platform.scorePlatform = (int)Math.Pow(2, (GetScorePlatform() + 3)) + 1;
        }
        else
        {
            platform.scorePlatform = (int)Math.Pow(2, (GetScorePlatform() + 1)) + 1;
        }
        platformInfo.Add(platform);
    }
    public void SpawnPlatform(int id, PlatformInfo platformInfo)
    {
        Platform _platform = Instantiate(platformAssetData.GetPlatformWithID(id).platform, tf);
        _platform.transform.position = platformInfo.position;
        _platform.transform.rotation = Quaternion.Euler(platformInfo.eulerAngle);
        _platform.transform.localScale = platformInfo.scale;
        if (GetScorePlatform() > 6)
        {
            _platform.scorePlatform = (int)Math.Pow(2, (GetScorePlatform() + 3)) + 1;
        }
        else
        {
            _platform.scorePlatform = (int)Math.Pow(2, (GetScorePlatform() + 1)) + 1;
        }
        platform.Add(_platform);
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
