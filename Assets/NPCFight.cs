using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFight : MonoBehaviour
{
	[SerializeField]
	GameObject HitboxHandL, HitboxHandR, HitboxFootL, HitboxFootR;
	int rand;
	NPCMove move;
	bool isAttackingGate;
	Animator anim;
	int rand2;
	protected void Start()
	{
		move = this.GetComponent<NPCMove>();
		anim = this.GetComponent<Animator>();
	}
	public void takeDamage(){
		anim.SetBool("isTakingDamage", true);
		anim.SetBool("isAttacking", false);
		anim.SetBool("isHeavyAttack", false);
		anim.SetBool("isLightAttack", false);
		isAttackingGate = true;
		rand2 = Random.Range(0,4);
		if(rand2 == 0){
			anim.SetInteger("whichFightIdleDamageAnim", 1);
		}
		if(rand2 == 1){
			anim.SetInteger("whichFightIdleDamageAnim", 2);
		}
		if(rand2 == 2){
			anim.SetInteger("whichFightIdleDamageAnim", 3);
		}
		if(rand2 == 3){
			anim.SetInteger("whichFightIdleDamageAnim", 4);
		}
	}
	
	public void resetIsAttacking(){
		//Debug.Log("Reset Attacking!");
		anim.SetBool("isTakingDamage", false);
		anim.SetBool("isAttacking", false);
		anim.SetBool("isHeavyAttack", false);
		anim.SetBool("isLightAttack", false);
		isAttackingGate = false;
	}
	public void EnterFighting(GameObject Min){
		move.changeAgentSpeedToGiven(Min.gameObject.GetComponent<NPCMove>().agent.speed * .5f);
		anim.SetBool("isFighting", true);
		            
		//Debug.Log("Caught up to chasee", this.gameObject);
		                
		//enable this if you want the chaser to have a hard time catching the runner,
		//basically slows down the chaser and speeds up the chase if they get close enough
		                
		//Min.gameObject.GetComponent<NPCMove>().changeAgentSpeedToGiven(agent.speed * 1.5f);
		//changeAgentSpeedToGiven(Min.gameObject.GetComponent<NPCMove>().agent.speed * .75f);
		float dist = Vector3.Distance(this.transform.position, Min.transform.position);
		Vector3 dirToThreat = this.transform.position - Min.transform.position;
		dirToThreat.Normalize();
		Vector3 newPos = (transform.position + dirToThreat);
		Quaternion toRotation = Quaternion.LookRotation(-dirToThreat, Vector3.up);
		this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (move.turnRate) * Time.deltaTime);
	}
	public void StartAttack(){
		move.changeAgentSpeedToGiven(0f);
		rand = Random.Range(0,4);
		if(!isAttackingGate){
			anim.SetBool("isAttacking", true);
			isAttackingGate = true;
			Debug.Log("Attacking!");
			if(rand == 0){
				anim.SetInteger("WhichHeavyAttackAnim", 1);
				Debug.Log("Doing heavy attack!");
				anim.SetBool("isHeavyAttack", true);
				anim.SetBool("isLightAttack", false);
			}
			else if (rand == 1){
				anim.SetInteger("WhichLightAttackAnim", 1);
				Debug.Log("Doing light attack!");
				anim.SetBool("isLightAttack", true);
				anim.SetBool("isHeavyAttack", false);
			}
			else if (rand == 2){
				anim.SetInteger("WhichHeavyAttackAnim", 2);
				Debug.Log("Doing heavy attack #2!");
				anim.SetBool("isLightAttack", false);
				anim.SetBool("isHeavyAttack", true);
			}
			else if (rand == 3){
				anim.SetInteger("WhichLightAttackAnim", 2);
				Debug.Log("Doing light attack #2!");
				anim.SetBool("isLightAttack", true);
				anim.SetBool("isHeavyAttack", false);

			}


		}
		            
	}
	void EnableHitboxHandL(){
		HitboxHandL.SetActive(true);
	}
	void EnableHitboxHandR(){
		HitboxHandR.SetActive(true);
	}
	void EnableHitboxFootL(){
		HitboxFootL.SetActive(true);
	}
	void EnableHitboxFootR(){
		HitboxFootR.SetActive(true);
	}
	void DisableAllHitboxes(){
		HitboxHandL.SetActive(false);
		HitboxHandR.SetActive(false);
		HitboxFootL.SetActive(false);
		HitboxFootR.SetActive(false);
	}
}
