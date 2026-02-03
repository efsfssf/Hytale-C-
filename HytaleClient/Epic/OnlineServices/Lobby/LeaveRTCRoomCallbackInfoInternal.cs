using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003B0 RID: 944
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveRTCRoomCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveRTCRoomCallbackInfo>, ISettable<LeaveRTCRoomCallbackInfo>, IDisposable
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600197B RID: 6523 RVA: 0x00025510 File Offset: 0x00023710
		// (set) Token: 0x0600197C RID: 6524 RVA: 0x00025528 File Offset: 0x00023728
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

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x0600197D RID: 6525 RVA: 0x00025534 File Offset: 0x00023734
		// (set) Token: 0x0600197E RID: 6526 RVA: 0x00025555 File Offset: 0x00023755
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

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x00025568 File Offset: 0x00023768
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x00025580 File Offset: 0x00023780
		// (set) Token: 0x06001981 RID: 6529 RVA: 0x000255A1 File Offset: 0x000237A1
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

		// Token: 0x06001982 RID: 6530 RVA: 0x000255B1 File Offset: 0x000237B1
		public void Set(ref LeaveRTCRoomCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x000255DC File Offset: 0x000237DC
		public void Set(ref LeaveRTCRoomCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00025635 File Offset: 0x00023835
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00025650 File Offset: 0x00023850
		public void Get(out LeaveRTCRoomCallbackInfo output)
		{
			output = default(LeaveRTCRoomCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B45 RID: 2885
		private Result m_ResultCode;

		// Token: 0x04000B46 RID: 2886
		private IntPtr m_ClientData;

		// Token: 0x04000B47 RID: 2887
		private IntPtr m_LobbyId;
	}
}
