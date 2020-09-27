using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"byte[]\", \"byte[]\", \"byte[]\"][\"int\", \"int\", \"int\"][][\"int\", \"int\"][\"string\"][\"uint\", \"int\"][\"byte[]\"][]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"Allwonsze\", \"walls\", \"playerapples\"][\"MapSize\", \"MinWonszLength\", \"StartWonszLength\"][][\"posx\", \"posy\"][\"text\"][\"Who\", \"What\"][\"byteswithresults\"][]]")]
	public abstract partial class GameLogicBehavior : NetworkBehavior
	{
		public const byte RPC_WONSZ_POSITION = 0 + 5;
		public const byte RPC_SET_MATCH_SETTINGS = 1 + 5;
		public const byte RPC_GAME_START = 2 + 5;
		public const byte RPC_MAKE_NEW_APPLE = 3 + 5;
		public const byte RPC_MESSAGE = 4 + 5;
		public const byte RPC_PLAYER_POINTS = 5 + 5;
		public const byte RPC_GAME_OVER = 6 + 5;
		public const byte RPC_CREATE_PLAYER = 7 + 5;
		
		public GameLogicNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (GameLogicNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("WonszPosition", WonszPosition, typeof(byte[]), typeof(byte[]), typeof(byte[]));
			networkObject.RegisterRpc("SetMatchSettings", SetMatchSettings, typeof(int), typeof(int), typeof(int));
			networkObject.RegisterRpc("GameStart", GameStart);
			networkObject.RegisterRpc("MakeNewApple", MakeNewApple, typeof(int), typeof(int));
			networkObject.RegisterRpc("Message", Message, typeof(string));
			networkObject.RegisterRpc("PlayerPoints", PlayerPoints, typeof(uint), typeof(int));
			networkObject.RegisterRpc("GameOver", GameOver, typeof(byte[]));
			networkObject.RegisterRpc("CreatePlayer", CreatePlayer);

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new GameLogicNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new GameLogicNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// byte[] Allwonsze
		/// byte[] walls
		/// byte[] playerapples
		/// </summary>
		public abstract void WonszPosition(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// int MapSize
		/// int MinWonszLength
		/// int StartWonszLength
		/// </summary>
		public abstract void SetMatchSettings(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void GameStart(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void MakeNewApple(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void Message(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void PlayerPoints(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void GameOver(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void CreatePlayer(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}