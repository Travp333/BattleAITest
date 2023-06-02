using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleTestNPCAnimController : MonoBehaviour
{
	Animator anim;
	Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
	    anim = GetComponent<Animator>();
	    body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
	    anim.SetFloat("motion z", body.velocity.magnitude);
	    anim.SetFloat("motion x", body.velocity.x);
    }
}
