using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000332 RID: 818
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EnumerateModsOptionsInternal : ISettable<EnumerateModsOptions>, IDisposable
	{
		// Token: 0x1700061E RID: 1566
		// (set) Token: 0x06001669 RID: 5737 RVA: 0x00020B19 File Offset: 0x0001ED19
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700061F RID: 1567
		// (set) Token: 0x0600166A RID: 5738 RVA: 0x00020B29 File Offset: 0x0001ED29
		public ModEnumerationType Type
		{
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00020B33 File Offset: 0x0001ED33
		public void Set(ref EnumerateModsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Type = other.Type;
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00020B58 File Offset: 0x0001ED58
		public void Set(ref EnumerateModsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Type = other.Value.Type;
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00020BA3 File Offset: 0x0001EDA3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040009C7 RID: 2503
		private int m_ApiVersion;

		// Token: 0x040009C8 RID: 2504
		private IntPtr m_LocalUserId;

		// Token: 0x040009C9 RID: 2505
		private ModEnumerationType m_Type;
	}
}
