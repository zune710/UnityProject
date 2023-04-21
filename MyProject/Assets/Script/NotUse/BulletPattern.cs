using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour
{
    public enum Pattern
    {
        Screw, 
        DelayScrew,
        Twist,
        D, 
        Explosion, 
        F, 
        GuideBullet
    };

    public Pattern pattern;
    public Sprite sprite;

    private List<GameObject> BulletList = new List<GameObject>();
    private GameObject BulletPrefab;


    void Start()
    {
        BulletPrefab = Resources.Load("Prefabs/PatternBullet") as GameObject;

        switch (pattern)
        {
            case Pattern.Screw:
                GetScrewPattern(5.0f, (int)(360 / 5.0f));
                break;

            case Pattern.DelayScrew:
                StartCoroutine(GetDelayScrewPattern());
                break;

            case Pattern.Twist:
                StartCoroutine(TwistPattern());
                break;

            case Pattern.D:

                break;

            case Pattern.Explosion:
                StartCoroutine(ExplosionPattern(5.0f, (int)(360 / 5.0f)));
                break;

            case Pattern.F:

                break;

            case Pattern.GuideBullet:
                GuideBulletPattern();
                break;
        }
    }

    private void GetScrewPattern(float _angle, int _count, bool _option = false)
    {
        for(int i = 0; i < _count; ++i)
        {
            GameObject Obj = Instantiate(BulletPrefab);
            BulletControl controller = Obj.GetComponent<BulletControl>();

            controller.Option = _option;

            _angle += 5.0f;

            controller.Direction = new Vector3(
                Mathf.Cos(_angle * 3.141592f / 180),
                Mathf.Sin(_angle * 3.141592f / 180),
                0.0f) * 5 + transform.position;
            // radian = 180 / pi
            // 각도 = pi / 180

            Obj.transform.position = transform.position;

            BulletList.Add(Obj);
        }
    }

    private IEnumerator GetDelayScrewPattern()
    {
        float fAngle = 30.0f;

        int iCount = (int)(360 / fAngle);

        int i = 0;

        while(i < (iCount) * 3)
        {
            GameObject Obj = Instantiate(BulletPrefab);
            BulletControl controller = Obj.GetComponent<BulletControl>();

            controller.Option = false;

            fAngle += 30.0f;

            

            controller.Direction = new Vector3(
                Mathf.Cos(fAngle * Mathf.Deg2Rad), // Mathf.Deg2Rad == 3.141592f / 180
                Mathf.Sin(fAngle * Mathf.Deg2Rad),
                0.0f) * 5 + transform.position;
            // radian = 180 / pi
            // 각도 = pi / 180

            Obj.transform.position = transform.position;

            BulletList.Add(Obj);
            
            ++i;
            
            yield return new WaitForSeconds(0.025f);
        }
    }

    public IEnumerator TwistPattern()
    {
        float fTime = 3.0f;

        while(fTime > 0)
        {
            fTime -= Time.deltaTime;

            GameObject Obj = Instantiate(Resources.Load("Prefabs/Twist")) as GameObject;

            yield return null;
        }
    }

    public IEnumerator ExplosionPattern(float _angle, int _count, bool _option = false)
    {
        GameObject ParentObj = new GameObject("Bullet");

        SpriteRenderer renderer = ParentObj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        BulletControl control = ParentObj.AddComponent<BulletControl>();

        control.Option = false;

        control.Direction = GameObject.Find("Target").transform.position - transform.position;

        ParentObj.transform.position = transform.position;

        yield return new WaitForSeconds(1.5f);

        Vector3 pos = ParentObj.transform.position;

        Destroy(ParentObj);

        for (int i = 0; i < _count; ++i)
        {
            GameObject Obj = Instantiate(BulletPrefab);

            BulletControl controller = Obj.GetComponent<BulletControl>();

            controller.Option = _option;

            _angle += 5.0f;

            controller.Direction = new Vector3(
                Mathf.Cos(_angle * 3.141592f / 180),
                Mathf.Sin(_angle * 3.141592f / 180),
                0.0f) * 5 + transform.position;

            Obj.transform.position = pos;

            BulletList.Add(Obj);
        }
    }

    public void GuideBulletPattern()
    {
        GameObject Obj = Instantiate(BulletPrefab);
        BulletControl controller = Obj.GetComponent<BulletControl>();

        controller.Target = GameObject.Find("Target");
        controller.Option = true;

        Obj.transform.position = transform.position;
    }
}