using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeController : MonoBehaviour
{
    // ** 0 ~ 180
    [Range(0.0f, 360.0f)]
    public float Angle = 90.0f;

    // ** ������ ���� ����
    [Range(10, 72)]
    public int Segments = 30;

    // ** ������
    [Range(1.0f, 50.0f)]
    public float radius = 5.0f;

    public List<Vector3> PointList = new List<Vector3>();

    private MeshFilter meshFilter;
    private Mesh mesh;


    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
    }

    void Start()
    {
        Angle = 90.0f;
        Segments = 36;
        radius = 5.0f;

#if UNITY_EDITOR

#else

#endif
    }

    void Update()
    {
        // ** 0.0 ��ǥ�� �����ؾ� �ϱ� ������ Segments + 1�� ������ �ʱ�ȭ
        int PointCount = Segments + 1;

        // ** ���ؽ��� ������ŭ ����
        Vector3[] vertices = new Vector3[PointCount];

        // ** ù ��° ���Ұ��� 0.0f (���� ����)
        vertices[0] = Vector3.zero;

        // ** ���� Angle�� 180����� ���ذ����κ��� -90�� ��ġ���� �����ؾ� �ϹǷ�
        // ** (Angle * 0.5f)�� ���� ���ذ����� ���ش�.
        // ** Angle = 180 �̹Ƿ�, (180 * 0.5f) = 90
        // ** ���ذ��� = transform.eulerAngles.y
        // ** ��������� transform.eulerAngls.y - 90 ���� �� �� �ִ�.
        float deltaAngle = transform.eulerAngles.y - (Angle * 0.5f);

        // ** int i = 1 ���� �����ؾ� �Ѵ�.
        for (int i = 1; i < vertices.Length; ++i)
        {
            // ** ��ä���� �����ϴ� ���
            Vector3 point = new Vector3(
                Mathf.Sin(deltaAngle * Mathf.Deg2Rad),
                0.0f,
                Mathf.Cos(deltaAngle * Mathf.Deg2Rad));

            Ray ray = new Ray(transform.position, point.normalized);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, radius))
                vertices[i] = hit.point;
            else
                vertices[i] = point * radius;

            deltaAngle += Angle / (Segments - 1);
        }

        /* [�θ���]
         * �ϳ��� ���� �̷� �� �ִ� �ּ� ���� = ������
         * �ϳ��� �������� 3���� ���ؽ��� �ʿ��ϰ�,
         * ���ؽ��� ������ 3����� �� �� ���� �մ� ������ �ʿ��ϴ�.
         * ���� ��� ���ؽ��� ������ 4����� �� ���� �մ� �θ��� ������ 2���� �ʿ��ϰ�,
         * ���ؽ��� ������ 5����� 3���� �ﰢ���� ��������� ������ �θ��� ������ 3���� �ʿ��ϴ�.
         * 
         * ���� = [(���ؽ� ���� - 2) * 3]
         */
        int[] triangles = new int[(PointCount - 2) * 3];

        // ** ������ �ﰢ������ ����
        for(int i = 0; i < PointCount - 2; ++i)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // ** ���� �ﰢ���� ��ġ�� ���ԵǾ� �׷��� �� �ֱ� ������ �׻� ������� �Ѵ�.
        mesh.Clear();

        // ** ���� �޽��� ����
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // ** �޽��� ���Ϳ� ����
        meshFilter.mesh = mesh;


#if UNITY_EDITOR

#else

#endif
    }
}
