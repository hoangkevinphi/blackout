using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class BlackoutLobbyHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer){
	
		LobbyPlayer lplayer = lobbyPlayer.GetComponent<LobbyPlayer> ();
		PlayerNetworking gplayer = gamePlayer.GetComponent<PlayerNetworking> ();

		gplayer.playerName = lplayer.playerName;

		gplayer.playerColor = lplayer.playerColor;

	
	}
}
