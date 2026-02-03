using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200039E RID: 926
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinLobbyCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinLobbyCallbackInfo>, ISettable<JoinLobbyCallbackInfo>, IDisposable
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x000246F4 File Offset: 0x000228F4
		// (set) Token: 0x060018E7 RID: 6375 RVA: 0x0002470C File Offset: 0x0002290C
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

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x00024718 File Offset: 0x00022918
		// (set) Token: 0x060018E9 RID: 6377 RVA: 0x00024739 File Offset: 0x00022939
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

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x0002474C File Offset: 0x0002294C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x00024764 File Offset: 0x00022964
		// (set) Token: 0x060018EC RID: 6380 RVA: 0x00024785 File Offset: 0x00022985
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

		// Token: 0x060018ED RID: 6381 RVA: 0x00024795 File Offset: 0x00022995
		public void Set(ref JoinLobbyCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x000247C0 File Offset: 0x000229C0
		public void Set(ref JoinLobbyCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00024819 File Offset: 0x00022A19
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00024834 File Offset: 0x00022A34
		public void Get(out JoinLobbyCallbackInfo output)
		{
			output = default(JoinLobbyCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B07 RID: 2823
		private Result m_ResultCode;

		// Token: 0x04000B08 RID: 2824
		private IntPtr m_ClientData;

		// Token: 0x04000B09 RID: 2825
		private IntPtr m_LobbyId;
	}
}
