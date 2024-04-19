using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallQueueManager : Singleton<BallQueueManager>
{
    public List<Ball> ballsWait;
    public PathController pathController;
    private void Update()
    {

    }
    public void MoveBallQueue()
    {
        bool isExit = false;
        while (isExit == false)
        {
            QueueBall foundBall = pathController.queueBalls.Find(ball => ball.isSort == true);
            if (foundBall != null)
            {
                for (int i = 0; i < ballsWait.Count; i++)
                {

                }    
            }
            else
            {
                isExit = true;
            }
        }
    }
}
