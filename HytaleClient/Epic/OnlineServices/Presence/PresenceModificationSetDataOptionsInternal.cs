using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D9 RID: 729
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PresenceModificationSetDataOptionsInternal : ISettable<PresenceModificationSetDataOptions>, IDisposable
	{
		// Token: 0x1700055E RID: 1374
		// (set) Token: 0x0600141E RID: 5150 RVA: 0x0001D67E File Offset: 0x0001B87E
		public DataRecord[] Records
		{
			set
			{
				Helper.Set<DataRecord, DataRecordInternal>(ref value, ref this.m_Records, out this.m_RecordsCount);
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0001D695 File Offset: 0x0001B895
		public void Set(ref PresenceModificationSetDataOptions other)
		{
			this.m_ApiVersion = 1;
			this.Records = other.Records;
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0001D6AC File Offset: 0x0001B8AC
		public void Set(ref PresenceModificationSetDataOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Records = other.Value.Records;
			}
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0001D6E2 File Offset: 0x0001B8E2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Records);
		}

		// Token: 0x040008D6 RID: 2262
		private int m_ApiVersion;

		// Token: 0x040008D7 RID: 2263
		private int m_RecordsCount;

		// Token: 0x040008D8 RID: 2264
		private IntPtr m_Records;
	}
}
