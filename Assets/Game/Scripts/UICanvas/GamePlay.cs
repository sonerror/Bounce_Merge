using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlay : UICanvas
{
    public GameObject imgTarget;
    public TextMeshProUGUI textTotal;
    public TextMeshProUGUI textCombo;
    private void Start()
    {
        imgTarget.SetActive(false);
        Open();
    }
    public override void Open()
    {
        base.Open();
    }
    private void Update()
    {
        int totalScore = DataManager.Ins.playerData.totalScore;
        int scoreCombo = InGameManager.Ins.scoreCombo;
        textTotal.text = FormatText.FormatNumber(totalScore);
        textCombo.text = FormatText.FormatNumber(scoreCombo);
    }
    public void btn_X2()
    {
        if (Time.timeScale < 2)
        {
            Time.timeScale = 2;
        }
    }
    public void btn_X1()
    {
        Time.timeScale = 1;
    }

}
