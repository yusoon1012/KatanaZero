using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExit : MonoBehaviour
{
    Kissyface_manager manager;
    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindAnyObjectByType<Kissyface_manager>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.isDie==true)
        {
            boxCollider.enabled = true;
        }
    }
}
