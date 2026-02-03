using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000FF RID: 255
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DestroySessionCallbackInfoInternal : ICallbackInfoInternal, IGettable<DestroySessionCallbackInfo>, ISettable<DestroySessionCallbackInfo>, IDisposable
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0000C5EC File Offset: 0x0000A7EC
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x0000C604 File Offset: 0x0000A804
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0000C610 File Offset: 0x0000A810
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x0000C631 File Offset: 0x0000A831
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

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0000C644 File Offset: 0x0000A844
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0000C65C File Offset: 0x0000A85C
		public void Set(ref DestroySessionCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0000C67C File Offset: 0x0000A87C
		public void Set(ref DestroySessionCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0000C6CF File Offset: 0x0000A8CF
		public void Get(out DestroySessionCallbackInfo output)
		{
			output = default(DestroySessionCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400040E RID: 1038
		private Result m_ResultCode;

		// Token: 0x0400040F RID: 1039
		private IntPtr m_ClientData;
	}
}
