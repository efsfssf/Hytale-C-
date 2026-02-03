using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000187 RID: 391
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchSetTargetUserIdOptionsInternal : ISettable<SessionSearchSetTargetUserIdOptions>, IDisposable
	{
		// Token: 0x17000298 RID: 664
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x000100E2 File Offset: 0x0000E2E2
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x000100F2 File Offset: 0x0000E2F2
		public void Set(ref SessionSearchSetTargetUserIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0001010C File Offset: 0x0000E30C
		public void Set(ref SessionSearchSetTargetUserIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00010142 File Offset: 0x0000E342
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000528 RID: 1320
		private int m_ApiVersion;

		// Token: 0x04000529 RID: 1321
		private IntPtr m_TargetUserId;
	}
}
