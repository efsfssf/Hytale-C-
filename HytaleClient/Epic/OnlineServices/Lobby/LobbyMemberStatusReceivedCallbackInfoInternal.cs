using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D8 RID: 984
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyMemberStatusReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyMemberStatusReceivedCallbackInfo>, ISettable<LobbyMemberStatusReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x000281EC File Offset: 0x000263EC
		// (set) Token: 0x06001AC3 RID: 6851 RVA: 0x0002820D File Offset: 0x0002640D
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

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x00028220 File Offset: 0x00026420
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x00028238 File Offset: 0x00026438
		// (set) Token: 0x06001AC6 RID: 6854 RVA: 0x00028259 File Offset: 0x00026459
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

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x0002826C File Offset: 0x0002646C
		// (set) Token: 0x06001AC8 RID: 6856 RVA: 0x0002828D File Offset: 0x0002648D
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

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x000282A0 File Offset: 0x000264A0
		// (set) Token: 0x06001ACA RID: 6858 RVA: 0x000282B8 File Offset: 0x000264B8
		public LobbyMemberStatus CurrentStatus
		{
			get
			{
				return this.m_CurrentStatus;
			}
			set
			{
				this.m_CurrentStatus = value;
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x000282C2 File Offset: 0x000264C2
		public void Set(ref LobbyMemberStatusReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.TargetUserId = other.TargetUserId;
			this.CurrentStatus = other.CurrentStatus;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x000282FC File Offset: 0x000264FC
		public void Set(ref LobbyMemberStatusReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
				this.TargetUserId = other.Value.TargetUserId;
				this.CurrentStatus = other.Value.CurrentStatus;
			}
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0002836A File Offset: 0x0002656A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x00028391 File Offset: 0x00026591
		public void Get(out LobbyMemberStatusReceivedCallbackInfo output)
		{
			output = default(LobbyMemberStatusReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000BF5 RID: 3061
		private IntPtr m_ClientData;

		// Token: 0x04000BF6 RID: 3062
		private IntPtr m_LobbyId;

		// Token: 0x04000BF7 RID: 3063
		private IntPtr m_TargetUserId;

		// Token: 0x04000BF8 RID: 3064
		private LobbyMemberStatus m_CurrentStatus;
	}
}
