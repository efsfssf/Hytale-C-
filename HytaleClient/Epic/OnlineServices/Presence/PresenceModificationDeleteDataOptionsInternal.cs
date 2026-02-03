using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D7 RID: 727
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PresenceModificationDeleteDataOptionsInternal : ISettable<PresenceModificationDeleteDataOptions>, IDisposable
	{
		// Token: 0x1700055C RID: 1372
		// (set) Token: 0x06001418 RID: 5144 RVA: 0x0001D5F8 File Offset: 0x0001B7F8
		public PresenceModificationDataRecordId[] Records
		{
			set
			{
				Helper.Set<PresenceModificationDataRecordId, PresenceModificationDataRecordIdInternal>(ref value, ref this.m_Records, out this.m_RecordsCount);
			}
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0001D60F File Offset: 0x0001B80F
		public void Set(ref PresenceModificationDeleteDataOptions other)
		{
			this.m_ApiVersion = 1;
			this.Records = other.Records;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0001D628 File Offset: 0x0001B828
		public void Set(ref PresenceModificationDeleteDataOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Records = other.Value.Records;
			}
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0001D65E File Offset: 0x0001B85E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Records);
		}

		// Token: 0x040008D2 RID: 2258
		private int m_ApiVersion;

		// Token: 0x040008D3 RID: 2259
		private int m_RecordsCount;

		// Token: 0x040008D4 RID: 2260
		private IntPtr m_Records;
	}
}
