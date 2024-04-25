using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : GameUnit
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            SimplePool.Despawn(this);
        }
    }
    IEnumerator IE_HitBomb(Collision collision)
    {
        yield return new WaitForEndOfFrame();
        VFX_Bomb vfxbom = SimplePool.Spawn<VFX_Bomb>(PoolType.VFX);
        vfxbom.transform.position = collision.collider.transform.position;
        yield return new WaitForSeconds(2f);
        SimplePool.Despawn(vfxbom);
    }
}