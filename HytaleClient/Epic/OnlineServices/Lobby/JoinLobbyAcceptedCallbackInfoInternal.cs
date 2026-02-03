using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000398 RID: 920
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinLobbyAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinLobbyAcceptedCallbackInfo>, ISettable<JoinLobbyAcceptedCallbackInfo>, IDisposable
	{
		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x00024168 File Offset: 0x00022368
		// (set) Token: 0x060018AC RID: 6316 RVA: 0x00024189 File Offset: 0x00022389
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

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0002419C File Offset: 0x0002239C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x000241B4 File Offset: 0x000223B4
		// (set) Token: 0x060018AF RID: 6319 RVA: 0x000241D5 File Offset: 0x000223D5
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

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060018B0 RID: 6320 RVA: 0x000241E8 File Offset: 0x000223E8
		// (set) Token: 0x060018B1 RID: 6321 RVA: 0x00024200 File Offset: 0x00022400
		public ulong UiEventId
		{
			get
			{
				return this.m_UiEventId;
			}
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0002420A File Offset: 0x0002240A
		public void Set(ref JoinLobbyAcceptedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.UiEventId = other.UiEventId;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00024234 File Offset: 0x00022434
		public void Set(ref JoinLobbyAcceptedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.UiEventId = other.Value.UiEventId;
			}
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0002428D File Offset: 0x0002248D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x000242A8 File Offset: 0x000224A8
		public void Get(out JoinLobbyAcceptedCallbackInfo output)
		{
			output = default(JoinLobbyAcceptedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000AEE RID: 2798
		private IntPtr m_ClientData;

		// Token: 0x04000AEF RID: 2799
		private IntPtr m_LocalUserId;

		// Token: 0x04000AF0 RID: 2800
		private ulong m_UiEventId;
	}
}
