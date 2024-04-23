using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformAssetData", menuName = "ScriptableObjects/PlatformAssetData", order = 2)]
public class PlatformAssetData : ScriptableObject
{
    public List<PlatformDataModel> listPlatformData = new List<PlatformDataModel>();
    public PlatformDataModel GetPlatformWithID(int _id)
    {
        return listPlatformData.Find(e => e.id == _id);
    }
}
[System.Serializable]
public class PlatformDataModel
{
    public int id;
    public Platform platform;
    public void CoppyFormOther(PlatformDataModel other)
    {
        id = other.id;
        platform = other.platform;
    }
}
