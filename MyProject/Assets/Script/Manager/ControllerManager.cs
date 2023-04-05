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
    public bool BossActive = false;

    public float BulletSpeed = 10.0f;
    
    public int Player_HP = 100;

    public int Heart = 3;
    public int EnemyCount = 0;

    public int EnemyId = 1;
    public int BossId = 1;

    public int Round = 1;
    public int Goal = 1; // Enemy 처치 수(목표) 20 (10씩 증가), ButtonController에서도 변경해야 함

    public bool GoalClear = false;
    public bool BossClear = false;

    public bool GameOver = false;
    public bool GameClear = false;

    public bool LoadGame = false;
}
