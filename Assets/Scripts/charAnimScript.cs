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
	float speedUpdateCap = 0.1f;
	float currentSpeed;
	float t;
	bool isBlending;
	float speedBlendTime = 1f;
	float speedLerp;
    // Start is called before the first frame update
    void Start()
    {
        move = this.gameObject.GetComponent<NPCMove>();
	    anim = this.gameObject.GetComponent<Animator>();
	    Invoke("SetSprintCutoff", .5f);
    }
	void SetSprintCutoff(){
		sprintCutoff = move.runSpeed - 1f;
	}
	
	void BlendFloats(float float1, float time){
		isBlending = true;
		Debug.Log("Enabled blend Lock!");
		speedLerp = Mathf.Lerp(currentSpeed, float1, t);
		anim.SetFloat("speed", speedLerp);
		//Debug.Log("blending from " + currentSpeed + " to " + float1 + " , We are at " + speedLerp + " / " + float1);
		t += Time.deltaTime;
		if(t >= time){
			Debug.Log("Disabled Blend Lock!");
			isBlending = false;
			return;
		}
		else{
			BlendFloats(float1, time);
		}
	}
	
	
	void BlendFloats2(){
		if(anim.GetFloat("speed") < speedometer){
			anim.SetFloat("speed", anim.GetFloat("speed") + Time.deltaTime ); 
			BlendFloats2
		}
	}
	

    // Update is called once per frame
    void Update()
    {        
	    speedometer = move.agent.velocity.magnitude;
	    speedUpdateTime += Time.deltaTime;
	    
	    if(speedUpdateTime >= speedUpdateCap){
		    if(speedometer/move.runSpeed >= .99){
		    	if(!isBlending){
			    	currentSpeed = anim.GetFloat("speed");
			    	BlendFloats(1, speedBlendTime);
		    	}
		    }
		    else if (speedometer/move.runSpeed <= 0){
		    	if(!isBlending){
			    	currentSpeed = anim.GetFloat("speed");
			    	BlendFloats(0, speedBlendTime);
		    	}
		    }
		    else{
		    	
		    	if(anim.GetFloat("speed") < speedometer){
		    		BlendFloats2();
		    	}
		    	
		    	
		    	if(!isBlending){
		    		currentSpeed = anim.GetFloat("speed");
			    	BlendFloats(speedometer/move.runSpeed, speedBlendTime);
		    	}
		    }
		    speedUpdateTime = speedUpdateTime - speedUpdateCap;
	    }
        

	    
	    
	    
	    
	    
        if(speedometer < .0001f){
            //Not Moving
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            anim.SetFloat("speed", 0);
        }
        else if(speedometer < sprintCutoff && speedometer > idleCutoff){
            //Walking Speed
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);

        }
        else if(speedometer > sprintCutoff){
            //Running Speed
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }

    }
}
