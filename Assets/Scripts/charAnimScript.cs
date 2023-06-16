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
        	blendTwoFloats(speedometer / move.runSpeed, 2f);
        }

    }
	void blendTwoFloats(float goal, float rate){
		if(Mathf.Approximately(Mathf.Round(anim.GetFloat("speed") * 10.0f) * 0.1f, Mathf.Round(goal * 10.0f) * 0.1f)){
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
	//compare two things, if greater, slowlyincrease with time.delta time. if less, slowly decreae
}
