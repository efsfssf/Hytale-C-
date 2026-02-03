using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000183 RID: 387
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchSetParameterOptionsInternal : ISettable<SessionSearchSetParameterOptions>, IDisposable
	{
		// Token: 0x17000293 RID: 659
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0000FFB7 File Offset: 0x0000E1B7
		public AttributeData? Parameter
		{
			set
			{
				Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref this.m_Parameter);
			}
		}

		// Token: 0x17000294 RID: 660
		// (set) Token: 0x06000B45 RID: 2885 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		public ComparisonOp ComparisonOp
		{
			set
			{
				this.m_ComparisonOp = value;
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0000FFD2 File Offset: 0x0000E1D2
		public void Set(ref SessionSearchSetParameterOptions other)
		{
			this.m_ApiVersion = 1;
			this.Parameter = other.Parameter;
			this.ComparisonOp = other.ComparisonOp;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		public void Set(ref SessionSearchSetParameterOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Parameter = other.Value.Parameter;
				this.ComparisonOp = other.Value.ComparisonOp;
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00010043 File Offset: 0x0000E243
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Parameter);
		}

		// Token: 0x04000521 RID: 1313
		private int m_ApiVersion;

		// Token: 0x04000522 RID: 1314
		private IntPtr m_Parameter;

		// Token: 0x04000523 RID: 1315
		private ComparisonOp m_ComparisonOp;
	}
}
