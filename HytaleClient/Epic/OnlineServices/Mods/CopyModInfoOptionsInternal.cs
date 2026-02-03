using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200032E RID: 814
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyModInfoOptionsInternal : ISettable<CopyModInfoOptions>, IDisposable
	{
		// Token: 0x17000611 RID: 1553
		// (set) Token: 0x06001649 RID: 5705 RVA: 0x0002082A File Offset: 0x0001EA2A
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000612 RID: 1554
		// (set) Token: 0x0600164A RID: 5706 RVA: 0x0002083A File Offset: 0x0001EA3A
		public ModEnumerationType Type
		{
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00020844 File Offset: 0x0001EA44
		public void Set(ref CopyModInfoOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Type = other.Type;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00020868 File Offset: 0x0001EA68
		public void Set(ref CopyModInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Type = other.Value.Type;
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000208B3 File Offset: 0x0001EAB3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040009BA RID: 2490
		private int m_ApiVersion;

		// Token: 0x040009BB RID: 2491
		private IntPtr m_LocalUserId;

		// Token: 0x040009BC RID: 2492
		private ModEnumerationType m_Type;
	}
}
