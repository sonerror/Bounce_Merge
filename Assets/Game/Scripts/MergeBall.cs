using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBall : MonoBehaviour
{
    private static MergeBall _ins;
    public static MergeBall Ins => _ins;
    private void Awake()
    {
        _ins = this;
    }
    public void MergeNumbers(List<int> numbers)
    {
       StartCoroutine(IE_MergeNumbers(numbers));
    }

    IEnumerator IE_MergeNumbers(List<int> numbers)
    {
        int n = numbers.Count;
        int i = 0;
        while (i < n - 1)
        {
            if (numbers[i] != 0 && numbers[i] == numbers[i + 1])
            {
                numbers[i] += 1;
                BallQueueManager.Ins.ballsWait[i].idMerge = numbers[i];
                SimplePool.Despawn(BallQueueManager.Ins.ballsWait[i + 1]);
                numbers[i + 1] = 0;
                n--;
                yield return new WaitForSeconds(2f);
            }
            else
            {
                i++;
            }
            yield return new WaitForSeconds(0.1f);
        }
        numbers.RemoveAll(num => num == 0);
    }
}
