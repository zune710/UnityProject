using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ScopeController))]
public class ScopeEditor : Editor  // Editor: component 내부를 변경  /  EditorWindow: Editor를 edit
{
    // 변경사항 생기면 유니티 환경에 즉시 적용
    //private void OnSceneGUI()
    //{
    //    ScopeController targetComponent = (ScopeController)target;

    //    Handles.DrawWireArc(targetComponent.transform.position, Vector3.up, Vector3.forward, 360.0f, targetComponent.radius);

    //    float Segments = targetComponent.Angle / targetComponent.Segments;

    //    for (int i = 0; i < targetComponent.Segments + 1; ++i)
    //    {
    //        Vector3 angleLeftPoint = new Vector3(
    //            Mathf.Sin(-(Segments * i) * Mathf.Deg2Rad),
    //            0.0f,
    //            Mathf.Cos(-(Segments * i) * Mathf.Deg2Rad));

    //        Vector3 angleRightPoint = new Vector3(
    //            Mathf.Sin((Segments * i) * Mathf.Deg2Rad),
    //            0.0f,
    //            Mathf.Cos((Segments * i) * Mathf.Deg2Rad));

    //        Handles.DrawLine(targetComponent.transform.position,
    //            targetComponent.transform.position + angleLeftPoint * targetComponent.radius);

    //        Handles.DrawLine(targetComponent.transform.position,
    //            targetComponent.transform.position + angleRightPoint * targetComponent.radius);
    //    }
    //}
}
