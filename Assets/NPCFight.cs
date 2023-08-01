using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFight : MonoBehaviour
{
	// make them back up a bit
	// make them walk around and rotate around eachother essentially (maybe only when blocking?)
	// implement dodging 
	// implement blocking 
	// implement ranged attackers
	// implement ragdolls
	// uppercut knocks enemies in the air?\
	// different attacks depending on range? uppercut misses a lot
	// REDO ANIMATIONS FOR KICK AND UPPERCUT THEY BOTH MISS A LOT
	// add a "onGround" check, this will be tough but try and recreate the logic from catlike. This would be very helpful for air launches and such
	// in chivalry, ranged agents will switch to a weaker close range attack when pressed. Maybe here theres a chance they will do that or run away, dependent on if theyre a coward or not. same roll as running away when low health
	// im thinking that the reverse can be true of agents here, everyone has a few ranged attacks. This way when an agent is scared for their life and running away they can easily hit them with a ranged attack
	NPCBehaviorChangersList list;
	[SerializeField]
	[Tooltip("Health")]
	int hp = 100;
	[SerializeField]
	[Tooltip("Hitboxes")]
	GameObject HitboxHandL, HitboxHandR, HitboxFootL, HitboxFootR;
	int rand;
	NPCMove move;
	Animator anim;
	int rand2;
	bool injuredGate;
	public bool invulnerabilityPeriod;
	public float invulnerabilityTimer = 0f;
	public bool attackCooldown;
	public float attackCooldownTimer = 3f;
	[SerializeField]
	[Tooltip("how far back an agent gets knocked when taking damage (should probably flip this so it depends on the attack rather than the victim)")]
	float knockBackForce = 250f;
	Rigidbody body;
	NavMeshAgent agent;
	bool agentDisable;
	[SerializeField]
	[Tooltip("amount of time the AI agent is disabled when taking damage")]
	float agentDisableTimerCap;
	float agentDisableTimer;
	Quaternion rotation;
	protected void Start()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("EmptyScriptHolders") ){
            if(g.GetComponent<NPCBehaviorChangersList>()!=null){
                list = g.GetComponent<NPCBehaviorChangersList>();
            }
        }
		agent = this.GetComponent<NavMeshAgent>();
		move = this.GetComponent<NPCMove>();
		anim = this.GetComponent<Animator>();
		body = this.GetComponent<Rigidbody>();
	}
	public void takeDamage(Vector3 dir){
		invulnerabilityPeriod = true;
		invulnerabilityTimer = 0f;
		attackCooldown = true;
		attackCooldownTimer = 3f;
		//===================
		//this was an attempt at making agents turn off their AI when damaged, to allow for falling off cliffs and knockup damage
		//agent.updatePosition = false;
		//agent.updateRotation = false;
		//agent.enabled = false;
		//rotation = this.transform.rotation;
		//agentDisable = true;
		//agentDisableTimer = agentDisableTimerCap;
		//===================
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
		//Debug.Log("Reset Attacking!");
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
			//Debug.Log("Attacking!");
			if(rand == 0){
				attackCooldown = true;
				attackCooldownTimer = 5f;
				//anim.Play("heavy attack");
				anim.SetInteger("WhichHeavyAttackAnim", 1);
				//Debug.Log(this.gameObject.name + " is doing heavy attack!");
				anim.SetBool("isHeavyAttack", true);
				anim.SetBool("isLightAttack", false);
			}
			else if (rand == 1){
				attackCooldown = true;
				attackCooldownTimer = 3f;
				//anim.Play("light attack");
				anim.SetInteger("WhichLightAttackAnim", 1);
				//Debug.Log(this.gameObject.name + " is doing light attack!");
				anim.SetBool("isLightAttack", true);
				anim.SetBool("isHeavyAttack", false);
			}
			else if (rand == 2){
				attackCooldown = true;
				attackCooldownTimer = 5f;
				//anim.Play("heavy attack alt");
				anim.SetInteger("WhichHeavyAttackAnim", 2);
				//Debug.Log(this.gameObject.name + " is doing heavy attack #2!");
				anim.SetBool("isLightAttack", false);
				anim.SetBool("isHeavyAttack", true);
			}
			else if (rand == 3){
				attackCooldown = true;
				attackCooldownTimer = 3f;
				//anim.Play("light attack variant");
				anim.SetInteger("WhichLightAttackAnim", 2);
				//Debug.Log(this.gameObject.name + " is doing light attack #2!");
				anim.SetBool("isLightAttack", true);
				anim.SetBool("isHeavyAttack", false);

			}


		}
		            
	}
	public void startDodge(){
		
	}
	public void startBlock(){
		anim.SetBool("isBlocking", true);
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
		//===================
		//this was an attempt at making agents turn off their AI when damaged, to allow for falling off cliffs and knockup damage
		//if(agentDisable){
		//	agentDisableTimer = agentDisableTimer - Time.deltaTime;
		//	this.transform.rotation = rotation;
		//	if(agentDisableTimer <= 0f){
		//		agent.updatePosition = true;
		//		agent.updateRotation = true;
		//		agentDisable = false;
		//		//agent.enabled = true;
		//		agentDisableTimer = 0f;
		//	}
		//}
		//===================
	}
}
