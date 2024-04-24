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
            ball.isSum = true;
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

        }
        if (collision.collider.CompareTag("Wall_Lose"))
        {
            Debug.LogError("Lose");
            //InGameManager.Ins.SpawnBomb(collision.collider.transform);
            UIManager.Ins.OpenUI<Lose>();
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
