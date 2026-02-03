using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200039A RID: 922
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinLobbyByIdCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinLobbyByIdCallbackInfo>, ISettable<JoinLobbyByIdCallbackInfo>, IDisposable
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060018BE RID: 6334 RVA: 0x00024338 File Offset: 0x00022538
		// (set) Token: 0x060018BF RID: 6335 RVA: 0x00024350 File Offset: 0x00022550
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

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x0002435C File Offset: 0x0002255C
		// (set) Token: 0x060018C1 RID: 6337 RVA: 0x0002437D File Offset: 0x0002257D
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

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x00024390 File Offset: 0x00022590
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x000243A8 File Offset: 0x000225A8
		// (set) Token: 0x060018C4 RID: 6340 RVA: 0x000243C9 File Offset: 0x000225C9
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

		// Token: 0x060018C5 RID: 6341 RVA: 0x000243D9 File Offset: 0x000225D9
		public void Set(ref JoinLobbyByIdCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x00024404 File Offset: 0x00022604
		public void Set(ref JoinLobbyByIdCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0002445D File Offset: 0x0002265D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x00024478 File Offset: 0x00022678
		public void Get(out JoinLobbyByIdCallbackInfo output)
		{
			output = default(JoinLobbyByIdCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000AF4 RID: 2804
		private Result m_ResultCode;

		// Token: 0x04000AF5 RID: 2805
		private IntPtr m_ClientData;

		// Token: 0x04000AF6 RID: 2806
		private IntPtr m_LobbyId;
	}
}
