using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatAssetData", menuName = "ScriptableObjects/MatAssetData", order = 3)]
public class MatAssetData : ScriptableObject
{
    public List<MatDataModel> listMatData = new List<MatDataModel>();
    public MatDataModel GetMatWithID(int _id)
    {
        return listMatData.Find(e => e.id == _id);
    }
}
[System.Serializable]
public class MatDataModel
{
    public int id;
    public Material mat;
}