using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFight : MonoBehaviour
{
	// make it so they cant move while attacking?
	// make them back up a bit
	// make them walk around and rotate around eachother essentially 
	// apply knockback when taking damage
	// implement dodging 
	// implement blocking 
	// implement ranged attackers
	NPCBehaviorChangersList list;
	[SerializeField]
	int hp = 100;
	[SerializeField]
	GameObject HitboxHandL, HitboxHandR, HitboxFootL, HitboxFootR;
	int rand;
	NPCMove move;
	Animator anim;
	int rand2;
	bool injuredGate;
	public bool invulnerabilityPeriod;
	public float invulnerabilityTimer = 3f;
	public bool attackCooldown;
	public float attackCooldownTimer = 3f;
	[SerializeField]
	float knockBackForce = 250f;
	Rigidbody body;
	protected void Start()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("EmptyScriptHolders") ){
            if(g.GetComponent<NPCBehaviorChangersList>()!=null){
                list = g.GetComponent<NPCBehaviorChangersList>();
            }
        }
		move = this.GetComponent<NPCMove>();
		anim = this.GetComponent<Animator>();
		body = this.GetComponent<Rigidbody>();
	}
	public void takeDamage(Vector3 dir){
		invulnerabilityPeriod = true;
		invulnerabilityTimer = 3f;
		attackCooldown = true;
		attackCooldownTimer = 3f;
		if(hp - 10 < 0){
			hp = 0;
		}
		else {
			hp = hp - 10;
		}

		if(hp == 0){
			list.removeFromNPCS(this.gameObject);
			Destroy(this.gameObject);
		}
		anim.SetBool("isTakingDamage", true);
		anim.SetBool("isAttacking", false);
		anim.SetBool("isHeavyAttack", false);
		anim.SetBool("isLightAttack", false);
		rand2 = Random.Range(0,4);
		if(rand2 == 0){
			anim.Play("fighting idle take damage");
			body.AddForce(dir * knockBackForce);
			//anim.SetInteger("whichFightIdleDamageAnim", 1);
		}
		if(rand2 == 1){
			anim.Play("fighting idle take damage 1");
			body.AddForce(dir * knockBackForce);
			//anim.SetInteger("whichFightIdleDamageAnim", 2);
		}
		if(rand2 == 2){
			anim.Play("fighting idle take damage 2");
			body.AddForce(dir * knockBackForce);
			//anim.SetInteger("whichFightIdleDamageAnim", 3);
		}
		if(rand2 == 3){
			anim.Play("fighting idle take damage 3");
			body.AddForce(dir * knockBackForce);
			//anim.SetInteger("whichFightIdleDamageAnim", 4);
		}
	}
	
	public void resetIsAttacking(){
		Debug.Log("Reset Attacking!");
		anim.SetBool("isTakingDamage", false);
		anim.SetBool("isAttacking", false);
		anim.SetBool("isHeavyAttack", false);
		anim.SetBool("isLightAttack", false);
	}
	public void EnterFighting(GameObject Min){
		//need to check in min, that being the npc this npc is targeting, to see if it is scared. If so, then we dont need the movement decrease on the attacker, just let them take em out
		move.changeAgentSpeedToGiven(this.gameObject.GetComponent<NPCMove>().defaultSpeed * .5f);
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
		if(!attackCooldown){
			anim.SetBool("isAttacking", true);
			Debug.Log("Attacking!");
			if(rand == 0){
				attackCooldown = true;
				attackCooldownTimer = 5f;
				//anim.Play("heavy attack");
				anim.SetInteger("WhichHeavyAttackAnim", 1);
				Debug.Log(this.gameObject.name + " is doing heavy attack!");
				anim.SetBool("isHeavyAttack", true);
				anim.SetBool("isLightAttack", false);
			}
			else if (rand == 1){
				attackCooldown = true;
				attackCooldownTimer = 3f;
				//anim.Play("light attack");
				anim.SetInteger("WhichLightAttackAnim", 1);
				Debug.Log(this.gameObject.name + " is doing light attack!");
				anim.SetBool("isLightAttack", true);
				anim.SetBool("isHeavyAttack", false);
			}
			else if (rand == 2){
				attackCooldown = true;
				attackCooldownTimer = 5f;
				//anim.Play("heavy attack alt");
				anim.SetInteger("WhichHeavyAttackAnim", 2);
				Debug.Log(this.gameObject.name + " is doing heavy attack #2!");
				anim.SetBool("isLightAttack", false);
				anim.SetBool("isHeavyAttack", true);
			}
			else if (rand == 3){
				attackCooldown = true;
				attackCooldownTimer = 3f;
				//anim.Play("light attack variant");
				anim.SetInteger("WhichLightAttackAnim", 2);
				Debug.Log(this.gameObject.name + " is doing light attack #2!");
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
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(attackCooldown){
			attackCooldownTimer = attackCooldownTimer - Time.deltaTime;
			if(attackCooldownTimer <= 0f){
				attackCooldown = false;
				attackCooldownTimer = 0f;
			}
		}
		if(invulnerabilityPeriod){
			invulnerabilityTimer = invulnerabilityTimer - Time.deltaTime;
			if(invulnerabilityTimer <= 0f){
				invulnerabilityPeriod = false;
				invulnerabilityTimer = 0f;
			}
		}
		if(!injuredGate){
			if (hp <= 10 && !move.brave){
				injuredGate = true;
				anim.SetBool("isFighting", false);
				move.runner = true;
				move.chaser = false;
				move.runSpeed = move.runSpeed/2;
				move.defaultSpeed = move.defaultSpeed/2;
			}
		}
	}
}
