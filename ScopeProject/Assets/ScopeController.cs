using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeController : MonoBehaviour
{
    // ** 0 ~ 180
    [Range(0.0f, 180.0f)]
    public float Angle = 45.0f;

    // ** 가상의 선의 개수
    [Range(10, 360)]
    public int Segments = 30;

    // ** 반지름
    [Range(1.0f, 50.0f)]
    public float radius = 5.0f;

    public List<Vector3> PointList = new List<Vector3>();


    public List<GameObject> LeftPointList = new List<GameObject>();
    public List<GameObject> RightPointList = new List<GameObject>();

#if UNITY_EDITOR

#else

#endif

    void Start()
    {
        Angle = 45.0f;
        Segments = 30;
        radius = 5.0f;





#if UNITY_EDITOR  // 에디터 모드에서만 사용

#else

#endif
        //float angle = Angle / Segments;

        //for (int i = 0; i < Segments + 1; ++i)
        //{
        //    GameObject Object = new GameObject("EditorGizmo");

        //    MyGizmo gizmo = Object.AddComponent<MyGizmo>();

        //    Object.transform.position = new Vector3(
        //        Mathf.Sin(-(angle * i) * Mathf.Deg2Rad),
        //        0.0f,
        //        Mathf.Cos(-(angle * i) * Mathf.Deg2Rad)) * radius + transform.position;

        //    PointList.Add(Object.transform.position);
        //}
    }

    void Update()
    {
#if UNITY_EDITOR

        //print(transform.eulerAngles.y);

        //float angle = 360 / Segments;
        //for (int i = 0; i < Segments; ++i)
        //{
        //    PointList2[i].transform.position = new Vector3(
        //        Mathf.Cos((angle * i) * Mathf.Deg2Rad),
        //        Mathf.Sin((angle * i) * Mathf.Deg2Rad),
        //        0.0f) * radius + transform.position;

        //}

#else

#endif
    }
}
