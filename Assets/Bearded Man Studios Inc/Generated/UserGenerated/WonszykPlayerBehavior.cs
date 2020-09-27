using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"string\"][\"int\"][\"string\"][\"Color\"][\"int\", \"int\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"positions\"][\"Type\"][\"name\"][\"color\"][\"x\", \"y\"]]")]
	public abstract partial class WonszykPlayerBehavior : NetworkBehavior
	{
		public const byte RPC_MOVE = 0 + 5;
		public const byte RPC_SHOOT = 1 + 5;
		public const byte RPC_SET_NAME = 2 + 5;
		public const byte RPC_SET_COLOR = 3 + 5;
		public const byte RPC_SET_HEAD_POSITION = 4 + 5;
		
		public WonszykPlayerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (WonszykPlayerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("Move", Move, typeof(string));
			networkObject.RegisterRpc("Shoot", Shoot, typeof(int));
			networkObject.RegisterRpc("SetName", SetName, typeof(string));
			networkObject.RegisterRpc("SetColor", SetColor, typeof(Color));
			networkObject.RegisterRpc("SetHeadPosition", SetHeadPosition, typeof(int), typeof(int));

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
			Initialize(new WonszykPlayerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new WonszykPlayerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// string positions
		/// </summary>
		public abstract void Move(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// int Type
		/// </summary>
		public abstract void Shoot(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// string name
		/// </summary>
		public abstract void SetName(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Color color
		/// </summary>
		public abstract void SetColor(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// int x
		/// int y
		/// </summary>
		public abstract void SetHeadPosition(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}