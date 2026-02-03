using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B1 RID: 1457
	public struct RequestToJoinReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x000377C2 File Offset: 0x000359C2
		// (set) Token: 0x060025CA RID: 9674 RVA: 0x000377CA File Offset: 0x000359CA
		public object ClientData { get; set; }

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x060025CB RID: 9675 RVA: 0x000377D3 File Offset: 0x000359D3
		// (set) Token: 0x060025CC RID: 9676 RVA: 0x000377DB File Offset: 0x000359DB
		public ProductUserId FromUserId { get; set; }

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x060025CD RID: 9677 RVA: 0x000377E4 File Offset: 0x000359E4
		// (set) Token: 0x060025CE RID: 9678 RVA: 0x000377EC File Offset: 0x000359EC
		public ProductUserId ToUserId { get; set; }

		// Token: 0x060025CF RID: 9679 RVA: 0x000377F8 File Offset: 0x000359F8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00037813 File Offset: 0x00035A13
		internal void Set(ref RequestToJoinReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.FromUserId = other.FromUserId;
			this.ToUserId = other.ToUserId;
		}
	}
}
