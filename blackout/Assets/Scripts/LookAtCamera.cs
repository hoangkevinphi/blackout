using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	Transform mainCamera;

	void Start(){
	
		mainCamera = Camera.main.transform;	
	}

	void LateUpdate(){
	
		if (mainCamera == null)
			return;
	
	
		//ensure UI elements are facing the player's camera
		transform.rotation = Quaternion.LookRotation (transform.position - mainCamera.position);
	
	}
}
