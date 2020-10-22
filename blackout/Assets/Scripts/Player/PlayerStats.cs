using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class PlayerStats : NetworkBehaviour
{
	[SerializeField] int maxHealth = 1;

	[SyncVar (hook="OnHealthChanged")]int health;

	PlayerNetworking player;



	void Awake(){
		player = GetComponent<PlayerNetworking> ();

	}
		
	[ServerCallback]
	void OnEnable(){
	
		health = maxHealth;

	
	}

	[Server]
	public bool TakeDamage(){
		bool died = false;

		if (health <= 0)
			return died;

		health--;
		died = health <= 0;

		RpcTakeDamage (died);

		return died;
	}

	[ClientRpc]
	void RpcTakeDamage(bool died){

		if (isLocalPlayer)
			PlayerCanvas.instance.FlashDamageEffect ();
		
		if (died)
			player.Die ();
	}

	void OnHealthChanged(int value){
		health = value;

		if (isLocalPlayer) {
			//do something
		}
	}
}
