using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationControlarScript : MonoBehaviour
{
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool _walk = Input.GetKey("w");
        bool _run = Input.GetKey("left shift");

        if (!isWalking && _walk)
        {
            animator.SetBool("isWalking", true);
        }
        if (isWalking && !_walk)
        {
            animator.SetBool("isWalking", false);
        }
        if(!isRunning && (_run && _walk))
        {
            animator.SetBool("isRunning", true);
        }
        if(isRunning && (!_run || !_walk))
        {
            animator.SetBool("isRunning", false);
        }

    }
}
