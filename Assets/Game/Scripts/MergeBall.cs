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
        int n = numbers.Count;
        int j = 0;
        for (int i = 0; i < n - 1; i++)
        {
            if (numbers[i] != 0 && numbers[i] == numbers[i + 1])
            {
                numbers[i] += 1;
                BallQueueManager.Ins.ballsWait[i].idMerge = numbers[i];
                SimplePool.Despawn(BallQueueManager.Ins.ballsWait[i + 1]);
                numbers[i + 1] = 0;
            }
        }
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
        numbers.RemoveAll(num => num == 0);
    }
    public void MergeNumbers1(List<Ball> numbers)
    {
        int n = numbers.Count;
        int j = 0;
        for (int i = 0; i < n - 1; i++)
        {
            if (numbers[i].idMerge != 0 && numbers[i].idMerge == numbers[i + 1].idMerge)
            {
                numbers[i + 1].idMerge = 0;
            }
        }
        for (int i = 0; i < n; i++)
        {
            if (numbers[i].idMerge != 0)
            {
                numbers[j++].idMerge = numbers[i].idMerge;
            }
        }
        for (int i = j; i < n; i++)
        {
            numbers[i].idMerge = 0;
        }
        numbers.RemoveAll(num => num.idMerge == 0);
    }
}
