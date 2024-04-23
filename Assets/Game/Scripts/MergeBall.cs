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

    private IEnumerator IE_MergeNumbers(List<int> numbers)
    {
        int n = numbers.Count;
        for (int i = 0; i < n - 1; i++)
        {
            if (numbers[i] != 0 && numbers[i] == numbers[i + 1])
            {
                numbers[i] += 1;
                BallQueueManager.Ins.ballsWait[i].idMerge = numbers[i];
                SimplePool.Despawn(BallQueueManager.Ins.ballsWait[i + 1]);
                BallQueueManager.Ins.ballsWait.RemoveAt(i + 1);
                numbers[i + 1] = 0;
                MoveNumbersToLeft(numbers);
                yield return new WaitForEndOfFrame();
            }
        }
        numbers.RemoveAll(num => num == 0);
    }
    private void MoveNumbersToLeft(List<int> numbers)
    {
        int n = numbers.Count;
        int j = 0;
        for (int i = 0; i < n; i++)
        {
            if (numbers[i] != 0)
            {
                numbers[j++] = numbers[i];
            }
        }
        for (int i = j; i < n; i++)
        {
            numbers[i] = 0;
        }
    }

}
