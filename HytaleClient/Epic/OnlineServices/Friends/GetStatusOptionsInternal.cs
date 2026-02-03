using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004E2 RID: 1250
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetStatusOptionsInternal : ISettable<GetStatusOptions>, IDisposable
	{
		// Token: 0x17000939 RID: 2361
		// (set) Token: 0x06002071 RID: 8305 RVA: 0x0002FAEF File Offset: 0x0002DCEF
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700093A RID: 2362
		// (set) Token: 0x06002072 RID: 8306 RVA: 0x0002FAFF File Offset: 0x0002DCFF
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x0002FB0F File Offset: 0x0002DD0F
		public void Set(ref GetStatusOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x0002FB34 File Offset: 0x0002DD34
		public void Set(ref GetStatusOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0002FB7F File Offset: 0x0002DD7F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000E26 RID: 3622
		private int m_ApiVersion;

		// Token: 0x04000E27 RID: 3623
		private IntPtr m_LocalUserId;

		// Token: 0x04000E28 RID: 3624
		private IntPtr m_TargetUserId;
	}
}
