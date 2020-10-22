using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
	[SerializeField] float shotCooldown = 1f;
	[SerializeField] int scoreToWin = 3;
	[SerializeField] Transform firePos;
	[SerializeField] ShotEffectsManager shotEffects;

	[SyncVar (hook="OnAmmoChanged")] int ammo;
	[SyncVar (hook="OnScoreChanged")] int score=0;
	int maxAmmo = 30;

	PlayerNetworking player;
	float elapsedTime;
	public bool canShoot;

	[ServerCallback]
	void OnEnable(){
		
		if (score > scoreToWin)
			score = 0;
		
		ammo = maxAmmo;
		canShoot = true;
	}

	[ServerCallback]
	void Start(){
		
		if (score > scoreToWin)
			score = 0;
		
		player = GetComponent<PlayerNetworking> ();

		ammo = maxAmmo;
		shotEffects.Initialize ();

		if (isLocalPlayer)
			canShoot = true;

	}

	void Update(){

		//fixes client shooting bug
		if (!canShoot)
			canShoot = true;

		if (ammo < 1)
			canShoot = false;

		if (!isLocalPlayer)
			return;

		elapsedTime += Time.deltaTime;

		if (canShoot) {
			if (Input.GetButtonDown ("Fire1") && elapsedTime > shotCooldown) {
				elapsedTime = 0f;
				//this.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
				CmdFireShot (firePos.position, firePos.forward);
		
			}
		}
	}

	[Command]
	void CmdFireShot(Vector3 origin, Vector3 direction){

		RaycastHit hit;
		Ray ray = new Ray (origin, direction);
		Debug.DrawRay (ray.origin, ray.direction * 3f, Color.red, 1f);

		bool result = Physics.Raycast (ray, out hit, 500f);

		if (result) {
			PlayerStats enemy = hit.transform.GetComponent<PlayerStats> ();

			if (enemy != null) {

				bool killShot = enemy.TakeDamage ();
				score++;

				if (killShot && (score >= scoreToWin))
					player.Won ();
				
			}
		}
		RpcProcessShotEffects (result, hit.point);
	}

	[ClientRpc]
	void RpcProcessShotEffects(bool result, Vector3 point){
		//ammo--;
		shotEffects.PlayShotEffects ();
		if (result)
			shotEffects.PlayImpactEffect (point);
	
	}

	void OnAmmoChanged(int value){
		ammo = value;
		//if (isLocalPlayer)
			//PlayerCanvas.instance.SetAmmo (value);
	}

	void OnScoreChanged(int val){
		score = val;
		if (isLocalPlayer)
			PlayerCanvas.instance.SetScore (val);
	}
}
