using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocks : MonoBehaviour
{
    [SerializeField]
    GameObject followObj;//参照する群れ
    Vector3 nextPos;
    Quaternion nextRot;
    float waitTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetFlocksPos();
        StartCoroutine(CallFlocks(nextPos, nextRot));

    }

    void GetFlocksPos()
    {
        if (followObj == null)
        {
            Destroy(this.gameObject);
            return;
        }
        nextPos = followObj.transform.position;
        nextRot = followObj.transform.rotation;
    }

    public void SetFollowObj(GameObject obj)
    {
        followObj = obj;
    }

    void SetFlocksPos(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    private IEnumerator CallFlocks(Vector3 pos, Quaternion rot)
    {
        yield return new WaitForSeconds(waitTime);
        SetFlocksPos(pos, rot);
        yield return null;
    }
}
