using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : UICanvas
{
    public GameObject imgTarget;
    private void Start()
    {
        imgTarget.SetActive(false);
        Open();
    }
    public override void Open()
    {
        base.Open();
    }
}
