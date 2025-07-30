using System.Collections;
using UnityEngine;

public class BombThrow : MonoBehaviour
{
    public float throwForce = 10f;
    public Transform bombArea;
    public GameObject bombPrefab;
    public Animator anim;
    bool nextThrow = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && nextThrow)
        {
            StartCoroutine(BombAnimation());
        }
    }
    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombArea.transform.position, bombArea.transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.AddForce(bombArea.transform.forward * throwForce, ForceMode.VelocityChange);
    }
    IEnumerator BombAnimation()
    {
        nextThrow = false;
        anim.SetBool("BombInAir", true);
        yield return new WaitForSeconds(0.5f);
        ThrowBomb();
        yield return new WaitForSeconds(1f);
        anim.SetBool("BombInAir", false);
        nextThrow = true;
    }
}
