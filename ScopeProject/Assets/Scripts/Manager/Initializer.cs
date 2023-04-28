using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        // 활성화
        var prefabManager = PrefabManager.GetInstance;
        var objectPoolManager = ObjectPoolManager.GetInstance;
    }
}
