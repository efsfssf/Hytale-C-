using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000C9 RID: 201
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IngestDataInternal : IGettable<IngestData>, ISettable<IngestData>, IDisposable
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0000A9A0 File Offset: 0x00008BA0
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x0000A9C1 File Offset: 0x00008BC1
		public Utf8String StatName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_StatName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_StatName);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0000A9D4 File Offset: 0x00008BD4
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0000A9EC File Offset: 0x00008BEC
		public int IngestAmount
		{
			get
			{
				return this.m_IngestAmount;
			}
			set
			{
				this.m_IngestAmount = value;
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000A9F6 File Offset: 0x00008BF6
		public void Set(ref IngestData other)
		{
			this.m_ApiVersion = 1;
			this.StatName = other.StatName;
			this.IngestAmount = other.IngestAmount;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0000AA1C File Offset: 0x00008C1C
		public void Set(ref IngestData? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.StatName = other.Value.StatName;
				this.IngestAmount = other.Value.IngestAmount;
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0000AA67 File Offset: 0x00008C67
		public void Dispose()
		{
			Helper.Dispose(ref this.m_StatName);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000AA76 File Offset: 0x00008C76
		public void Get(out IngestData output)
		{
			output = default(IngestData);
			output.Set(ref this);
		}

		// Token: 0x04000389 RID: 905
		private int m_ApiVersion;

		// Token: 0x0400038A RID: 906
		private IntPtr m_StatName;

		// Token: 0x0400038B RID: 907
		private int m_IngestAmount;
	}
}
