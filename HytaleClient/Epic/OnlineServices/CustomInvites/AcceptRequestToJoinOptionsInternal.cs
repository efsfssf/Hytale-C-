using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000577 RID: 1399
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AcceptRequestToJoinOptionsInternal : ISettable<AcceptRequestToJoinOptions>, IDisposable
	{
		// Token: 0x17000AAB RID: 2731
		// (set) Token: 0x06002482 RID: 9346 RVA: 0x00035C49 File Offset: 0x00033E49
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (set) Token: 0x06002483 RID: 9347 RVA: 0x00035C59 File Offset: 0x00033E59
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x00035C69 File Offset: 0x00033E69
		public void Set(ref AcceptRequestToJoinOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x00035C90 File Offset: 0x00033E90
		public void Set(ref AcceptRequestToJoinOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x00035CDB File Offset: 0x00033EDB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04001001 RID: 4097
		private int m_ApiVersion;

		// Token: 0x04001002 RID: 4098
		private IntPtr m_LocalUserId;

		// Token: 0x04001003 RID: 4099
		private IntPtr m_TargetUserId;
	}
}
