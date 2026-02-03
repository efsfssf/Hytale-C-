using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x0200019E RID: 414
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreatePlayerSanctionAppealOptionsInternal : ISettable<CreatePlayerSanctionAppealOptions>, IDisposable
	{
		// Token: 0x170002C6 RID: 710
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x0001187D File Offset: 0x0000FA7D
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170002C7 RID: 711
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x0001188D File Offset: 0x0000FA8D
		public SanctionAppealReason Reason
		{
			set
			{
				this.m_Reason = value;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x00011897 File Offset: 0x0000FA97
		public Utf8String ReferenceId
		{
			set
			{
				Helper.Set(value, ref this.m_ReferenceId);
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000118A7 File Offset: 0x0000FAA7
		public void Set(ref CreatePlayerSanctionAppealOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Reason = other.Reason;
			this.ReferenceId = other.ReferenceId;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x000118D8 File Offset: 0x0000FAD8
		public void Set(ref CreatePlayerSanctionAppealOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Reason = other.Value.Reason;
				this.ReferenceId = other.Value.ReferenceId;
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00011938 File Offset: 0x0000FB38
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ReferenceId);
		}

		// Token: 0x0400057F RID: 1407
		private int m_ApiVersion;

		// Token: 0x04000580 RID: 1408
		private IntPtr m_LocalUserId;

		// Token: 0x04000581 RID: 1409
		private SanctionAppealReason m_Reason;

		// Token: 0x04000582 RID: 1410
		private IntPtr m_ReferenceId;
	}
}
