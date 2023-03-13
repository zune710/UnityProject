using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager
{
    private static ControllerManager Instance = null;

    public static ControllerManager GetInstance()
    {
        if (Instance == null)
            Instance = new ControllerManager();
        return Instance;
    }

    public bool DirLeft;
    public bool DirRight;

    // 코드의 안정성 위해 필수!
    private ControllerManager()
    {

    }
}
