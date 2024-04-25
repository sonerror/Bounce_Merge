using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Platform : GameUnit
{
    public TextMeshProUGUI textScore;
    public int scorePlatform = 33;
    private void Update()
    {
        if (scorePlatform <= 0)
        {
            Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
            ball.transform.position = this.transform.position;
            ball.rb.constraints = RigidbodyConstraints.None;
            ball.rb.constraints = RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            ball.idMerge = 2;
            MatManager.Ins.ChangeMat(ball.idMerge, ball.mat);
            InGameManager.Ins.ballSpawns++;
            PlatformManager.Ins.platform.Remove(this);
            Destroy(this.gameObject);
        }
        else
        {
            textScore.text = scorePlatform.ToString();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Punch();
            Ball ball = collision.collider.GetComponent<Ball>();
            scorePlatform -= ball.scoreBall;
            InGameManager.Ins.scoreCombo += ball.scoreBall;
        }
        if (collision.collider.CompareTag("Wall_Lose"))
        {
            for (int i = 0; i < PlatformManager.Ins.platform.Count; i++)
            {
                if (PlatformManager.Ins.platform[i].transform.position.y > 42)
                {
                    Destroy(PlatformManager.Ins.platform[i].transform.gameObject);
                }
            }
            UIManager.Ins.OpenUI<Lose>();
        }
        if (collision.collider.CompareTag("Bomb"))
        {
            Punch();
            InGameManager.Ins.scoreCombo += this.scorePlatform;
            this.scorePlatform -= this.scorePlatform;
        }
    }

    public void Punch()
    {
        transform.DOShakePosition(0.3f, strength: new Vector3(0.3f, 0.3f, 0), vibrato: 10, randomness: 90);
    }
    public void PunchDie()
    {
        transform.DOShakePosition(0.5f, strength: new Vector3(0.5f, 0.5f, 0), vibrato: 10, randomness: 90).
            OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
    }
}
