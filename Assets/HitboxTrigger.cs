using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
	NPCFight fight;
	Vector3 dist;
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != this.gameObject && other.gameObject.layer == 7){
			//for capsule collision
			if (other.gameObject.GetComponent<NPCFight>() != null){
				if (other.gameObject.GetComponent<NPCFight>().invulnerabilityPeriod == false){
					//Debug.Log(this.transform.root.gameObject.name + " Hit " + other.gameObject.name);
					fight = other.gameObject.GetComponent<NPCFight>();
					dist = this.transform.root.forward;
					fight.takeDamage(dist);
				}
			}
			//for hitbox collision
			if (other.gameObject.transform.root.gameObject.GetComponent<NPCFight>() != null){
				if (other.gameObject.transform.root.gameObject.GetComponent<NPCFight>().invulnerabilityPeriod == false){
					//Debug.Log(this.transform.root.gameObject.name + " Hit " + other.gameObject.name);
					fight = other.gameObject.transform.root.gameObject.GetComponent<NPCFight>();
					dist = this.transform.root.forward;
					fight.takeDamage(dist);
				}
			}
		}
	}

}
