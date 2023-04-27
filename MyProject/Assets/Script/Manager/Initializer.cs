using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        var prefabManager = PrefabManager.GetInstance;  // 활성화
    }
}
