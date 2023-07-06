using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charAnimScript : MonoBehaviour
{
    NPCMove move;
    Animator anim;
    [SerializeField]
    [Tooltip("How fast this npc is currently moving, dont edit")]
	float speedometer, speedometerX, speedometerZ;
    [SerializeField]
    float sprintCutoff;
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
	    speedometerX = move.agent.velocity.x;
	    speedometerZ = move.agent.velocity.z;
	    if(speedometer / move.runSpeed < .01f && speedometer / move.runSpeed > .01f){
            //Not Moving
	        anim.SetFloat("speed", 0);
	        anim.SetFloat("motion x", 0);
	        anim.SetFloat("motion z", 0);
        }
        else if(speedometer / move.runSpeed > 1){
	        //over max, cap at 1
	        anim.SetFloat("speed", 1);
        }
        else if(speedometerX / move.runSpeed > 1){
	        //over max, cap at 1
	        anim.SetFloat("motion x", 1);
        }
        else if(speedometerZ / move.runSpeed > 1){
	        //over max, cap at 1
	        anim.SetFloat("motion z", 1);
        }
        else if(speedometer / move.runSpeed < -1){
	        //under min, cap at -1
	        anim.SetFloat("speed", -1);
        }
        else if(speedometerX / move.runSpeed < -1){
	        //under min, cap at -1
	        anim.SetFloat("motion x", -1);
        }
        else if(speedometerZ / move.runSpeed < -1){
	        //under min, cap at -1
	        anim.SetFloat("motion z", -1);
        }
        else{
        	blendSpeed(speedometer / move.runSpeed, 2f);
        	blendX(speedometerX / move.runSpeed, 2f);
        	//blendZ(speedometerZ / move.runSpeed, 2f);
        }

    }
    
	//basically just blends the starting value to a given value at a given rate
	void blendSpeed(float goal, float rate){
		if(Mathf.Approximately(Mathf.Round(anim.GetFloat("speed") * 100.0f) * 0.01f, Mathf.Round(goal * 100.0f) * 0.01f)){
			//Debug.Log("Close enough, matching speeds");
			//anim.SetFloat("speed", goal);
			return;
		}
		else if(anim.GetFloat("speed") > goal){
			anim.SetFloat("speed", anim.GetFloat("speed") - Time.deltaTime / rate);
		}
		else if (anim.GetFloat("speed") < goal){
			anim.SetFloat("speed", anim.GetFloat("speed") + Time.deltaTime / rate);
		}

	}
	
	void blendX(float goal, float rate){
		if(Mathf.Approximately(Mathf.Round(anim.GetFloat("motion x") * 100.0f) * 0.01f, Mathf.Round(goal * 100.0f) * 0.01f)){
			//Debug.Log("Close enough, matching speeds");
			//anim.SetFloat("speed", goal);
			return;
		}
		else if(anim.GetFloat("motion x") > goal){
			anim.SetFloat("motion x", anim.GetFloat("motion x") - Time.deltaTime / rate);
		}
		else if (anim.GetFloat("motion x") < goal){
			anim.SetFloat("motion x", anim.GetFloat("motion x") + Time.deltaTime / rate);
		}

	}
	void blendZ(float goal, float rate){
		if(Mathf.Approximately(Mathf.Round(anim.GetFloat("motion z") * 100.0f) * 0.01f, Mathf.Round(goal * 100.0f) * 0.01f)){
			//Debug.Log("Close enough, matching speeds");
			//anim.SetFloat("speed", goal);
			return;
		}
		else if(anim.GetFloat("motion z") > goal){
			anim.SetFloat("motion z", anim.GetFloat("motion z") - Time.deltaTime / rate);
		}
		else if (anim.GetFloat("motion z") < goal){
			anim.SetFloat("motion z", anim.GetFloat("motion z") + Time.deltaTime / rate);
		}

	}
}
