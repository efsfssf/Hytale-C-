using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005DA RID: 1498
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateUserOptionsInternal : ISettable<CreateUserOptions>, IDisposable
	{
		// Token: 0x17000B50 RID: 2896
		// (set) Token: 0x060026EA RID: 9962 RVA: 0x0003997F File Offset: 0x00037B7F
		public ContinuanceToken ContinuanceToken
		{
			set
			{
				Helper.Set(value, ref this.m_ContinuanceToken);
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x0003998F File Offset: 0x00037B8F
		public void Set(ref CreateUserOptions other)
		{
			this.m_ApiVersion = 1;
			this.ContinuanceToken = other.ContinuanceToken;
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000399A8 File Offset: 0x00037BA8
		public void Set(ref CreateUserOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ContinuanceToken = other.Value.ContinuanceToken;
			}
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000399DE File Offset: 0x00037BDE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ContinuanceToken);
		}

		// Token: 0x040010E2 RID: 4322
		private int m_ApiVersion;

		// Token: 0x040010E3 RID: 4323
		private IntPtr m_ContinuanceToken;
	}
}
