using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererToggler : MonoBehaviour {

	[SerializeField] float turnOnDelay = .1f;
	[SerializeField] float turnoffDelay = 3.5f;
	[SerializeField] bool enabledOnLoad=false;

	Renderer[] renderers;

	void Awake(){
		renderers = GetComponentsInChildren<Renderer> (true);

		if (enabledOnLoad)
			EnableRenderers ();
		else
			DisableRenderers ();
	}

	public void ToggleRenderersDelayed(bool isOn){
		if (isOn)
			Invoke ("EnableRenderers", turnOnDelay);
		else
			Invoke ("DisableRenderers", turnoffDelay);
	}
	public void EnableRenderers(){
		for (int i = 0; i < renderers.Length; i++) {
			renderers [i].enabled = true;
		}
	
	}
	public void DisableRenderers(){
		for (int i = 0; i < renderers.Length; i++) {
			renderers [i].enabled = false;
		}

	}
	public void ChangeColor(Color newColor){
	
		for (int i = 0; i < renderers.Length; i++) {
			renderers [i].material.color = newColor;
		}
	}
}
