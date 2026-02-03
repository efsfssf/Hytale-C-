using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000592 RID: 1426
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnCustomInviteAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnCustomInviteAcceptedCallbackInfo>, ISettable<OnCustomInviteAcceptedCallbackInfo>, IDisposable
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06002509 RID: 9481 RVA: 0x00036B8C File Offset: 0x00034D8C
		// (set) Token: 0x0600250A RID: 9482 RVA: 0x00036BAD File Offset: 0x00034DAD
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

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x00036BC0 File Offset: 0x00034DC0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x00036BD8 File Offset: 0x00034DD8
		// (set) Token: 0x0600250D RID: 9485 RVA: 0x00036BF9 File Offset: 0x00034DF9
		public ProductUserId TargetUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x00036C0C File Offset: 0x00034E0C
		// (set) Token: 0x0600250F RID: 9487 RVA: 0x00036C2D File Offset: 0x00034E2D
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

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x00036C40 File Offset: 0x00034E40
		// (set) Token: 0x06002511 RID: 9489 RVA: 0x00036C61 File Offset: 0x00034E61
		public Utf8String CustomInviteId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CustomInviteId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CustomInviteId);
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x00036C74 File Offset: 0x00034E74
		// (set) Token: 0x06002513 RID: 9491 RVA: 0x00036C95 File Offset: 0x00034E95
		public Utf8String Payload
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Payload, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Payload);
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x00036CA8 File Offset: 0x00034EA8
		public void Set(ref OnCustomInviteAcceptedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
			this.CustomInviteId = other.CustomInviteId;
			this.Payload = other.Payload;
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x00036CF8 File Offset: 0x00034EF8
		public void Set(ref OnCustomInviteAcceptedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
				this.CustomInviteId = other.Value.CustomInviteId;
				this.Payload = other.Value.Payload;
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x00036D7B File Offset: 0x00034F7B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_CustomInviteId);
			Helper.Dispose(ref this.m_Payload);
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x00036DBA File Offset: 0x00034FBA
		public void Get(out OnCustomInviteAcceptedCallbackInfo output)
		{
			output = default(OnCustomInviteAcceptedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001033 RID: 4147
		private IntPtr m_ClientData;

		// Token: 0x04001034 RID: 4148
		private IntPtr m_TargetUserId;

		// Token: 0x04001035 RID: 4149
		private IntPtr m_LocalUserId;

		// Token: 0x04001036 RID: 4150
		private IntPtr m_CustomInviteId;

		// Token: 0x04001037 RID: 4151
		private IntPtr m_Payload;
	}
}
