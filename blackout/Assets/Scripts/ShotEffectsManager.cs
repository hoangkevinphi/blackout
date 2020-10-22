using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEffectsManager : MonoBehaviour {

	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] AudioSource gunAudio;
	[SerializeField] GameObject impactPrefab;

	ParticleSystem impactEffect;

	public void Initialize(){
		impactEffect = Instantiate (impactPrefab).GetComponent<ParticleSystem> ();
	
	}

	public void PlayShotEffects(){
		muzzleFlash.Stop (true);
		muzzleFlash.Play (true);
		gunAudio.Stop ();
		gunAudio.Play ();
	
	
	}
	

	public void PlayImpactEffect(Vector3 impactPosition){
		
		if (impactEffect == null)
			return;
		
		impactEffect.transform.position = impactPosition;
		impactEffect.Stop ();
		impactEffect.Play ();
	
	}
}
