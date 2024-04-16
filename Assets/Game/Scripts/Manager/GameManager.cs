using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected void Awake()
    {
        Input.multiTouchEnabled = true;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Start()
    {
        StartCoroutine(I_InitGame());
    }


    IEnumerator I_InitGame()
    {
        yield return new WaitUntil(
            () => (
            Ins != null
             && UIManager.Ins != null
            )
        );
        UIManager.Ins.OpenUI<Loading>();
    }
}
