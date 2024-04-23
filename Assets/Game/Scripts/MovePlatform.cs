using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public void MoveP()
    {
        StartCoroutine(IE_MoveP());
    }
    IEnumerator IE_MoveP()
    {
        yield return new WaitForSeconds(5f);
        yield return this.transform.DOMoveY(2f, 2f).SetEase(Ease.Linear);

    }
}
