using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000402 RID: 1026
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchSetParameterOptionsInternal : ISettable<LobbySearchSetParameterOptions>, IDisposable
	{
		// Token: 0x170007D3 RID: 2003
		// (set) Token: 0x06001B75 RID: 7029 RVA: 0x0002932B File Offset: 0x0002752B
		public AttributeData? Parameter
		{
			set
			{
				Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref this.m_Parameter);
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (set) Token: 0x06001B76 RID: 7030 RVA: 0x0002933C File Offset: 0x0002753C
		public ComparisonOp ComparisonOp
		{
			set
			{
				this.m_ComparisonOp = value;
			}
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x00029346 File Offset: 0x00027546
		public void Set(ref LobbySearchSetParameterOptions other)
		{
			this.m_ApiVersion = 1;
			this.Parameter = other.Parameter;
			this.ComparisonOp = other.ComparisonOp;
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0002936C File Offset: 0x0002756C
		public void Set(ref LobbySearchSetParameterOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Parameter = other.Value.Parameter;
				this.ComparisonOp = other.Value.ComparisonOp;
			}
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000293B7 File Offset: 0x000275B7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Parameter);
		}

		// Token: 0x04000C51 RID: 3153
		private int m_ApiVersion;

		// Token: 0x04000C52 RID: 3154
		private IntPtr m_Parameter;

		// Token: 0x04000C53 RID: 3155
		private ComparisonOp m_ComparisonOp;
	}
}
