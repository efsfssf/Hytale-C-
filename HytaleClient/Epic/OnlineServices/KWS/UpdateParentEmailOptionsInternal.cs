using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004B0 RID: 1200
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateParentEmailOptionsInternal : ISettable<UpdateParentEmailOptions>, IDisposable
	{
		// Token: 0x170008E4 RID: 2276
		// (set) Token: 0x06001F5E RID: 8030 RVA: 0x0002DE84 File Offset: 0x0002C084
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (set) Token: 0x06001F5F RID: 8031 RVA: 0x0002DE94 File Offset: 0x0002C094
		public Utf8String ParentEmail
		{
			set
			{
				Helper.Set(value, ref this.m_ParentEmail);
			}
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0002DEA4 File Offset: 0x0002C0A4
		public void Set(ref UpdateParentEmailOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ParentEmail = other.ParentEmail;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0002DEC8 File Offset: 0x0002C0C8
		public void Set(ref UpdateParentEmailOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ParentEmail = other.Value.ParentEmail;
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0002DF13 File Offset: 0x0002C113
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ParentEmail);
		}

		// Token: 0x04000D9D RID: 3485
		private int m_ApiVersion;

		// Token: 0x04000D9E RID: 3486
		private IntPtr m_LocalUserId;

		// Token: 0x04000D9F RID: 3487
		private IntPtr m_ParentEmail;
	}
}
