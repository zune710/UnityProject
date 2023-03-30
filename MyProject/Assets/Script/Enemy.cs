using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Chameleon,
    Plant,
    Rock
};
public struct EnemyInfo
{
    public EnemyType enemyType;
    public bool hasBullet;
    public int HP;
    public float Speed;
    public float AttackRange;
    public float SpawnTime;
}

public class Enemy : MonoBehaviour
{
    public EnemyInfo info;
    public Enemy() { }

}

public class Chameleon : Enemy
{
    public Chameleon() : base()
    {
        info = new EnemyInfo();
        info.enemyType = EnemyType.Chameleon;
        info.hasBullet = false;
        info.HP = 3;
        info.Speed = 0.2f;
        info.AttackRange = 2.0f;
        info.SpawnTime = 1.5f;
    }
}

public class Plant : Enemy
{
    public Plant() : base()
    {
        info = new EnemyInfo();
        info.enemyType = EnemyType.Chameleon;
        info.hasBullet = false;
        info.HP = 5;
        info.Speed = 0.0f;
        info.AttackRange = 14.5f;
        info.SpawnTime = 3.0f;
    }
}
