using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000640 RID: 1600
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LinkAccountOptionsInternal : ISettable<LinkAccountOptions>, IDisposable
	{
		// Token: 0x17000C16 RID: 3094
		// (set) Token: 0x06002973 RID: 10611 RVA: 0x0003D1CB File Offset: 0x0003B3CB
		public LinkAccountFlags LinkAccountFlags
		{
			set
			{
				this.m_LinkAccountFlags = value;
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (set) Token: 0x06002974 RID: 10612 RVA: 0x0003D1D5 File Offset: 0x0003B3D5
		public ContinuanceToken ContinuanceToken
		{
			set
			{
				Helper.Set(value, ref this.m_ContinuanceToken);
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (set) Token: 0x06002975 RID: 10613 RVA: 0x0003D1E5 File Offset: 0x0003B3E5
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0003D1F5 File Offset: 0x0003B3F5
		public void Set(ref LinkAccountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LinkAccountFlags = other.LinkAccountFlags;
			this.ContinuanceToken = other.ContinuanceToken;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x0003D228 File Offset: 0x0003B428
		public void Set(ref LinkAccountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LinkAccountFlags = other.Value.LinkAccountFlags;
				this.ContinuanceToken = other.Value.ContinuanceToken;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x0003D288 File Offset: 0x0003B488
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ContinuanceToken);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040011D4 RID: 4564
		private int m_ApiVersion;

		// Token: 0x040011D5 RID: 4565
		private LinkAccountFlags m_LinkAccountFlags;

		// Token: 0x040011D6 RID: 4566
		private IntPtr m_ContinuanceToken;

		// Token: 0x040011D7 RID: 4567
		private IntPtr m_LocalUserId;
	}
}
