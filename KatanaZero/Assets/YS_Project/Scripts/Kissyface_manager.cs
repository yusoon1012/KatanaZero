using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kissyface_manager : MonoBehaviour
{
    [SerializeField] private KissyFace_JumpAttack jumpAttack;
    [SerializeField] private Kissyface_Lunge lunge;
    public bool isAction = false;
    public int pattern=0;
    public int lastPattern=0;
    public BoxCollider2D playerCollider;
    private BoxCollider2D kissyfaceCollider;
    const int IDLE = 0;
    const int JUMP_ATTACK = 1;
    const int LUNGE = 2;
    const int THROW = 3;

    // Start is called before the first frame update
    void Start()
    {
        kissyfaceCollider = GetComponent<BoxCollider2D>();
        jumpAttack = GetComponent<KissyFace_JumpAttack>();
        lunge = GetComponent<Kissyface_Lunge>();
        StartCoroutine(SelectAction());
        Physics2D.IgnoreCollision(kissyfaceCollider, playerCollider);

    }

    // Update is called once per frame
    void Update()
    {
        if(!isAction)
        {
            lunge.enabled = false;
            jumpAttack.enabled = false;
            StartCoroutine(SelectAction());
        }
    }
    private IEnumerator SelectAction()
    {
        isAction = true;
        while(lastPattern==pattern)
        {
        pattern = Random.Range(1, 3);

        }

        
        lastPattern = pattern;
        yield return new WaitForSeconds(1f);
        if(pattern==JUMP_ATTACK)
        {
            jumpAttack.enabled = true;
        }
        else if(pattern==LUNGE)
        {
            lunge.enabled = true;
        }
    }
}
