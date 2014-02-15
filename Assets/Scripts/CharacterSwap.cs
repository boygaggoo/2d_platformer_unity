﻿using UnityEngine;
using System.Collections;

public class CharacterSwap : MonoBehaviour {

	//the intermediary swap explosion
	public GameObject explosion;
	//the keypress as chosen via the inspector
	public KeyCode swapKey;
	//allow some grace before next swap
	public float timeBeforeNextSwap;
	//the actual character list array
	public GameObject[] characters;

	private GameObject currentCharacter;
	private int currentCharacterID;
	private Transform cam;
	private CameraFollow camFollow;


	void Awake() {

		//this script will destroy the character and instantiate the next one so we need to tell this new info to the main cam
		this.cam = Camera.main.transform;
		this.camFollow = cam.GetComponent<CameraFollow>();
		//we track current character via an ID
		this.currentCharacterID = 0;
	}

	void Start () {

		this.StartCoroutine(CheckSwapChars());
		Vector3 characterPosition = new Vector3(this.cam.position.x, this.cam.position.y, 0);
		//		this.currentCharacter = Instantiate(this.characters[this.currentCharacterID], characterPosition, Quaternion.identity) as GameObject;
		this.currentCharacter = ObjectPool.instance.GetObjectForType("Character", true);
		this.currentCharacter.transform.position = characterPosition;
	}

	IEnumerator CheckSwapChars() {

		float swapRate = 0;
		while (true) {
			if (Input.GetKeyDown(this.swapKey) && Time.time > swapRate) {
				StartCoroutine(SwapCharacters());
				swapRate = Time.time + this.timeBeforeNextSwap;
			}

			yield return null;
		}
	}

	IEnumerator SwapCharacters () {

		Vector3 currentCharPosition = this.currentCharacter.transform.position;

		this.camFollow.FollowPlayer();

//		this.currentCharacter.renderer.enabled = false;
		ObjectPool.instance.PoolObject(this.currentCharacter);

//		GameObject swapExplosion = Instantiate(this.explosion, currentCharPosition, Quaternion.identity) as GameObject;		


		GameObject swapExplosion = ObjectPool.instance.GetObjectForType ("Player Explosion", true);
		swapExplosion.transform.position = currentCharPosition;

		AudioHelper.CreatePlayAudioObject(BaseManager.instance.characterSwap, .5f);

		yield return new WaitForSeconds(1.25f);

//		Destroy(swapExplosion);
		ObjectPool.instance.PoolObject(swapExplosion);

		this.currentCharacterID++;
		GameObject newCurrentChar;

		switch (this.currentCharacterID) {
//			case 0: newCurrentChar = Instantiate(this.characters[0], currentCharPosition, Quaternion.identity) as GameObject;
			case 0: newCurrentChar = ObjectPool.instance.GetObjectForType(this.characters[0].name, true);
			newCurrentChar.transform.position = currentCharPosition;
			break;
//			case 1: newCurrentChar = Instantiate(this.characters[1], currentCharPosition, Quaternion.identity) as GameObject;
			case 1: newCurrentChar = ObjectPool.instance.GetObjectForType(this.characters[1].name, true);
			newCurrentChar.transform.position = currentCharPosition;
			break;
//			case 2: newCurrentChar = Instantiate(this.characters[2], currentCharPosition, Quaternion.identity) as GameObject;
			case 2: newCurrentChar = ObjectPool.instance.GetObjectForType(this.characters[2].name, true);
			newCurrentChar.transform.position = currentCharPosition;
			break;
			default: this.currentCharacterID = 0;
//			newCurrentChar = Instantiate(this.characters[0], currentCharPosition, Quaternion.identity) as GameObject;
			newCurrentChar = ObjectPool.instance.GetObjectForType(this.characters[0].name, true);
			newCurrentChar.transform.position = currentCharPosition;
			break;
		}

//		yield return StartCoroutine(this.camFollow.SetNewPlayer(newCurrentChar.name));
		this.camFollow.SetNewPlayer(newCurrentChar.name);
		this.camFollow.FollowPlayer();

//		Destroy(this.currentCharacter);
		this.currentCharacter = GameObject.Find(newCurrentChar.name);
	}
}
