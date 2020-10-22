using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour {

	public static PlayerCanvas instance;

	[Header("Component References")]
	[SerializeField] Image reticule;
	[SerializeField] Image damageImage;
	[SerializeField] Text gameStatusText;
	[SerializeField] Text ammoValue;
	[SerializeField] Text scoreValue;
	[SerializeField] Text logText;
	[SerializeField] AudioSource deathAudio;

	void Awake(){
	
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
	}

	void Reset(){
		reticule = GameObject.Find ("reticule").GetComponent<Image> ();
		damageImage = GameObject.Find ("damagedflash").GetComponent<Image> ();
		gameStatusText = GameObject.Find ("gamestatustext").GetComponent<Text> ();
		ammoValue = GameObject.Find ("ammovalue").GetComponent<Text> ();
		scoreValue = GameObject.Find ("scorevalue").GetComponent<Text> ();
		logText = GameObject.Find ("logtext").GetComponent<Text> ();
		deathAudio = GameObject.Find ("deathaudio").GetComponent < AudioSource> ();
	}

	public void Initialize(){
		reticule.enabled = true;
		gameStatusText.text = "";
	
	}

	public void HideReticule(){
		reticule.enabled = false;
	}

	public void FlashDamageEffect(float duration=0.10f){
		CancelInvoke ();
		damageImage.enabled = true;
		Invoke ("ClearFlashImage", duration);
	}
	void ClearFlashImage(){
		damageImage.enabled = false;
	}

	public void PlayDeathAudio(){
		if (!deathAudio.isPlaying)
			deathAudio.Play ();
	}

	public void SetAmmo(int amount){
		ammoValue.text = amount.ToString ();
	}

	public void SetScore(int amount){
		scoreValue.text = amount.ToString ();
	}

	public void WriteGameStatusText(string text){
		gameStatusText.text = text;
	}

	public void WriteLogText(string text,float duration){
		CancelInvoke ();
		logText.text = text;
		Invoke ("ClearLogText", duration);
	}

	void ClearLogText(){
		logText.text = "";
	}



}
