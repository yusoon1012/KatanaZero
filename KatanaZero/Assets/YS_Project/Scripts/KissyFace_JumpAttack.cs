using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissyFace_JumpAttack : MonoBehaviour
{
    public GameObject attackPrefab;
    private float axeTimer = 0;
    private float axeRate = 1f;
    Animator kissyAni;
    Rigidbody2D rb;
    bool jumpAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        kissyAni = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(!jumpAttack)
        {
          
            jumpAttack = true;
        kissyAni.Play("Kissyface_prejump");
        Vector3 jump = new Vector3(0, 8f, 0);
        rb.AddForce(jump, ForceMode2D.Impulse);
            StartCoroutine(AxeRoutine());
        }
        
    }
    private IEnumerator AxeRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        kissyAni.Play("Kissyface_jumpswing");
        GameObject axe = Instantiate(attackPrefab, transform.position, transform.rotation);
       
        rb.gravityScale = 0.5f;
        yield return new WaitForSeconds(1.7f);
        kissyAni.Play("Kissyface_landattack");

    }

}
