using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004AB RID: 1195
	public struct RequestPermissionsOptions
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0002DBBE File Offset: 0x0002BDBE
		// (set) Token: 0x06001F3F RID: 7999 RVA: 0x0002DBC6 File Offset: 0x0002BDC6
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x0002DBCF File Offset: 0x0002BDCF
		// (set) Token: 0x06001F41 RID: 8001 RVA: 0x0002DBD7 File Offset: 0x0002BDD7
		public Utf8String[] PermissionKeys { get; set; }
	}
}
