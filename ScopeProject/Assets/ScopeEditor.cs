using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ScopeController))]
public class ScopeEditor : Editor  // Editor: component ���θ� ����  /  EditorWindow: Editor�� edit
{
    // ������� ����� ����Ƽ ȯ�濡 ��� ����
    private void OnSceneGUI()
    {
        ScopeController targetComponent = (ScopeController)target;

        Handles.DrawWireArc(targetComponent.transform.position, Vector3.up, Vector3.forward, 360.0f, targetComponent.radius);


        float Segments = targetComponent.Angle / targetComponent.Segments;

        for (int i = 0; i < targetComponent.Segments + 1; ++i)
        {
            Vector3 angleLeftPoint = new Vector3(
                Mathf.Sin(-(Segments * i) * Mathf.Deg2Rad),
                0.0f,
                Mathf.Cos(-(Segments * i) * Mathf.Deg2Rad));

            Vector3 angleRightPoint = new Vector3(
                Mathf.Sin((Segments * i) * Mathf.Deg2Rad),
                0.0f,
                Mathf.Cos((Segments * i) * Mathf.Deg2Rad));

            Handles.DrawLine(targetComponent.transform.position,
                targetComponent.transform.position + angleLeftPoint * targetComponent.radius);

            Handles.DrawLine(targetComponent.transform.position,
                targetComponent.transform.position + angleRightPoint * targetComponent.radius);



            targetComponent.LeftPointList[i].transform.position = targetComponent.transform.position + angleLeftPoint * targetComponent.radius;
            targetComponent.RightPointList[i].transform.position = targetComponent.transform.position + angleRightPoint * targetComponent.radius;
        }
    }

    private void OnEnable()
    {
        GameObject gizmoPrefab = Resources.Load("EditorGizmo") as GameObject;

        ScopeController targetComponent = (ScopeController)target;
        float Segments = targetComponent.Angle / targetComponent.Segments;

        for (int i = 0; i < targetComponent.LeftPointList.Count; ++i)
        {
            DestroyImmediate(targetComponent.LeftPointList[i]);
            DestroyImmediate(targetComponent.RightPointList[i]);
        }

        targetComponent.LeftPointList.Clear();
        targetComponent.RightPointList.Clear();

        for (int i = 0; i < targetComponent.Segments + 1; ++i)
        {
            targetComponent.LeftPointList.Add(Instantiate(gizmoPrefab));
            targetComponent.RightPointList.Add(Instantiate(gizmoPrefab));

            targetComponent.LeftPointList[i].transform.position = new Vector3(
                    Mathf.Sin(-(Segments * i) * Mathf.Deg2Rad),
                    0.0f,
                    Mathf.Cos(-(Segments * i) * Mathf.Deg2Rad)) * targetComponent.radius + targetComponent.transform.position;

            targetComponent.RightPointList[i].transform.position = new Vector3(
                   Mathf.Sin((Segments * i) * Mathf.Deg2Rad),
                   0.0f,
                   Mathf.Cos((Segments * i) * Mathf.Deg2Rad)) * targetComponent.radius + targetComponent.transform.position;
        }
    }
}
