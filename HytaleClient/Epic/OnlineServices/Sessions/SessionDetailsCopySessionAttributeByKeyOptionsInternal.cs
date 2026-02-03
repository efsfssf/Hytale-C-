using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000153 RID: 339
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionDetailsCopySessionAttributeByKeyOptionsInternal : ISettable<SessionDetailsCopySessionAttributeByKeyOptions>, IDisposable
	{
		// Token: 0x17000234 RID: 564
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x0000E302 File Offset: 0x0000C502
		public Utf8String AttrKey
		{
			set
			{
				Helper.Set(value, ref this.m_AttrKey);
			}
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0000E312 File Offset: 0x0000C512
		public void Set(ref SessionDetailsCopySessionAttributeByKeyOptions other)
		{
			this.m_ApiVersion = 1;
			this.AttrKey = other.AttrKey;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0000E32C File Offset: 0x0000C52C
		public void Set(ref SessionDetailsCopySessionAttributeByKeyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AttrKey = other.Value.AttrKey;
			}
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0000E362 File Offset: 0x0000C562
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AttrKey);
		}

		// Token: 0x0400049D RID: 1181
		private int m_ApiVersion;

		// Token: 0x0400049E RID: 1182
		private IntPtr m_AttrKey;
	}
}
