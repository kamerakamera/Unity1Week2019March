using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    public float posOffset = 4.3f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (player.transform.position.x + posOffset >= transform.position.x)
        {
            transform.position = new Vector3(player.transform.position.x + posOffset, 0, -10);
        }
    }

    public void SetStartPos()
    {
        transform.position = new Vector3(0, 0, -10);
        player = GameObject.Find("Player");
    }
}
