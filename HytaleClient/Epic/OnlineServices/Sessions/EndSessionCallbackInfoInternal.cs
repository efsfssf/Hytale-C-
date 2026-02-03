using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000105 RID: 261
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EndSessionCallbackInfoInternal : ICallbackInfoInternal, IGettable<EndSessionCallbackInfo>, ISettable<EndSessionCallbackInfo>, IDisposable
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0000C840 File Offset: 0x0000AA40
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x0000C858 File Offset: 0x0000AA58
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0000C864 File Offset: 0x0000AA64
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x0000C885 File Offset: 0x0000AA85
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0000C898 File Offset: 0x0000AA98
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		public void Set(ref EndSessionCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		public void Set(ref EndSessionCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0000C914 File Offset: 0x0000AB14
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0000C923 File Offset: 0x0000AB23
		public void Get(out EndSessionCallbackInfo output)
		{
			output = default(EndSessionCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000418 RID: 1048
		private Result m_ResultCode;

		// Token: 0x04000419 RID: 1049
		private IntPtr m_ClientData;
	}
}
