using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager
{
    // �ڵ��� ������ ���� �ʼ�!
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
}
