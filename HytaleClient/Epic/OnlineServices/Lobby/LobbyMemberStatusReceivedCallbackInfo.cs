using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D7 RID: 983
	public struct LobbyMemberStatusReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x00028154 File Offset: 0x00026354
		// (set) Token: 0x06001AB9 RID: 6841 RVA: 0x0002815C File Offset: 0x0002635C
		public object ClientData { get; set; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001ABA RID: 6842 RVA: 0x00028165 File Offset: 0x00026365
		// (set) Token: 0x06001ABB RID: 6843 RVA: 0x0002816D File Offset: 0x0002636D
		public Utf8String LobbyId { get; set; }

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x00028176 File Offset: 0x00026376
		// (set) Token: 0x06001ABD RID: 6845 RVA: 0x0002817E File Offset: 0x0002637E
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x00028187 File Offset: 0x00026387
		// (set) Token: 0x06001ABF RID: 6847 RVA: 0x0002818F File Offset: 0x0002638F
		public LobbyMemberStatus CurrentStatus { get; set; }

		// Token: 0x06001AC0 RID: 6848 RVA: 0x00028198 File Offset: 0x00026398
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000281B3 File Offset: 0x000263B3
		internal void Set(ref LobbyMemberStatusReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.TargetUserId = other.TargetUserId;
			this.CurrentStatus = other.CurrentStatus;
		}
	}
}
