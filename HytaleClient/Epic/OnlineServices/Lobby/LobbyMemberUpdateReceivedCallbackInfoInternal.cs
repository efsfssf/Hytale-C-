using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003DA RID: 986
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyMemberUpdateReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyMemberUpdateReceivedCallbackInfo>, ISettable<LobbyMemberUpdateReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x00028420 File Offset: 0x00026620
		// (set) Token: 0x06001AD8 RID: 6872 RVA: 0x00028441 File Offset: 0x00026641
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

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x00028454 File Offset: 0x00026654
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x0002846C File Offset: 0x0002666C
		// (set) Token: 0x06001ADB RID: 6875 RVA: 0x0002848D File Offset: 0x0002668D
		public Utf8String LobbyId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LobbyId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06001ADC RID: 6876 RVA: 0x000284A0 File Offset: 0x000266A0
		// (set) Token: 0x06001ADD RID: 6877 RVA: 0x000284C1 File Offset: 0x000266C1
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

		// Token: 0x06001ADE RID: 6878 RVA: 0x000284D1 File Offset: 0x000266D1
		public void Set(ref LobbyMemberUpdateReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x000284FC File Offset: 0x000266FC
		public void Set(ref LobbyMemberUpdateReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00028555 File Offset: 0x00026755
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x0002857C File Offset: 0x0002677C
		public void Get(out LobbyMemberUpdateReceivedCallbackInfo output)
		{
			output = default(LobbyMemberUpdateReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000BFC RID: 3068
		private IntPtr m_ClientData;

		// Token: 0x04000BFD RID: 3069
		private IntPtr m_LobbyId;

		// Token: 0x04000BFE RID: 3070
		private IntPtr m_TargetUserId;
	}
}
