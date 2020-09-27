using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[][\"string\"][\"string\"][\"Vector2\"][\"int\", \"uint\"][\"uint\", \"int\"][\"int\", \"int\", \"int\"][\"string\"][\"int\", \"uint\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[][\"Results\"][\"Results\"][\"newapple\"][\"Type\", \"player\"][\"Who\", \"Power\"][\"size\", \"minLength\", \"startLength\"][\"Content\"][\"kindOfApple\", \"player\"]]")]
	public abstract partial class WonszykLogicBehavior : NetworkBehavior
	{
		public const byte RPC_GAME_START = 0 + 5;
		public const byte RPC_GAME_ENDED = 1 + 5;
		public const byte RPC_RESULTS = 2 + 5;
		public const byte RPC_MAKE_NEW_APPLE = 3 + 5;
		public const byte RPC_PLAYER_SHOT = 4 + 5;
		public const byte RPC_PLAYER_HIT = 5 + 5;
		public const byte RPC_SET_MATCH_SETTINGS = 6 + 5;
		public const byte RPC_MESSAGE = 7 + 5;
		public const byte RPC_PLAYER_ATE_APPLE = 8 + 5;
		
		public WonszykLogicNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (WonszykLogicNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("GameStart", GameStart);
			networkObject.RegisterRpc("GameEnded", GameEnded, typeof(string));
			networkObject.RegisterRpc("Results", Results, typeof(string));
			networkObject.RegisterRpc("MakeNewApple", MakeNewApple, typeof(Vector2));
			networkObject.RegisterRpc("PlayerShot", PlayerShot, typeof(int), typeof(uint));
			networkObject.RegisterRpc("PlayerHit", PlayerHit, typeof(uint), typeof(int));
			networkObject.RegisterRpc("SetMatchSettings", SetMatchSettings, typeof(int), typeof(int), typeof(int));
			networkObject.RegisterRpc("Message", Message, typeof(string));
			networkObject.RegisterRpc("PlayerAteApple", PlayerAteApple, typeof(int), typeof(uint));

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
			Initialize(new WonszykLogicNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new WonszykLogicNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void GameStart(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void GameEnded(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void Results(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void MakeNewApple(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void PlayerShot(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void PlayerHit(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void SetMatchSettings(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void Message(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void PlayerAteApple(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}