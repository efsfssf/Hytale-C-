using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200066A RID: 1642
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct VerifyUserAuthCallbackInfoInternal : ICallbackInfoInternal, IGettable<VerifyUserAuthCallbackInfo>, ISettable<VerifyUserAuthCallbackInfo>, IDisposable
	{
		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x0003EE28 File Offset: 0x0003D028
		// (set) Token: 0x06002AC2 RID: 10946 RVA: 0x0003EE40 File Offset: 0x0003D040
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

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x0003EE4C File Offset: 0x0003D04C
		// (set) Token: 0x06002AC4 RID: 10948 RVA: 0x0003EE6D File Offset: 0x0003D06D
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

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x0003EE80 File Offset: 0x0003D080
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x0003EE98 File Offset: 0x0003D098
		public void Set(ref VerifyUserAuthCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0003EEB8 File Offset: 0x0003D0B8
		public void Set(ref VerifyUserAuthCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0003EEFC File Offset: 0x0003D0FC
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x0003EF0B File Offset: 0x0003D10B
		public void Get(out VerifyUserAuthCallbackInfo output)
		{
			output = default(VerifyUserAuthCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001254 RID: 4692
		private Result m_ResultCode;

		// Token: 0x04001255 RID: 4693
		private IntPtr m_ClientData;
	}
}
