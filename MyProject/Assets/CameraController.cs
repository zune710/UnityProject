using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ������ ȣ��� �� �ʱ�ȭ��
    // ** ī�޶��� ���� �ð�
    private float shakeTime = 0.15f;

    //** ī�޶��� ���� ����
    private Vector3 offset = new Vector3(0.15f, 0.15f, 0.0f);
    private Vector3 OldPosition;
    
    // ** �ڷ�ƾ �Լ� ����
    // IEnumerator: Coroutine �Լ�(�������� �����ϴ� �Լ�, ����� ���� �Լ�)
    IEnumerator Start()
    {
        // ** ī�޶��� ����ȿ���� �ֱ� �� ī�޶� ��ġ�� �޾ƿ´�.
        OldPosition = new Vector3(0.0f, -1.9f, -10.0f);
        
        // ** 0.15�� ���� ����
        while (shakeTime > 0.0f)
        {
            shakeTime -= Time.deltaTime;

            // ** �ݺ����� ����Ǵ� ���� �ݺ������� ȣ��
            // null�� �ָ� �⺻ ������Ÿ��(deltaTime)�� ���´�. deltaTime��ŭ �ڷ� ���ٰ� �ٽ� �� ��ġ�� ���ƿͼ� �Ʒ� �̾ ����. 
            yield return null;
            // ��) yield return new WaitForSeconds(2.0f); // 2�� �� �ݺ� - ����ڰ� ������ ���ϴ� ��� ���� �� �ִ�. 

            // ** ī�޶� ���� ������ŭ ������Ų��.
            Camera.main.transform.position = new Vector3(
            Random.Range(OldPosition.x - offset.x, OldPosition.x + offset.x),
            Random.Range(OldPosition.y - offset.y, OldPosition.y + offset.y),
            -10.0f);
        }

        // ** �ݺ����� ����Ǹ� ī�޶� ��ġ�� �ٽ� ������ ���´�.
        Camera.main.transform.position = OldPosition;
        
        // ** Ŭ������ �����Ѵ�.
        Destroy(this.gameObject);
    }
}
