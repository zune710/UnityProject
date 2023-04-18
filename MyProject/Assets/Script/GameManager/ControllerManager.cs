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

    // RoundData
    public int Round = 1;
    public int Goal = 20; // Enemy 처치 수(목표) 20 (10씩 증가)
    public bool onEnemy = true;
    public bool onBoss = false;
    public bool BossActive = false;
    
    // PlayerData
    public int Player_HP = 100;
    public float BulletSpeed = 10.0f;
    public int Heart = 3;
    public int EnemyCount = 0;

    // EnemyData
    public int EnemyId = 1;
    public int BossId = 0;  // 보스전 시작 시 증가하기 때문

    // GameData
    public bool LoadGame = false;
    public bool GoalClear = false;
    public bool BossClear = false;
    public bool GameOver = false;
    public bool GameClear = false;

}
