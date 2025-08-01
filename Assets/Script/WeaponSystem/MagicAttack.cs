using System.Collections;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public float throwForce = 10f;
    public Transform magicArea;
    public GameObject magicPrefab;
    public GameObject book;
    public Animator anim;
    bool nextThrow = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && nextThrow)
        {
            book.SetActive(true);
            StartCoroutine(MagicAnimation());
        }
    }
    void ThrowMagic()
    {
        GameObject magic = Instantiate(magicPrefab, magicArea.transform.position, magicArea.transform.rotation);
        Rigidbody rb = magic.GetComponent<Rigidbody>();
        rb.AddForce(magicArea.transform.forward * throwForce, ForceMode.VelocityChange);
    }
    IEnumerator MagicAnimation()
    {
        nextThrow = false;
        anim.SetBool("MagicInAir", true);
        yield return new WaitForSeconds(0.5f);
        ThrowMagic();
        yield return new WaitForSeconds(1f);
        anim.SetBool("MagicInAir", false);
        nextThrow = true;
    }
}
