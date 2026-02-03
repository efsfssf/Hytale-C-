using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200049F RID: 1183
	public struct PermissionsUpdateReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x0002CFB0 File Offset: 0x0002B1B0
		// (set) Token: 0x06001ECA RID: 7882 RVA: 0x0002CFB8 File Offset: 0x0002B1B8
		public object ClientData { get; set; }

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x0002CFC1 File Offset: 0x0002B1C1
		// (set) Token: 0x06001ECC RID: 7884 RVA: 0x0002CFC9 File Offset: 0x0002B1C9
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x0002CFD2 File Offset: 0x0002B1D2
		// (set) Token: 0x06001ECE RID: 7886 RVA: 0x0002CFDA File Offset: 0x0002B1DA
		public Utf8String KWSUserId { get; set; }

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x0002CFE3 File Offset: 0x0002B1E3
		// (set) Token: 0x06001ED0 RID: 7888 RVA: 0x0002CFEB File Offset: 0x0002B1EB
		public Utf8String DateOfBirth { get; set; }

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x0002CFF4 File Offset: 0x0002B1F4
		// (set) Token: 0x06001ED2 RID: 7890 RVA: 0x0002CFFC File Offset: 0x0002B1FC
		public bool IsMinor { get; set; }

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x0002D005 File Offset: 0x0002B205
		// (set) Token: 0x06001ED4 RID: 7892 RVA: 0x0002D00D File Offset: 0x0002B20D
		public Utf8String ParentEmail { get; set; }

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0002D018 File Offset: 0x0002B218
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0002D034 File Offset: 0x0002B234
		internal void Set(ref PermissionsUpdateReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.KWSUserId = other.KWSUserId;
			this.DateOfBirth = other.DateOfBirth;
			this.IsMinor = other.IsMinor;
			this.ParentEmail = other.ParentEmail;
		}
	}
}
