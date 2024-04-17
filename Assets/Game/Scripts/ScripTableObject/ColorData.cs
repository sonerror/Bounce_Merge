using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialType
{
    red = 0,
    green = 1,
    blue = 2,
    white = 3,
    yellow = 4,
    Black = 5,
}

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData", order = 1)]
public class ColorData : ScriptableObject
{
    [SerializeField] private Material[] materials;

    public Material GetMat(MaterialType materialType)
    {
        return materials[(int)materialType];
    }
}
