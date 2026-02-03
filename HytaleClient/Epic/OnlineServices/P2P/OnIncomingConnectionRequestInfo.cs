using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200078B RID: 1931
	public struct OnIncomingConnectionRequestInfo : ICallbackInfo
	{
		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x0004A9C8 File Offset: 0x00048BC8
		// (set) Token: 0x060031E5 RID: 12773 RVA: 0x0004A9D0 File Offset: 0x00048BD0
		public object ClientData { get; set; }

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x060031E6 RID: 12774 RVA: 0x0004A9D9 File Offset: 0x00048BD9
		// (set) Token: 0x060031E7 RID: 12775 RVA: 0x0004A9E1 File Offset: 0x00048BE1
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x0004A9EA File Offset: 0x00048BEA
		// (set) Token: 0x060031E9 RID: 12777 RVA: 0x0004A9F2 File Offset: 0x00048BF2
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x0004A9FB File Offset: 0x00048BFB
		// (set) Token: 0x060031EB RID: 12779 RVA: 0x0004AA03 File Offset: 0x00048C03
		public SocketId? SocketId { get; set; }

		// Token: 0x060031EC RID: 12780 RVA: 0x0004AA0C File Offset: 0x00048C0C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x0004AA27 File Offset: 0x00048C27
		internal void Set(ref OnIncomingConnectionRequestInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
		}
	}
}
