using UnityEngine;

public class endingNPCTrigger : MonoBehaviour
{
    public GameObject npcToActivate; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcToActivate.SetActive(true); 
                                           
            Animator anim = npcToActivate.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("YourTriggerName"); 
            }
        }
    }
}
