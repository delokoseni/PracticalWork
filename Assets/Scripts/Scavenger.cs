using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenger : Creature
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


}
