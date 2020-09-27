using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0]")]
	public partial class PlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 6;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private int _Direction;
		public event FieldEvent<int> DirectionChanged;
		public Interpolated<int> DirectionInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int Direction
		{
			get { return _Direction; }
			set
			{
				// Don't do anything if the value is the same
				if (_Direction == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_Direction = value;
				hasDirtyFields = true;
			}
		}

		public void SetDirectionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_Direction(ulong timestep)
		{
			if (DirectionChanged != null) DirectionChanged(_Direction, timestep);
			if (fieldAltered != null) fieldAltered("Direction", _Direction, timestep);
		}
		[ForgeGeneratedField]
		private bool _Shoot;
		public event FieldEvent<bool> ShootChanged;
		public Interpolated<bool> ShootInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool Shoot
		{
			get { return _Shoot; }
			set
			{
				// Don't do anything if the value is the same
				if (_Shoot == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_Shoot = value;
				hasDirtyFields = true;
			}
		}

		public void SetShootDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_Shoot(ulong timestep)
		{
			if (ShootChanged != null) ShootChanged(_Shoot, timestep);
			if (fieldAltered != null) fieldAltered("Shoot", _Shoot, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			DirectionInterpolation.current = DirectionInterpolation.target;
			ShootInterpolation.current = ShootInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _Direction);
			UnityObjectMapper.Instance.MapBytes(data, _Shoot);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_Direction = UnityObjectMapper.Instance.Map<int>(payload);
			DirectionInterpolation.current = _Direction;
			DirectionInterpolation.target = _Direction;
			RunChange_Direction(timestep);
			_Shoot = UnityObjectMapper.Instance.Map<bool>(payload);
			ShootInterpolation.current = _Shoot;
			ShootInterpolation.target = _Shoot;
			RunChange_Shoot(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Direction);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Shoot);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (DirectionInterpolation.Enabled)
				{
					DirectionInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					DirectionInterpolation.Timestep = timestep;
				}
				else
				{
					_Direction = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_Direction(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (ShootInterpolation.Enabled)
				{
					ShootInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					ShootInterpolation.Timestep = timestep;
				}
				else
				{
					_Shoot = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_Shoot(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (DirectionInterpolation.Enabled && !DirectionInterpolation.current.UnityNear(DirectionInterpolation.target, 0.0015f))
			{
				_Direction = (int)DirectionInterpolation.Interpolate();
				//RunChange_Direction(DirectionInterpolation.Timestep);
			}
			if (ShootInterpolation.Enabled && !ShootInterpolation.current.UnityNear(ShootInterpolation.target, 0.0015f))
			{
				_Shoot = (bool)ShootInterpolation.Interpolate();
				//RunChange_Shoot(ShootInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerNetworkObject() : base() { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
