using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000064 RID: 100
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct MemoryMonitorCallbackInfoInternal : ICallbackInfoInternal, IGettable<MemoryMonitorCallbackInfo>, ISettable<MemoryMonitorCallbackInfo>, IDisposable
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00006F08 File Offset: 0x00005108
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x00006F29 File Offset: 0x00005129
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00006F3C File Offset: 0x0000513C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00006F54 File Offset: 0x00005154
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00006F6C File Offset: 0x0000516C
		public IntPtr SystemMemoryMonitorReport
		{
			get
			{
				return this.m_SystemMemoryMonitorReport;
			}
			set
			{
				this.m_SystemMemoryMonitorReport = value;
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00006F76 File Offset: 0x00005176
		public void Set(ref MemoryMonitorCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.SystemMemoryMonitorReport = other.SystemMemoryMonitorReport;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00006F94 File Offset: 0x00005194
		public void Set(ref MemoryMonitorCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.SystemMemoryMonitorReport = other.Value.SystemMemoryMonitorReport;
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00006FD8 File Offset: 0x000051D8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_SystemMemoryMonitorReport);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00006FF3 File Offset: 0x000051F3
		public void Get(out MemoryMonitorCallbackInfo output)
		{
			output = default(MemoryMonitorCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000274 RID: 628
		private IntPtr m_ClientData;

		// Token: 0x04000275 RID: 629
		private IntPtr m_SystemMemoryMonitorReport;
	}
}
