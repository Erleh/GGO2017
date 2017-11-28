﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Player
{
    //Animator
    Animator anim;

    //Sets initial values
    void Awake()
    {
        PassiveFatigue = 0.025f;
        PushFatigue = 0.05f;
        ShoveFatigue = 5f;
        KickFatigue = 10f;
        //speed = -.2f;
        StrOfKick = 10f;
        StrOfShove = 20f;
        ExtendStrength = 2f;
        /*Need implementation first*/
        //ShoveFatigue = 5f;
        //KickFatigue = 7f;

        anim = this.GetComponent<Animator>();
    }

    void OnEnable()
    {
        ActionAnimationHandler.onPush += this.OnCharacterPush;
        ActionAnimationHandler.onShove += this.OnCharacterShove;
        ActionAnimationHandler.onKick += this.OnCharacterKick;
    }

    void OnDisable()
    {
        ActionAnimationHandler.onPush -= this.OnCharacterPush;
        ActionAnimationHandler.onShove -= this.OnCharacterShove;
        ActionAnimationHandler.onKick -= this.OnCharacterKick;
    }

    void Start()
    {

    }

    public void OnCharacterPush()
    {
        anim.SetBool("Push", pushing);
    }

    public void OnCharacterShove()
    {
        anim.SetBool("Shove", shoving);
    }

    public void OnCharacterKick()
    {
        anim.SetBool("Kick", kicking);
    }

    //Player controller
    void FixedUpdate()
    {
        anim.SetBool("ChargeAt", charging);
        //Debug.Log(pushing);
        //Debug.Log(coRunning);
        //Debug.Log("Player Update");
        if (enemy && !fc.loseGame)
        {
            //If player is grappling enemy and no coroutines are running
            if (grapple && !coRunning)
            {
                //can't shove if already shoving
                if (Input.GetKeyDown(KeyCode.Z) && shoveCoroutine == null && extendCoroutine == null)
                {
                    pushing = false;
                    Shove();
                }

                if (Input.GetKeyDown(KeyCode.X) && kickCoroutine == null)
                {
                    pushing = false;
                    //kicking = true;
                    Kick();
                }

                //can't push when charging back at the enemy
                if (Input.GetKey(KeyCode.Space) && chargeCoroutine == null)
                {
                    pushing = true;
                    Push();
                    //Debug.Log("Work it.");

                    Debug.Log("Pushing = " + pushing);
                }
                //sets variable to false so enemy can continue resisting in their update
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    pushing = false;
                }
            }
            else
            {
                pushing = false;
                //kicking = false;

                //waits for full  shove lerp to play before charging back at enemy
                //debug test statement:
                // if(extendShoveCoroutine != null) { Debug.Log("Extend shove still running..."); }
                if (shoveCoroutine == null && extendCoroutine == null && kickCoroutine == null)
                    chargeCoroutine = StartCoroutine(ChargeAtEnemy());

                anim.SetBool("Grapple", grapple);
            }
        }
        else
        {
            pushing = false;
            //Lose Game here
            Debug.Log("Lost the game. Fatigued.");
        }
    }
}
