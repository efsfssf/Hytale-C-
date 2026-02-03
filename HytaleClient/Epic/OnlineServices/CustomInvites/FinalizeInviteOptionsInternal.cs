using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200058C RID: 1420
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct FinalizeInviteOptionsInternal : ISettable<FinalizeInviteOptions>, IDisposable
	{
		// Token: 0x17000ABC RID: 2748
		// (set) Token: 0x060024E6 RID: 9446 RVA: 0x000369B3 File Offset: 0x00034BB3
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (set) Token: 0x060024E7 RID: 9447 RVA: 0x000369C3 File Offset: 0x00034BC3
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (set) Token: 0x060024E8 RID: 9448 RVA: 0x000369D3 File Offset: 0x00034BD3
		public Utf8String CustomInviteId
		{
			set
			{
				Helper.Set(value, ref this.m_CustomInviteId);
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (set) Token: 0x060024E9 RID: 9449 RVA: 0x000369E3 File Offset: 0x00034BE3
		public Result ProcessingResult
		{
			set
			{
				this.m_ProcessingResult = value;
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000369ED File Offset: 0x00034BED
		public void Set(ref FinalizeInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
			this.CustomInviteId = other.CustomInviteId;
			this.ProcessingResult = other.ProcessingResult;
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x00036A2C File Offset: 0x00034C2C
		public void Set(ref FinalizeInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
				this.CustomInviteId = other.Value.CustomInviteId;
				this.ProcessingResult = other.Value.ProcessingResult;
			}
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00036AA1 File Offset: 0x00034CA1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_CustomInviteId);
		}

		// Token: 0x04001029 RID: 4137
		private int m_ApiVersion;

		// Token: 0x0400102A RID: 4138
		private IntPtr m_TargetUserId;

		// Token: 0x0400102B RID: 4139
		private IntPtr m_LocalUserId;

		// Token: 0x0400102C RID: 4140
		private IntPtr m_CustomInviteId;

		// Token: 0x0400102D RID: 4141
		private Result m_ProcessingResult;
	}
}
