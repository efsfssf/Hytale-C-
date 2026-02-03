using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200048C RID: 1164
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetPermissionByKeyOptionsInternal : ISettable<GetPermissionByKeyOptions>, IDisposable
	{
		// Token: 0x170008A1 RID: 2209
		// (set) Token: 0x06001E6F RID: 7791 RVA: 0x0002C925 File Offset: 0x0002AB25
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (set) Token: 0x06001E70 RID: 7792 RVA: 0x0002C935 File Offset: 0x0002AB35
		public Utf8String Key
		{
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0002C945 File Offset: 0x0002AB45
		public void Set(ref GetPermissionByKeyOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Key = other.Key;
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0002C96C File Offset: 0x0002AB6C
		public void Set(ref GetPermissionByKeyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Key = other.Value.Key;
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0002C9B7 File Offset: 0x0002ABB7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Key);
		}

		// Token: 0x04000D48 RID: 3400
		private int m_ApiVersion;

		// Token: 0x04000D49 RID: 3401
		private IntPtr m_LocalUserId;

		// Token: 0x04000D4A RID: 3402
		private IntPtr m_Key;
	}
}
