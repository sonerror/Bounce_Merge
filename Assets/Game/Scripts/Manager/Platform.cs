using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int scorePlatform = 100;
    private void Start()
    {

    }
    private void Update()
    {
        if (scorePlatform <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Punch();
            Debug.LogError(collision.collider.gameObject.name);
            Ball ball = collision.collider.GetComponent<Ball>();
            scorePlatform -= ball.scoreBall;
            Debug.LogError(scorePlatform);
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
