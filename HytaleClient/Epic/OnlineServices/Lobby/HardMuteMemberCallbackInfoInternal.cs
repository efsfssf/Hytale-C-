using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000392 RID: 914
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct HardMuteMemberCallbackInfoInternal : ICallbackInfoInternal, IGettable<HardMuteMemberCallbackInfo>, ISettable<HardMuteMemberCallbackInfo>, IDisposable
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x00023D08 File Offset: 0x00021F08
		// (set) Token: 0x0600187F RID: 6271 RVA: 0x00023D20 File Offset: 0x00021F20
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

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001880 RID: 6272 RVA: 0x00023D2C File Offset: 0x00021F2C
		// (set) Token: 0x06001881 RID: 6273 RVA: 0x00023D4D File Offset: 0x00021F4D
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

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001882 RID: 6274 RVA: 0x00023D60 File Offset: 0x00021F60
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x00023D78 File Offset: 0x00021F78
		// (set) Token: 0x06001884 RID: 6276 RVA: 0x00023D99 File Offset: 0x00021F99
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

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x00023DAC File Offset: 0x00021FAC
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x00023DCD File Offset: 0x00021FCD
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

		// Token: 0x06001887 RID: 6279 RVA: 0x00023DDD File Offset: 0x00021FDD
		public void Set(ref HardMuteMemberCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00023E14 File Offset: 0x00022014
		public void Set(ref HardMuteMemberCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00023E82 File Offset: 0x00022082
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00023EA9 File Offset: 0x000220A9
		public void Get(out HardMuteMemberCallbackInfo output)
		{
			output = default(HardMuteMemberCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000AD9 RID: 2777
		private Result m_ResultCode;

		// Token: 0x04000ADA RID: 2778
		private IntPtr m_ClientData;

		// Token: 0x04000ADB RID: 2779
		private IntPtr m_LobbyId;

		// Token: 0x04000ADC RID: 2780
		private IntPtr m_TargetUserId;
	}
}
