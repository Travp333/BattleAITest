using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charAnimScript : MonoBehaviour
{
    NPCMove move;
    Animator anim;
    [SerializeField]
    [Tooltip("How fast this npc is currently moving, dont edit")]
    float speedometer;
    [SerializeField]
    float sprintCutoff;
    [SerializeField]
	float idleCutoff = .01f;
	float speedUpdateTime;
	float speedUpdateCap = 1f;
	float currentSpeed;
	float t;
	bool isBlending;
	float speedBlendTime = 1f;
	float speedLerp;
	Vector3 tempScale;
    // Start is called before the first frame update
    void Start()
	{
		tempScale = transform.localScale;
        move = this.gameObject.GetComponent<NPCMove>();
	    anim = this.gameObject.GetComponent<Animator>();
	    Invoke("SetSprintCutoff", .5f);
    }
	void SetSprintCutoff(){
		sprintCutoff = move.runSpeed - 1f;
	}
	
    // Update is called once per frame
    void Update()
    {        
	    speedometer = move.agent.velocity.magnitude;
        if(speedometer / move.runSpeed < .01f){
            //Not Moving
            anim.SetFloat("speed", 0);
        }
        else if(speedometer / move.runSpeed > 1){
	        //over max, cap at 1
	        anim.SetFloat("speed", 1);
        }
        else{
        	anim.SetFloat("speed", speedometer / move.runSpeed);
        }

    }
}
