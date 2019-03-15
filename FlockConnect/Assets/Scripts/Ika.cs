using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ika : Enemy
{
    // Start is called before the first frame update
    Vector2 rushPower = new Vector2(0, 10);
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
