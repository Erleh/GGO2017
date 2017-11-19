﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour {

	public delegate void makePush(GameObject character);
	public delegate void onResist(GameObject character);

	public static event onResist resist;
	public static event makePush onPush;

	public GameObject character;

	public static void OnCharacterPush(GameObject character)
	{
		if(onPush != null)
		{
			onPush(character);
		}
	}
		
	public static void EnemyResist(GameObject character)
	{
		if(resist != null)
		{
			resist(character);
		}
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Space))
		{
			onPush(character);
			//Debug.Log("Push");
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			EnemyResist(character);
			//Debug.Log("Resisting");
		}
	}
}
