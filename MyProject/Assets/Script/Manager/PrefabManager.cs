using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public interface test//<T>
{
    public void GetObjects();

    // ** 데이터 저장소(repository)
    //private Dictionary<T, GameObject> prototypeObjectList = new Dictionary<T, GameObject>();
}
*/

public class PrefabManager //where<string, ...> as :
{
    // ** 인스턴스 생성
    public static PrefabManager GetInstance { get; } = new PrefabManager();

    // ** 데이터 저장소(repository)
    private Dictionary<string, GameObject> prototypeObjectList = new Dictionary<string, GameObject>();

    private PrefabManager()
    {
        // ** 데이터를 불러온다.
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");  // list 여러 개로?

        // ** 불러온 데이터를 Dictionary에 보관
        foreach (GameObject element in prefabs)
            prototypeObjectList.Add(element.name, element);

        //AAA a = new BBB();
        //a.GetObjects();
    }

    // ** 외부에서 보관 중인 Prefab을 참조할 수 있도록 Getter를 제공
    public GameObject GetPrefabByName(string key)
    {
        // ** 만약에 key가 존재한다면 원형 객체를 반환하고,
        if (prototypeObjectList.ContainsKey(key))
            return prototypeObjectList[key];

        // ** 그렇지 않을 때에는 null을 반환한다.
        return null;
    }
}

/*
public abstract class AAA : test
{
    public virtual void GetObjects()
    {

    }
}

public class BBB : AAA
{
    public override void GetObjects()
    {

    }
}
*/
