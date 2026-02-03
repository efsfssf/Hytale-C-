using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000255 RID: 597
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ParticipantUpdatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<ParticipantUpdatedCallbackInfo>, ISettable<ParticipantUpdatedCallbackInfo>, IDisposable
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x00017B6C File Offset: 0x00015D6C
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x00017B8D File Offset: 0x00015D8D
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00017BA0 File Offset: 0x00015DA0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00017BB8 File Offset: 0x00015DB8
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x00017BD9 File Offset: 0x00015DD9
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00017BEC File Offset: 0x00015DEC
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x00017C0D File Offset: 0x00015E0D
		public Utf8String RoomName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RoomName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00017C20 File Offset: 0x00015E20
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x00017C41 File Offset: 0x00015E41
		public ProductUserId ParticipantId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_ParticipantId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00017C54 File Offset: 0x00015E54
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x00017C75 File Offset: 0x00015E75
		public bool Speaking
		{
			get
			{
				bool result;
				Helper.Get(this.m_Speaking, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Speaking);
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00017C88 File Offset: 0x00015E88
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x00017CA0 File Offset: 0x00015EA0
		public RTCAudioStatus AudioStatus
		{
			get
			{
				return this.m_AudioStatus;
			}
			set
			{
				this.m_AudioStatus = value;
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00017CAC File Offset: 0x00015EAC
		public void Set(ref ParticipantUpdatedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Speaking = other.Speaking;
			this.AudioStatus = other.AudioStatus;
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00017D08 File Offset: 0x00015F08
		public void Set(ref ParticipantUpdatedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.Speaking = other.Value.Speaking;
				this.AudioStatus = other.Value.AudioStatus;
			}
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00017DA3 File Offset: 0x00015FA3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00017DD6 File Offset: 0x00015FD6
		public void Get(out ParticipantUpdatedCallbackInfo output)
		{
			output = default(ParticipantUpdatedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000731 RID: 1841
		private IntPtr m_ClientData;

		// Token: 0x04000732 RID: 1842
		private IntPtr m_LocalUserId;

		// Token: 0x04000733 RID: 1843
		private IntPtr m_RoomName;

		// Token: 0x04000734 RID: 1844
		private IntPtr m_ParticipantId;

		// Token: 0x04000735 RID: 1845
		private int m_Speaking;

		// Token: 0x04000736 RID: 1846
		private RTCAudioStatus m_AudioStatus;
	}
}
