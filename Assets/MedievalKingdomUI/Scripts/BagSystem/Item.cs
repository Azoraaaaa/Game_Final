using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public string itemName;

    public int quantity;

    public Sprite sprite;

    public string itemDescription;

    private bool canBePickedUp = false;
    public float pickupDelay = 1f;

    //public AudioSource audioSource;
    //public AudioClip pickSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EnablePickupAfterDelay());
    }
    private IEnumerator EnablePickupAfterDelay()
    {
        yield return new WaitForSeconds(pickupDelay);
        canBePickedUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canBePickedUp && other.gameObject.tag == "Player")
        {
            //ItemSoundManager.instance.PlaySound(pickSound, transform.position);
            Debug.Log("Play!");
            Debug.Log($"Player tries to pick up: {itemName}, Quantity: {quantity}");
            int leftOverItems = InventoryController.instance.AddItem(itemName, quantity, sprite, itemDescription);
            if (leftOverItems <= 0)
            {
                Destroy(gameObject);
                Debug.Log("Item picked up");
            }
            else
            {
                quantity = leftOverItems;
            }
        }
    }
    /*
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    */
}
