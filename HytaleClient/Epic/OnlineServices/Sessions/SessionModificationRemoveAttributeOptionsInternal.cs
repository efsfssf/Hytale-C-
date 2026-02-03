using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000164 RID: 356
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationRemoveAttributeOptionsInternal : ISettable<SessionModificationRemoveAttributeOptions>, IDisposable
	{
		// Token: 0x17000273 RID: 627
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x0000F5EF File Offset: 0x0000D7EF
		public Utf8String Key
		{
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0000F5FF File Offset: 0x0000D7FF
		public void Set(ref SessionModificationRemoveAttributeOptions other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0000F618 File Offset: 0x0000D818
		public void Set(ref SessionModificationRemoveAttributeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
			}
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0000F64E File Offset: 0x0000D84E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
		}

		// Token: 0x040004EC RID: 1260
		private int m_ApiVersion;

		// Token: 0x040004ED RID: 1261
		private IntPtr m_Key;
	}
}
