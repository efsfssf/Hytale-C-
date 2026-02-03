using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000596 RID: 1430
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnCustomInviteReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnCustomInviteReceivedCallbackInfo>, ISettable<OnCustomInviteReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x0600252C RID: 9516 RVA: 0x00036E90 File Offset: 0x00035090
		// (set) Token: 0x0600252D RID: 9517 RVA: 0x00036EB1 File Offset: 0x000350B1
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

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x0600252E RID: 9518 RVA: 0x00036EC4 File Offset: 0x000350C4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x00036EDC File Offset: 0x000350DC
		// (set) Token: 0x06002530 RID: 9520 RVA: 0x00036EFD File Offset: 0x000350FD
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

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x00036F10 File Offset: 0x00035110
		// (set) Token: 0x06002532 RID: 9522 RVA: 0x00036F31 File Offset: 0x00035131
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

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x00036F44 File Offset: 0x00035144
		// (set) Token: 0x06002534 RID: 9524 RVA: 0x00036F65 File Offset: 0x00035165
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

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x00036F78 File Offset: 0x00035178
		// (set) Token: 0x06002536 RID: 9526 RVA: 0x00036F99 File Offset: 0x00035199
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

		// Token: 0x06002537 RID: 9527 RVA: 0x00036FAC File Offset: 0x000351AC
		public void Set(ref OnCustomInviteReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
			this.CustomInviteId = other.CustomInviteId;
			this.Payload = other.Payload;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00036FFC File Offset: 0x000351FC
		public void Set(ref OnCustomInviteReceivedCallbackInfo? other)
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

		// Token: 0x06002539 RID: 9529 RVA: 0x0003707F File Offset: 0x0003527F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_CustomInviteId);
			Helper.Dispose(ref this.m_Payload);
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000370BE File Offset: 0x000352BE
		public void Get(out OnCustomInviteReceivedCallbackInfo output)
		{
			output = default(OnCustomInviteReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400103D RID: 4157
		private IntPtr m_ClientData;

		// Token: 0x0400103E RID: 4158
		private IntPtr m_TargetUserId;

		// Token: 0x0400103F RID: 4159
		private IntPtr m_LocalUserId;

		// Token: 0x04001040 RID: 4160
		private IntPtr m_CustomInviteId;

		// Token: 0x04001041 RID: 4161
		private IntPtr m_Payload;
	}
}
