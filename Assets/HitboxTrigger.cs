using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
	NPCFight fight;
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != this.gameObject && other.gameObject.layer == 7){
			if (other.gameObject.GetComponent<NPCFight>() != null){
				if (other.gameObject.GetComponent<NPCFight>().invulnerabilityPeriod == false){
					Debug.Log(this.transform.root.gameObject.name + " Hit " + other.gameObject.name);
					fight = other.gameObject.GetComponent<NPCFight>();
					fight.takeDamage();
				}
			}
			if (other.gameObject.transform.root.gameObject.GetComponent<NPCFight>() != null){
				if (other.gameObject.transform.root.gameObject.GetComponent<NPCFight>().invulnerabilityPeriod == false){
					Debug.Log(this.transform.root.gameObject.name + " Hit " + other.gameObject.name);
					fight = other.gameObject.transform.root.gameObject.GetComponent<NPCFight>();
					fight.takeDamage();
				}
			}
		}
	}

}
