using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager
{
    public static ObjectPoolManager GetInstance { get; } = new ObjectPoolManager();

    private ObjectPoolManager() { }

    //private Stack<GameObject> DisableList = new Stack<GameObject>();  // simple
    
    private Dictionary<string, Stack<GameObject>> DisableList = new Dictionary<string, Stack<GameObject>>();


    public GameObject getObject(string key)
    {
        Stack<GameObject> stack = null;
        GameObject prefab = null;    
        
        if(DisableList.TryGetValue(key, out stack) && stack.Count > 0)  // stack 값을 먼저 받아야 하므로 stack.Count > 0 이 뒤에 와야 함
            prefab = stack.Pop();
        else
        {
            prefab = PrefabManager.GetInstance.GetPrefabByName(key);

            if (prefab == null)
                return null;

            prefab.name = key;
        }

        prefab.SetActive(true);

        // ** 초기화
        //prefab.GetComponent<EnemyController>().Initialize();  // 기존 데이터 초기화
        
        return prefab;
    }

    public void returnObject(GameObject Obj)
    {
        if(DisableList.ContainsKey(Obj.name))
            DisableList[Obj.name].Push(Obj);
        else
        {
            Stack<GameObject> stack = new Stack<GameObject>();

            stack.Push(Obj);
            DisableList.Add(Obj.name, stack);
        }
    }
}
