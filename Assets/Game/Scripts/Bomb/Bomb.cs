using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : GameUnit
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            InGameManager.Ins.VFX_Bomb(collision);
            SimplePool.Despawn(this);
        }
    }
}
