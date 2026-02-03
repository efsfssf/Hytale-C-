using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A2 RID: 930
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinRTCRoomCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinRTCRoomCallbackInfo>, ISettable<JoinRTCRoomCallbackInfo>, IDisposable
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600190E RID: 6414 RVA: 0x00024AB0 File Offset: 0x00022CB0
		// (set) Token: 0x0600190F RID: 6415 RVA: 0x00024AC8 File Offset: 0x00022CC8
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

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001910 RID: 6416 RVA: 0x00024AD4 File Offset: 0x00022CD4
		// (set) Token: 0x06001911 RID: 6417 RVA: 0x00024AF5 File Offset: 0x00022CF5
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

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001912 RID: 6418 RVA: 0x00024B08 File Offset: 0x00022D08
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001913 RID: 6419 RVA: 0x00024B20 File Offset: 0x00022D20
		// (set) Token: 0x06001914 RID: 6420 RVA: 0x00024B41 File Offset: 0x00022D41
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

		// Token: 0x06001915 RID: 6421 RVA: 0x00024B51 File Offset: 0x00022D51
		public void Set(ref JoinRTCRoomCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00024B7C File Offset: 0x00022D7C
		public void Set(ref JoinRTCRoomCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x00024BD5 File Offset: 0x00022DD5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x00024BF0 File Offset: 0x00022DF0
		public void Get(out JoinRTCRoomCallbackInfo output)
		{
			output = default(JoinRTCRoomCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B1A RID: 2842
		private Result m_ResultCode;

		// Token: 0x04000B1B RID: 2843
		private IntPtr m_ClientData;

		// Token: 0x04000B1C RID: 2844
		private IntPtr m_LobbyId;
	}
}
