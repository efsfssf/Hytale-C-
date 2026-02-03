using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000293 RID: 659
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryJoinRoomTokenCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryJoinRoomTokenCompleteCallbackInfo>, ISettable<QueryJoinRoomTokenCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0001AC18 File Offset: 0x00018E18
		// (set) Token: 0x06001270 RID: 4720 RVA: 0x0001AC30 File Offset: 0x00018E30
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

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x0001AC3C File Offset: 0x00018E3C
		// (set) Token: 0x06001272 RID: 4722 RVA: 0x0001AC5D File Offset: 0x00018E5D
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

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x0001AC70 File Offset: 0x00018E70
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x0001AC88 File Offset: 0x00018E88
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x0001ACA9 File Offset: 0x00018EA9
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

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x0001ACBC File Offset: 0x00018EBC
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x0001ACDD File Offset: 0x00018EDD
		public Utf8String ClientBaseUrl
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ClientBaseUrl, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientBaseUrl);
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0001ACF0 File Offset: 0x00018EF0
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x0001AD08 File Offset: 0x00018F08
		public uint QueryId
		{
			get
			{
				return this.m_QueryId;
			}
			set
			{
				this.m_QueryId = value;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0001AD14 File Offset: 0x00018F14
		// (set) Token: 0x0600127B RID: 4731 RVA: 0x0001AD2C File Offset: 0x00018F2C
		public uint TokenCount
		{
			get
			{
				return this.m_TokenCount;
			}
			set
			{
				this.m_TokenCount = value;
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0001AD38 File Offset: 0x00018F38
		public void Set(ref QueryJoinRoomTokenCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RoomName = other.RoomName;
			this.ClientBaseUrl = other.ClientBaseUrl;
			this.QueryId = other.QueryId;
			this.TokenCount = other.TokenCount;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0001AD94 File Offset: 0x00018F94
		public void Set(ref QueryJoinRoomTokenCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.RoomName = other.Value.RoomName;
				this.ClientBaseUrl = other.Value.ClientBaseUrl;
				this.QueryId = other.Value.QueryId;
				this.TokenCount = other.Value.TokenCount;
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0001AE2F File Offset: 0x0001902F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ClientBaseUrl);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0001AE56 File Offset: 0x00019056
		public void Get(out QueryJoinRoomTokenCompleteCallbackInfo output)
		{
			output = default(QueryJoinRoomTokenCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000811 RID: 2065
		private Result m_ResultCode;

		// Token: 0x04000812 RID: 2066
		private IntPtr m_ClientData;

		// Token: 0x04000813 RID: 2067
		private IntPtr m_RoomName;

		// Token: 0x04000814 RID: 2068
		private IntPtr m_ClientBaseUrl;

		// Token: 0x04000815 RID: 2069
		private uint m_QueryId;

		// Token: 0x04000816 RID: 2070
		private uint m_TokenCount;
	}
}
