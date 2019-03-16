using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maguro : Enemy
{
    Vector2 rushPower = new Vector2(-6, 0);
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Rush();
    }

    void Rush()
    {
        rb.velocity = rushPower;
    }
}
