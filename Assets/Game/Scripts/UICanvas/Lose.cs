using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : UICanvas
{
    public override void Open()
    {
        base.Open();
        Time.timeScale = 0f;
    }
    public void btn_Ads()
    {
        PlatformManager.Ins.platform.Clear();
        BallQueueManager.Ins.ballsWait.Clear();
        for (int i = 0; i < PlatformManager.Ins.platform.Count; i++)
        {
            Destroy(PlatformManager.Ins.platform[i].transform.gameObject);
        }
        for(int i = 0;i< BallQueueManager.Ins.ballsWait.Count; i++)
        {
            SimplePool.Despawn(BallQueueManager.Ins.ballsWait[i]);
        }

        InGameManager.Ins.Oninit();
        UIManager.Ins.CloseUI<Lose>();
    }
    public void btn_Reset()
    {
        Time.timeScale = 1f;
        UIManager.Ins.CloseUI<Lose>();
    }
}
