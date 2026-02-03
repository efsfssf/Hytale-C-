using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005EE RID: 1518
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LinkAccountOptionsInternal : ISettable<LinkAccountOptions>, IDisposable
	{
		// Token: 0x17000B7F RID: 2943
		// (set) Token: 0x06002768 RID: 10088 RVA: 0x0003A518 File Offset: 0x00038718
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (set) Token: 0x06002769 RID: 10089 RVA: 0x0003A528 File Offset: 0x00038728
		public ContinuanceToken ContinuanceToken
		{
			set
			{
				Helper.Set(value, ref this.m_ContinuanceToken);
			}
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x0003A538 File Offset: 0x00038738
		public void Set(ref LinkAccountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ContinuanceToken = other.ContinuanceToken;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x0003A55C File Offset: 0x0003875C
		public void Set(ref LinkAccountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ContinuanceToken = other.Value.ContinuanceToken;
			}
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x0003A5A7 File Offset: 0x000387A7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ContinuanceToken);
		}

		// Token: 0x04001117 RID: 4375
		private int m_ApiVersion;

		// Token: 0x04001118 RID: 4376
		private IntPtr m_LocalUserId;

		// Token: 0x04001119 RID: 4377
		private IntPtr m_ContinuanceToken;
	}
}
