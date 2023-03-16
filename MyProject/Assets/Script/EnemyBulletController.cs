using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private void DestroyEnemyBullet()
    {
        Destroy(gameObject, 0.016f);
    }
}
