using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : UICanvas
{

    public override void Open()
    {
        base.Open();

    }
    public void BtnPlay()
    {
        SceneController.Ins.ChangeScene("Main", () =>
        {
            UIManager.Ins.CloseUI<Home>();
            UIManager.Ins.OpenUI<GamePlay>();
            UIManager.Ins.bg.gameObject.SetActive(false);
        }, true, true);
    }
}
