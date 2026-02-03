using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000637 RID: 1591
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeletePersistentAuthCallbackInfoInternal : ICallbackInfoInternal, IGettable<DeletePersistentAuthCallbackInfo>, ISettable<DeletePersistentAuthCallbackInfo>, IDisposable
	{
		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x0003CBF4 File Offset: 0x0003ADF4
		// (set) Token: 0x06002937 RID: 10551 RVA: 0x0003CC0C File Offset: 0x0003AE0C
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06002938 RID: 10552 RVA: 0x0003CC18 File Offset: 0x0003AE18
		// (set) Token: 0x06002939 RID: 10553 RVA: 0x0003CC39 File Offset: 0x0003AE39
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600293A RID: 10554 RVA: 0x0003CC4C File Offset: 0x0003AE4C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x0003CC64 File Offset: 0x0003AE64
		public void Set(ref DeletePersistentAuthCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0003CC84 File Offset: 0x0003AE84
		public void Set(ref DeletePersistentAuthCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x0003CCC8 File Offset: 0x0003AEC8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x0003CCD7 File Offset: 0x0003AED7
		public void Get(out DeletePersistentAuthCallbackInfo output)
		{
			output = default(DeletePersistentAuthCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040011BA RID: 4538
		private Result m_ResultCode;

		// Token: 0x040011BB RID: 4539
		private IntPtr m_ClientData;
	}
}
