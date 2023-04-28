using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject Parent = null;
    private string EnemyName = "Enemy";

    private void Awake()
    {
        Parent = new GameObject("ObjectList");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            ObjectPoolManager.GetInstance.getObject(EnemyName).transform.SetParent(Parent.transform); ;
    }
}
