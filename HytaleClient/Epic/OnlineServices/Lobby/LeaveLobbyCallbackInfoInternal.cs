using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003AA RID: 938
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveLobbyCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveLobbyCallbackInfo>, ISettable<LeaveLobbyCallbackInfo>, IDisposable
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600194C RID: 6476 RVA: 0x00025088 File Offset: 0x00023288
		// (set) Token: 0x0600194D RID: 6477 RVA: 0x000250A0 File Offset: 0x000232A0
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x000250AC File Offset: 0x000232AC
		// (set) Token: 0x0600194F RID: 6479 RVA: 0x000250CD File Offset: 0x000232CD
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

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001950 RID: 6480 RVA: 0x000250E0 File Offset: 0x000232E0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x000250F8 File Offset: 0x000232F8
		// (set) Token: 0x06001952 RID: 6482 RVA: 0x00025119 File Offset: 0x00023319
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

		// Token: 0x06001953 RID: 6483 RVA: 0x00025129 File Offset: 0x00023329
		public void Set(ref LeaveLobbyCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x00025154 File Offset: 0x00023354
		public void Set(ref LeaveLobbyCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x000251AD File Offset: 0x000233AD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x000251C8 File Offset: 0x000233C8
		public void Get(out LeaveLobbyCallbackInfo output)
		{
			output = default(LeaveLobbyCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B34 RID: 2868
		private Result m_ResultCode;

		// Token: 0x04000B35 RID: 2869
		private IntPtr m_ClientData;

		// Token: 0x04000B36 RID: 2870
		private IntPtr m_LobbyId;
	}
}
