using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCBehaviorChangersList : MonoBehaviour
{
    public List<GameObject> npcs = new List<GameObject>();
    public List<GameObject> scary = new List<GameObject>();
    public List<GameObject> attractive = new List<GameObject>();
    public List<GameObject> nonScaryNPCs = new List<GameObject>();

    public void removeFromScary(GameObject agent){
        if(scary.Contains(agent)){
            scary.Remove(agent);
        }
        if(!nonScaryNPCs.Contains(agent)){
            nonScaryNPCs.Add(agent);
        }
        
    }
    public void removeFromAttractive(GameObject agent){
        if(attractive.Contains(agent)){
            attractive.Remove(agent);
        }
        
    }
    public void removeFromNPCS(GameObject agent){
        if(npcs.Contains(agent)){
            npcs.Remove(agent);
        }
        if(nonScaryNPCs.Contains(agent)){
            nonScaryNPCs.Remove(agent);
        }
        
    }
    public void updateNPCList(GameObject g){
        if(!npcs.Contains(g)){
            npcs.Add(g);
        }
        if(!nonScaryNPCs.Contains(g) && !g.GetComponent<NPCMove>().scary){
            nonScaryNPCs.Add(g);
        }
    }
    public void updateScaryList(GameObject g){
        if(!scary.Contains(g)){
            scary.Add(g);
        }
        if(nonScaryNPCs.Contains(g)){
            nonScaryNPCs.Remove(g);
        }
    }

    public void updateAttractiveList(GameObject g){
        if(!attractive.Contains(g)){
            attractive.Add(g);
        }

    }


}
