using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Creature
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

}
