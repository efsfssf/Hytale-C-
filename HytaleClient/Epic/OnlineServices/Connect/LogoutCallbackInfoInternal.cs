using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F6 RID: 1526
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogoutCallbackInfoInternal : ICallbackInfoInternal, IGettable<LogoutCallbackInfo>, ISettable<LogoutCallbackInfo>, IDisposable
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x060027AC RID: 10156 RVA: 0x0003AB90 File Offset: 0x00038D90
		// (set) Token: 0x060027AD RID: 10157 RVA: 0x0003ABA8 File Offset: 0x00038DA8
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

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x0003ABB4 File Offset: 0x00038DB4
		// (set) Token: 0x060027AF RID: 10159 RVA: 0x0003ABD5 File Offset: 0x00038DD5
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

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x0003ABE8 File Offset: 0x00038DE8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x060027B1 RID: 10161 RVA: 0x0003AC00 File Offset: 0x00038E00
		// (set) Token: 0x060027B2 RID: 10162 RVA: 0x0003AC21 File Offset: 0x00038E21
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x0003AC31 File Offset: 0x00038E31
		public void Set(ref LogoutCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x0003AC5C File Offset: 0x00038E5C
		public void Set(ref LogoutCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x0003ACB5 File Offset: 0x00038EB5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x0003ACD0 File Offset: 0x00038ED0
		public void Get(out LogoutCallbackInfo output)
		{
			output = default(LogoutCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001132 RID: 4402
		private Result m_ResultCode;

		// Token: 0x04001133 RID: 4403
		private IntPtr m_ClientData;

		// Token: 0x04001134 RID: 4404
		private IntPtr m_LocalUserId;
	}
}
