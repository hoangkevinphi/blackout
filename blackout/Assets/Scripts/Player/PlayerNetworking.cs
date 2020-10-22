using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class PlayerNetworking : NetworkBehaviour {

	[SyncVar (hook = "OnNameChanged")] public string playerName;
	[SyncVar (hook = "OnColorChanged")] public Color playerColor;
	[SyncVar (hook = "OnFlashlightToggled")] public bool flashlightToggle;

	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleRemote;
	[SerializeField] float respawnTime = 5f;

	static List<PlayerNetworking> players = new List<PlayerNetworking> ();

	GameObject mainCamera;
	NetworkAnimator anim;
	public Light flashlight;

	//fixes flashlight bug
	public bool flashlightenabled = true;

	void Start(){
		anim = GetComponent<NetworkAnimator> ();
//		flashlight = GetComponentInChildren<Light> (true);
//		Debug.Log (flashlight.gameObject.name);
		mainCamera = Camera.main.gameObject;

		EnablePlayer ();
	}

	[ServerCallback]
	void OnEnable(){
		if (!players.Contains (this))
			players.Add (this);

		flashlight.enabled = true;
		flashlightenabled = true;
	}

	[ServerCallback]
	void OnDisable(){
		if (players.Contains (this))
			players.Remove (this);
	}

	void Update(){

		if (flashlightenabled) {
			flashlight.enabled = false;
			flashlightenabled = false;
		}

		//not interested in remote players
		if (!isLocalPlayer)
			return;

//		if (flashlight == null) {
//			flashlight = GetComponentInChildren<Light> (true);
//			Debug.Log (flashlight.gameObject.name);
//		}

		if (Input.GetKeyDown (KeyCode.F)) {
			this.CmdToggleFlashlight ();
		}


		anim.animator.SetFloat ("Speed", Input.GetAxis ("Vertical"));
		anim.animator.SetFloat ("Strafe", Input.GetAxis ("Horizontal"));
	}

	void DisablePlayer(){

		if (isLocalPlayer) {
			PlayerCanvas.instance.HideReticule ();
			mainCamera.SetActive (true);
		
		}

		onToggleShared.Invoke (false);

		if (isLocalPlayer)
			onToggleLocal.Invoke (false);
		else
			onToggleRemote.Invoke (false);
	
	}

	void EnablePlayer(){

		if (isLocalPlayer) {
			PlayerCanvas.instance.Initialize ();
			mainCamera.SetActive (false);
		}
		
		onToggleShared.Invoke (true);

		if (isLocalPlayer)
			onToggleLocal.Invoke (true);
		else
			onToggleRemote.Invoke (true);
	
	}

	public void Die(){

		if (isLocalPlayer) {
			PlayerCanvas.instance.WriteGameStatusText ("You Died!");
			PlayerCanvas.instance.PlayDeathAudio ();

			anim.SetTrigger ("Died");
		}

		DisablePlayer ();

		Invoke ("Respawn", respawnTime);
	
	}
	void Respawn(){
	
		if (isLocalPlayer || playerControllerId == -1)
			anim.SetTrigger ("Restart");
		

		if (isLocalPlayer) {
			Transform spawn = NetworkManager.singleton.GetStartPosition ();
			transform.position = spawn.position;
			transform.rotation = spawn.rotation;
		}

		EnablePlayer ();
	}

	void OnNameChanged(string val){
		playerName = val;
		gameObject.name = playerName;
		GetComponentInChildren<Text> (true).text = playerName;
	}

	void OnColorChanged(Color val){
		playerColor = val;
		GetComponentInChildren<RendererToggler> ().ChangeColor (playerColor);
	}

	void OnFlashlightToggled(bool val){
		flashlightToggle = val;
	}

	[Command]
	void CmdToggleFlashlight(){
		RpcToggleFlashlight ();
	}

	[ClientRpc]
	void RpcToggleFlashlight(){
		this.flashlightToggle = !flashlightToggle;
		this.flashlight.enabled = flashlightToggle;
	}

	[Server]
	public void Won(){
		//tell other players
		for (int i = 0; i < players.Count; i++)
			players [i].RpcGameOver (netId, name);
		
		//go back to lobby (everyone)
		Invoke("BackToLobby",5f);

	
	}

	[ClientRpc]
	void RpcGameOver(NetworkInstanceId networkID, string name){

		DisablePlayer ();

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		if (isLocalPlayer) {
			if (netId == networkID)
				PlayerCanvas.instance.WriteGameStatusText ("You Won!");
			else {
				PlayerCanvas.instance.WriteGameStatusText ("Game Over\n" + name + " won!");
			}
		}
	}


	void BackToLobby(){
		FindObjectOfType<NetworkLobbyManager> ().SendReturnToLobby ();
	}
}
