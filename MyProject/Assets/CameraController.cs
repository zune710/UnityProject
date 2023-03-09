using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 생성자 호출될 때 초기화됨
    // ** 카메라의 진동 시간
    private float shakeTime = 0.15f;

    //** 카메라의 진동 범위
    private Vector3 offset = new Vector3(0.15f, 0.15f, 0.0f);
    private Vector3 OldPosition;
    
    // ** 코루틴 함수 실행
    // IEnumerator: Coroutine 함수(순서들을 무시하는 함수, 사용자 정의 함수)
    IEnumerator Start()
    {
        // ** 카메라의 진동효과를 주기 전 카메라 위치를 받아온다.
        OldPosition = new Vector3(0.0f, -1.9f, -10.0f);
        
        // ** 0.15초 동안 실행
        while (shakeTime > 0.0f)
        {
            shakeTime -= Time.deltaTime;

            // ** 반복문이 실행되는 동안 반복적으로 호출
            // null을 주면 기본 프레임타임(deltaTime)을 갖는다. deltaTime만큼 자러 갔다가 다시 이 위치로 돌아와서 아래 이어서 실행. 
            yield return null;
            // 예) yield return new WaitForSeconds(2.0f); // 2초 뒤 반복 - 사용자가 루프를 원하는 대로 정할 수 있다. 

            // ** 카메라를 진동 범위만큼 진동시킨다.
            Camera.main.transform.position = new Vector3(
            Random.Range(OldPosition.x - offset.x, OldPosition.x + offset.x),
            Random.Range(OldPosition.y - offset.y, OldPosition.y + offset.y),
            -10.0f);
        }

        // ** 반복문이 종료되면 카메라 위치를 다시 원점에 놓는다.
        Camera.main.transform.position = OldPosition;
        
        // ** 클래스를 종료한다.
        Destroy(this.gameObject);
    }
}
