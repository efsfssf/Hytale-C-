using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000063 RID: 99
	public struct MemoryMonitorCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00006EAC File Offset: 0x000050AC
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00006EB4 File Offset: 0x000050B4
		public object ClientData { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00006EBD File Offset: 0x000050BD
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x00006EC5 File Offset: 0x000050C5
		public IntPtr SystemMemoryMonitorReport { get; set; }

		// Token: 0x060004C1 RID: 1217 RVA: 0x00006ED0 File Offset: 0x000050D0
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00006EEB File Offset: 0x000050EB
		internal void Set(ref MemoryMonitorCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.SystemMemoryMonitorReport = other.SystemMemoryMonitorReport;
		}
	}
}
