using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000111 RID: 273
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinSessionCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinSessionCallbackInfo>, ISettable<JoinSessionCallbackInfo>, IDisposable
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0000CDEC File Offset: 0x0000AFEC
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0000CE04 File Offset: 0x0000B004
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

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0000CE10 File Offset: 0x0000B010
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0000CE31 File Offset: 0x0000B031
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

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0000CE44 File Offset: 0x0000B044
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public void Set(ref JoinSessionCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000CE7C File Offset: 0x0000B07C
		public void Set(ref JoinSessionCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0000CEC0 File Offset: 0x0000B0C0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0000CECF File Offset: 0x0000B0CF
		public void Get(out JoinSessionCallbackInfo output)
		{
			output = default(JoinSessionCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000432 RID: 1074
		private Result m_ResultCode;

		// Token: 0x04000433 RID: 1075
		private IntPtr m_ClientData;
	}
}
