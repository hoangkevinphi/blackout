using UnityEngine;
using UnityEngine.Networking;

public class GunPositionSync : NetworkBehaviour {

	[SerializeField] Transform cameraTransform;
	[SerializeField] Transform handMount;
	[SerializeField] Transform gunPivot;
	[SerializeField] float threshold=10f;
	[SerializeField] float smoothing=5f;

//	public float testy=45f;
//	public float testz=45f;

	[SyncVar] float pitch;
	Vector3 lastOffset;
	float lastSyncedPitch;

	void Start(){
		
		if (isLocalPlayer)
			gunPivot.parent = cameraTransform;
		else
			lastOffset = handMount.position - transform.position;
	
	}

	void Update(){
		
		if (isLocalPlayer) {
			
			pitch = cameraTransform.localRotation.eulerAngles.x;

			if (Mathf.Abs (lastSyncedPitch - pitch) >= threshold) {
				CmdUpdatePitch (pitch);
				lastSyncedPitch = pitch;
			}

		} else {

			Vector3 currentOffset = handMount.position - transform.position;
			gunPivot.localPosition += currentOffset - lastOffset;
			lastOffset = currentOffset;

			Quaternion newRotation = Quaternion.Euler (90f, 90f, 0f);
			gunPivot.localRotation = Quaternion.Lerp (gunPivot.localRotation, newRotation, Time.deltaTime * smoothing);
				
		}


	
	}

	[Command]
	void CmdUpdatePitch(float newPitch){
		pitch = newPitch;
	}

}
