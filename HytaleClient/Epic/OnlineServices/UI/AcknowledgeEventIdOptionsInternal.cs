using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200004E RID: 78
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AcknowledgeEventIdOptionsInternal : ISettable<AcknowledgeEventIdOptions>, IDisposable
	{
		// Token: 0x17000085 RID: 133
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x000069ED File Offset: 0x00004BED
		public ulong UiEventId
		{
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x000069F7 File Offset: 0x00004BF7
		public Result Result
		{
			set
			{
				this.m_Result = value;
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00006A01 File Offset: 0x00004C01
		public void Set(ref AcknowledgeEventIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.UiEventId = other.UiEventId;
			this.Result = other.Result;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00006A28 File Offset: 0x00004C28
		public void Set(ref AcknowledgeEventIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UiEventId = other.Value.UiEventId;
				this.Result = other.Value.Result;
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00006A73 File Offset: 0x00004C73
		public void Dispose()
		{
		}

		// Token: 0x040001D9 RID: 473
		private int m_ApiVersion;

		// Token: 0x040001DA RID: 474
		private ulong m_UiEventId;

		// Token: 0x040001DB RID: 475
		private Result m_Result;
	}
}
