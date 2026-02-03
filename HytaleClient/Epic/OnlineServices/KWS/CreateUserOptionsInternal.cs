using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200048A RID: 1162
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateUserOptionsInternal : ISettable<CreateUserOptions>, IDisposable
	{
		// Token: 0x1700089C RID: 2204
		// (set) Token: 0x06001E65 RID: 7781 RVA: 0x0002C81B File Offset: 0x0002AA1B
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700089D RID: 2205
		// (set) Token: 0x06001E66 RID: 7782 RVA: 0x0002C82B File Offset: 0x0002AA2B
		public Utf8String DateOfBirth
		{
			set
			{
				Helper.Set(value, ref this.m_DateOfBirth);
			}
		}

		// Token: 0x1700089E RID: 2206
		// (set) Token: 0x06001E67 RID: 7783 RVA: 0x0002C83B File Offset: 0x0002AA3B
		public Utf8String ParentEmail
		{
			set
			{
				Helper.Set(value, ref this.m_ParentEmail);
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0002C84B File Offset: 0x0002AA4B
		public void Set(ref CreateUserOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.DateOfBirth = other.DateOfBirth;
			this.ParentEmail = other.ParentEmail;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0002C87C File Offset: 0x0002AA7C
		public void Set(ref CreateUserOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.DateOfBirth = other.Value.DateOfBirth;
				this.ParentEmail = other.Value.ParentEmail;
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0002C8DC File Offset: 0x0002AADC
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_DateOfBirth);
			Helper.Dispose(ref this.m_ParentEmail);
		}

		// Token: 0x04000D42 RID: 3394
		private int m_ApiVersion;

		// Token: 0x04000D43 RID: 3395
		private IntPtr m_LocalUserId;

		// Token: 0x04000D44 RID: 3396
		private IntPtr m_DateOfBirth;

		// Token: 0x04000D45 RID: 3397
		private IntPtr m_ParentEmail;
	}
}
