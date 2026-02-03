using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A6 RID: 934
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct KickMemberCallbackInfoInternal : ICallbackInfoInternal, IGettable<KickMemberCallbackInfo>, ISettable<KickMemberCallbackInfo>, IDisposable
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x00024D9C File Offset: 0x00022F9C
		// (set) Token: 0x0600192E RID: 6446 RVA: 0x00024DB4 File Offset: 0x00022FB4
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

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600192F RID: 6447 RVA: 0x00024DC0 File Offset: 0x00022FC0
		// (set) Token: 0x06001930 RID: 6448 RVA: 0x00024DE1 File Offset: 0x00022FE1
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

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x00024DF4 File Offset: 0x00022FF4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001932 RID: 6450 RVA: 0x00024E0C File Offset: 0x0002300C
		// (set) Token: 0x06001933 RID: 6451 RVA: 0x00024E2D File Offset: 0x0002302D
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

		// Token: 0x06001934 RID: 6452 RVA: 0x00024E3D File Offset: 0x0002303D
		public void Set(ref KickMemberCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x00024E68 File Offset: 0x00023068
		public void Set(ref KickMemberCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x00024EC1 File Offset: 0x000230C1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00024EDC File Offset: 0x000230DC
		public void Get(out KickMemberCallbackInfo output)
		{
			output = default(KickMemberCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B27 RID: 2855
		private Result m_ResultCode;

		// Token: 0x04000B28 RID: 2856
		private IntPtr m_ClientData;

		// Token: 0x04000B29 RID: 2857
		private IntPtr m_LobbyId;
	}
}
