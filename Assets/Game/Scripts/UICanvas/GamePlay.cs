using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : UICanvas
{
    private void Start()
    {
        Open();
    }
    public override void Open()
    {
        base.Open();
        Debug.Log("nônno");
    }
}
