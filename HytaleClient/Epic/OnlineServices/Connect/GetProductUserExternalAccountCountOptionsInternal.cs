using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E6 RID: 1510
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetProductUserExternalAccountCountOptionsInternal : ISettable<GetProductUserExternalAccountCountOptions>, IDisposable
	{
		// Token: 0x17000B6B RID: 2923
		// (set) Token: 0x06002734 RID: 10036 RVA: 0x0003A06C File Offset: 0x0003826C
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x0003A07C File Offset: 0x0003827C
		public void Set(ref GetProductUserExternalAccountCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x0003A094 File Offset: 0x00038294
		public void Set(ref GetProductUserExternalAccountCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x0003A0CA File Offset: 0x000382CA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04001101 RID: 4353
		private int m_ApiVersion;

		// Token: 0x04001102 RID: 4354
		private IntPtr m_TargetUserId;
	}
}
