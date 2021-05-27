using System;
using kTools.Pooling;
using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    private float time = 0f;

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            time += Time.deltaTime;
            if (time >= WeaponSystem.instance.config.lifeTime)
            {
                LifeTimeEnd();
                time = 0f;
                Debug.Log("Çalıştı.");
            }
        }
        else
        {
            time = 0f;
        }
    }

    private void LifeTimeEnd()
    {
        PoolingSystem.ReturnInstance(WeaponSystem.instance.projectilePoolKey, this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Physics.IgnoreCollision(other.collider,GetComponent<Collider>());
        }else
        {
            PoolingSystem.ReturnInstance(WeaponSystem.instance.projectilePoolKey, this.gameObject);
        }
    }
}