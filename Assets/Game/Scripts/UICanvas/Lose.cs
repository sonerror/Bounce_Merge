using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : UICanvas
{
    public override void Open()
    {
        base.Open();
        Time.timeScale = 0f;
    }
    public void btn_Ads()
    {

    }   
    public void btn_Reset()
    {
        Time.timeScale = 1f;
        UIManager.Ins.CloseUI<Lose>();
    }    
}
