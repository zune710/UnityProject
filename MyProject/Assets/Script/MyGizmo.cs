using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // ** Gizmos�� ���� �����Ѵ�.
        Gizmos.color = Color.red;

        // ** Gizmos�� �׸���.
        Gizmos.DrawSphere(this.transform.position, 0.2f);
    }
}
