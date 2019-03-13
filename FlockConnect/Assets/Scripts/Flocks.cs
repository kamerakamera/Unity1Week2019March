using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocks : MonoBehaviour
{
    [SerializeField]
    GameObject followObj;//参照する群れ
    Vector3 nextPos;
    float waitTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetFlocksPos();
        StartCoroutine("CallFlocks",nextPos);
        
    }

    void GetFlocksPos()
    {
        nextPos = followObj.transform.position;
    }

    public void SetFollowObj(GameObject obj){
        followObj = obj;
    }

    void SetFlocksPos(Vector3 pos)
    {
        transform.position = pos;
        
    }

    private IEnumerator CallFlocks(Vector3 pos){
        yield return new WaitForSeconds(waitTime);
        SetFlocksPos(pos);
        yield return null;
    }
}
