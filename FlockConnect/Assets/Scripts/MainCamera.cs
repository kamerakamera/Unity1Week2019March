﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]GameObject player;
    private float posOffset = 4.3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x + posOffset >= transform.position.x){
            transform.position = new Vector3(player.transform.position.x + posOffset,0,-10);
        }
    }
}