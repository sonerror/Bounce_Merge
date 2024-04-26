using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    public GameObject imgTarget;
    public TextMeshProUGUI textTotal;
    public TextMeshProUGUI textCombo;
    public Button btn_bomb;
    public TextMeshProUGUI textBomb;
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
        textBomb.text = "x" + DataManager.Ins.playerData.countBom.ToString();
        if (DataManager.Ins.playerData.countBom < 0)
        {
            textBomb.text = "x0";

        }
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
    public void btn_Sort()
    {
        InGameManager.Ins.isRo = false;
        if (InGameManager.Ins.isShootBomb == true)
        {
            InGameManager.Ins.cannonController.lineRenderer.enabled = false;
            InGameManager.Ins.isShootBomb = false;
            InGameManager.Ins.isClickBtn = true;
            InGameManager.Ins.SortBall();
        }
    }
    public void btn_Bomb()
    {
        DataManager.Ins.playerData.countBom -= 1;
        if (DataManager.Ins.playerData.countBom > 0)
        {
            if (InGameManager.Ins.isShootBomb == true)
            {
                InGameManager.Ins.cannonController.lineRenderer.enabled = false;
                InGameManager.Ins.isShootBomb = false;
                InGameManager.Ins.isClickBtn = true;
                InGameManager.Ins.MoveBomb();
            }
        }
    }
}
