using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Platform : GameUnit
{
    public TextMeshProUGUI textScore;
    public int scorePlatform = 33;
    public GameObject bomb;
    public int ballAdd = 1;

    private void OnEnable()
    {
        bomb.SetActive(false);
    }
    public void Start()
    {
        ballAdd = 1;
    }
    private void Update()
    {
        if (scorePlatform <= 0)
        {
            for(int i = 0; i < ballAdd; i++)
            {
                Ball ball = SimplePool.Spawn<Ball>(PoolType.ball);
                ball.transform.position = this.transform.position;
                ball.rb.AddForce(Vector3.up * 25f, ForceMode.Impulse);
                Vector3 randomDir = Random.insideUnitSphere.normalized;
                ball.rb.AddForce(randomDir * 8, ForceMode.Impulse);
                ball.rb.constraints = RigidbodyConstraints.None;
                ball.rb.constraints = RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                ball.idMerge = Random.Range(1, 3);
                MatManager.Ins.ChangeMat(ball.idMerge, ball.mat);
                InGameManager.Ins.ballSpawns++;
            }
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
            StartCoroutine(IE_HitBombLose());
           
        }
        if (collision.collider.CompareTag("Bomb"))
        {
            StartCoroutine(IE_HitBomb());
        }
    }
    IEnumerator IE_HitBomb()
    {
        yield return new WaitForEndOfFrame();
        bomb.SetActive(true);
        Punch();
        InGameManager.Ins.scoreCombo += this.scorePlatform;
        yield return new WaitForSeconds(0.2f);
        this.scorePlatform -= this.scorePlatform;
    }
    IEnumerator IE_HitBombLose()
    {
        bomb.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        UIManager.Ins.OpenUI<Lose>();
        for (int i = 0; i < PlatformManager.Ins.platform.Count; i++)
        {
            if (PlatformManager.Ins.platform[i].transform.position.y > 42)
            {
                Destroy(PlatformManager.Ins.platform[i].transform.gameObject);
                PlatformManager.Ins.platform.Remove(PlatformManager.Ins.platform[i]);
            }
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
