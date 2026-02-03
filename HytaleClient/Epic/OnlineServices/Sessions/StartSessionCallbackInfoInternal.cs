using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200018A RID: 394
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct StartSessionCallbackInfoInternal : ICallbackInfoInternal, IGettable<StartSessionCallbackInfo>, ISettable<StartSessionCallbackInfo>, IDisposable
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00010E50 File Offset: 0x0000F050
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x00010E68 File Offset: 0x0000F068
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

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00010E74 File Offset: 0x0000F074
		// (set) Token: 0x06000B92 RID: 2962 RVA: 0x00010E95 File Offset: 0x0000F095
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

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00010EA8 File Offset: 0x0000F0A8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00010EC0 File Offset: 0x0000F0C0
		public void Set(ref StartSessionCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00010EE0 File Offset: 0x0000F0E0
		public void Set(ref StartSessionCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00010F24 File Offset: 0x0000F124
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00010F33 File Offset: 0x0000F133
		public void Get(out StartSessionCallbackInfo output)
		{
			output = default(StartSessionCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000551 RID: 1361
		private Result m_ResultCode;

		// Token: 0x04000552 RID: 1362
		private IntPtr m_ClientData;
	}
}
