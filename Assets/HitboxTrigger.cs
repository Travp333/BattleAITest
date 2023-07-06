using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
	NPCFight fight;
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != this.gameObject && other.gameObject.layer == 7){
			Debug.Log(this.transform.root.gameObject.name + " Hit " + other.gameObject.name);
			if (other.gameObject.GetComponent<NPCFight>() != null){
				fight = other.gameObject.GetComponent<NPCFight>();
				fight.takeDamage();
			}
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
