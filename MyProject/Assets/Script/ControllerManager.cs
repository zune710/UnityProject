using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager
{
    // 코드의 안정성 위해 필수!
    private ControllerManager() { }

    private static ControllerManager Instance = null;

    public static ControllerManager GetInstance()
    {
        if (Instance == null)
            Instance = new ControllerManager();
        return Instance;
    }

    public bool DirLeft;
    public bool DirRight;

    public bool onEnemy = true;
    public bool onBoss = false;

    public float BulletSpeed = 10.0f;
    public int Player_HP = 100;

    public int EnemyCount = 0;
}
