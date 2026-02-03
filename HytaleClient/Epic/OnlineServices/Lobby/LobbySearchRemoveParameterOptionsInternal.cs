using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003FC RID: 1020
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchRemoveParameterOptionsInternal : ISettable<LobbySearchRemoveParameterOptions>, IDisposable
	{
		// Token: 0x170007CB RID: 1995
		// (set) Token: 0x06001B60 RID: 7008 RVA: 0x00029186 File Offset: 0x00027386
		public Utf8String Key
		{
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x170007CC RID: 1996
		// (set) Token: 0x06001B61 RID: 7009 RVA: 0x00029196 File Offset: 0x00027396
		public ComparisonOp ComparisonOp
		{
			set
			{
				this.m_ComparisonOp = value;
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000291A0 File Offset: 0x000273A0
		public void Set(ref LobbySearchRemoveParameterOptions other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
			this.ComparisonOp = other.ComparisonOp;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000291C4 File Offset: 0x000273C4
		public void Set(ref LobbySearchRemoveParameterOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
				this.ComparisonOp = other.Value.ComparisonOp;
			}
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x0002920F File Offset: 0x0002740F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
		}

		// Token: 0x04000C46 RID: 3142
		private int m_ApiVersion;

		// Token: 0x04000C47 RID: 3143
		private IntPtr m_Key;

		// Token: 0x04000C48 RID: 3144
		private ComparisonOp m_ComparisonOp;
	}
}
