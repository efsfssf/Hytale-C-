using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B4 RID: 1460
	public struct RequestToJoinResponseReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x060025DC RID: 9692 RVA: 0x000379AE File Offset: 0x00035BAE
		// (set) Token: 0x060025DD RID: 9693 RVA: 0x000379B6 File Offset: 0x00035BB6
		public object ClientData { get; set; }

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x000379BF File Offset: 0x00035BBF
		// (set) Token: 0x060025DF RID: 9695 RVA: 0x000379C7 File Offset: 0x00035BC7
		public ProductUserId FromUserId { get; set; }

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x000379D0 File Offset: 0x00035BD0
		// (set) Token: 0x060025E1 RID: 9697 RVA: 0x000379D8 File Offset: 0x00035BD8
		public ProductUserId ToUserId { get; set; }

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x000379E1 File Offset: 0x00035BE1
		// (set) Token: 0x060025E3 RID: 9699 RVA: 0x000379E9 File Offset: 0x00035BE9
		public RequestToJoinResponse Response { get; set; }

		// Token: 0x060025E4 RID: 9700 RVA: 0x000379F4 File Offset: 0x00035BF4
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00037A0F File Offset: 0x00035C0F
		internal void Set(ref RequestToJoinResponseReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.FromUserId = other.FromUserId;
			this.ToUserId = other.ToUserId;
			this.Response = other.Response;
		}
	}
}
