using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005D8 RID: 1496
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<CreateUserCallbackInfo>, ISettable<CreateUserCallbackInfo>, IDisposable
	{
		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x060026DD RID: 9949 RVA: 0x0003981C File Offset: 0x00037A1C
		// (set) Token: 0x060026DE RID: 9950 RVA: 0x00039834 File Offset: 0x00037A34
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

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x060026DF RID: 9951 RVA: 0x00039840 File Offset: 0x00037A40
		// (set) Token: 0x060026E0 RID: 9952 RVA: 0x00039861 File Offset: 0x00037A61
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

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x00039874 File Offset: 0x00037A74
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060026E2 RID: 9954 RVA: 0x0003988C File Offset: 0x00037A8C
		// (set) Token: 0x060026E3 RID: 9955 RVA: 0x000398AD File Offset: 0x00037AAD
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

		// Token: 0x060026E4 RID: 9956 RVA: 0x000398BD File Offset: 0x00037ABD
		public void Set(ref CreateUserCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000398E8 File Offset: 0x00037AE8
		public void Set(ref CreateUserCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x00039941 File Offset: 0x00037B41
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x0003995C File Offset: 0x00037B5C
		public void Get(out CreateUserCallbackInfo output)
		{
			output = default(CreateUserCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040010DE RID: 4318
		private Result m_ResultCode;

		// Token: 0x040010DF RID: 4319
		private IntPtr m_ClientData;

		// Token: 0x040010E0 RID: 4320
		private IntPtr m_LocalUserId;
	}
}
