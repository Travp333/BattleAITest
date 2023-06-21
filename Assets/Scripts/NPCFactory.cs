using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFactory : MonoBehaviour
{
    NPCBehaviorChangersList list;
    [SerializeField]
    int LifespanUpper, LifespanLower;
    NavMeshPath path;
    [SerializeField]
    Transform homeBeacon;
    [SerializeField]
    public List<Transform> NPCSpawners = new List<Transform>();
    [SerializeField]
    public List<GameObject> NPCList = new List<GameObject>();
    [SerializeField]
    int maxNPC;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    int NPCStartingAmountUpper, NPCStartingAmountLower;
    int NPCStartingAmount;
    float spawnTimerCount, searchCap = 1;

    void AssignRandomNpcTraits(GameObject g2){
        int rand2 = Random.Range(0,150);
        //probably a bit of a dumb way to do rng but i can change this later easy
        if(rand2 == 5){
            g2.GetComponent<NPCMove>().runner = true;
        }
        else if (rand2 == 6){
            g2.GetComponent<NPCMove>().attractive = true;
        }
        else if (rand2 == 34){
            g2.GetComponent<NPCMove>().runner = true;
            g2.GetComponent<NPCMove>().attractive = true;
        }
        else if (rand2 == 12 || rand2 == 15 || rand2 == 100 || rand2 == 120 || rand2 == 122){
            g2.GetComponent<NPCMove>().scary = true;
        }
        else if (rand2 == 150){
            g2.GetComponent<NPCMove>().brave = true;
        }
        else if (rand2 == 69){
            g2.GetComponent<NPCMove>().brave = true;
            g2.GetComponent<NPCMove>().attractive = true;
        }
    }

    public void DespawnNPC(GameObject g){
        //more optimized catlike method of removing from a list

        int lastIndex = list.npcs.Count - 1;
        list.npcs[list.npcs.IndexOf(g)] = list.npcs[lastIndex];  
        list.npcs.RemoveAt(lastIndex);
        list.npcs.Remove(g);

        if(g.GetComponent<NPCMove>()!= null){
            if(!g.GetComponent<NPCMove>().scary){
                lastIndex = list.nonScaryNPCs.Count - 1;
                list.nonScaryNPCs[list.nonScaryNPCs.IndexOf(g)] = list.nonScaryNPCs[lastIndex];  
                list.nonScaryNPCs.RemoveAt(lastIndex);
                list.nonScaryNPCs.Remove(g);
            }
            if(g.GetComponent<NPCMove>().scary){
                lastIndex = list.scary.Count - 1;
                list.scary[list.scary.IndexOf(g)] = list.scary[lastIndex];  
                list.scary.RemoveAt(lastIndex);
                list.scary.Remove(g);
            }
            if(g.GetComponent<NPCMove>().attractive){
                lastIndex = list.attractive.Count - 1;
                list.attractive[list.attractive.IndexOf(g)] = list.attractive[lastIndex];  
                list.attractive.RemoveAt(lastIndex);
                list.attractive.Remove(g);
            }
        }
        Destroy(g);


    }
    void SpawnNPC(){
        Vector3 randPos = GetRandomPoint();
        GameObject g2 = Instantiate(NPCList[Random.Range(0, NPCList.Count)], randPos, Quaternion.identity);
        AssignRandomNpcTraits(g2);
    }
    void SpawnRandomLocationNpc(){
        Vector3 randPos = GetValidSpawnPoint();
        GameObject g2 = Instantiate(NPCList[Random.Range(0, NPCList.Count)], randPos, Quaternion.identity);
        AssignRandomNpcTraits(g2);

    }

    public Vector3 GetValidSpawnPoint(){
        Vector3 randPos = RandomNavmeshLocation(Random.Range(10f, 500f));
        NavMesh.CalculatePath(homeBeacon.position, randPos, NavMesh.AllAreas, path);
        if(path.status == NavMeshPathStatus.PathComplete){
            return randPos;
        }
        else{
            return GetValidSpawnPoint();
        }
    }

    void Update()
    {
        if(list.npcs.Count < maxNPC){
            spawnTimerCount += Time.deltaTime;
            if(spawnTimerCount >= searchCap){
                SpawnNPC();
                spawnTimerCount = spawnTimerCount - searchCap;
                searchCap = Random.Range(5f, 10f);
            }
        }
    }
    void Start()
    {
        list = this.gameObject.GetComponent<NPCBehaviorChangersList>();
        searchCap = Random.Range(5f, 10f);
        path = new NavMeshPath();
        NPCStartingAmount = Random.Range(NPCStartingAmountUpper, NPCStartingAmountLower);
        for (int i = 0; i < NPCStartingAmount; i++)
        {
            SpawnRandomLocationNpc();
        }
    }

    public Vector3 GetRandomPoint(){
        int rand = Random.Range(0, NPCSpawners.Count - 1);
        Transform center = NPCSpawners[rand].transform;

        Vector3 finalPos;
        Vector3 randomDirection = GetRandomDirectionVector3();
        RaycastHit hit;
        if(Physics.Raycast(center.position, randomDirection, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide)){
            //Debug.Log("using random spot");
            //Debug.DrawLine(center.position, hit.point, Color.green, 5f);
            Vector3 difference = hit.point - center.position;
            Vector3 newDifference = difference * Random.Range(0f, 1f);
            finalPos = center.position + newDifference;
            NavMesh.CalculatePath(homeBeacon.position, finalPos, NavMesh.AllAreas, path);
            if(path.status == NavMeshPathStatus.PathComplete){
                return finalPos;
            }
            else{
                return GetRandomPoint();
            }

        }
        else{
            //Debug.Log("using center");
            return center.position;
        }

    }

    private Vector3 GetRandomDirectionVector3() {
        return ProjectDirectionOnPlane(Random.insideUnitSphere.normalized, this.transform.up);
    }
    Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}
    public Vector3 RandomNavmeshLocation(float radius) {
         Vector3 randomDirection = Random.insideUnitSphere * radius;
         randomDirection += transform.position;
         NavMeshHit hit;
         Vector3 finalPosition = Vector3.zero;
         if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
             finalPosition = hit.position;            
         }
        return finalPosition;
         
     }
}
