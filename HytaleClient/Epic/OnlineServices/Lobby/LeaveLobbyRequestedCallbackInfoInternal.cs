using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003AE RID: 942
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveLobbyRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveLobbyRequestedCallbackInfo>, ISettable<LeaveLobbyRequestedCallbackInfo>, IDisposable
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x00025324 File Offset: 0x00023524
		// (set) Token: 0x06001969 RID: 6505 RVA: 0x00025345 File Offset: 0x00023545
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

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x00025358 File Offset: 0x00023558
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x00025370 File Offset: 0x00023570
		// (set) Token: 0x0600196C RID: 6508 RVA: 0x00025391 File Offset: 0x00023591
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

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x000253A4 File Offset: 0x000235A4
		// (set) Token: 0x0600196E RID: 6510 RVA: 0x000253C5 File Offset: 0x000235C5
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

		// Token: 0x0600196F RID: 6511 RVA: 0x000253D5 File Offset: 0x000235D5
		public void Set(ref LeaveLobbyRequestedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00025400 File Offset: 0x00023600
		public void Set(ref LeaveLobbyRequestedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00025459 File Offset: 0x00023659
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00025480 File Offset: 0x00023680
		public void Get(out LeaveLobbyRequestedCallbackInfo output)
		{
			output = default(LeaveLobbyRequestedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B3F RID: 2879
		private IntPtr m_ClientData;

		// Token: 0x04000B40 RID: 2880
		private IntPtr m_LocalUserId;

		// Token: 0x04000B41 RID: 2881
		private IntPtr m_LobbyId;
	}
}
