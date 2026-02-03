using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005D4 RID: 1492
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateDeviceIdCallbackInfoInternal : ICallbackInfoInternal, IGettable<CreateDeviceIdCallbackInfo>, ISettable<CreateDeviceIdCallbackInfo>, IDisposable
	{
		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x0003962C File Offset: 0x0003782C
		// (set) Token: 0x060026C7 RID: 9927 RVA: 0x00039644 File Offset: 0x00037844
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

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x00039650 File Offset: 0x00037850
		// (set) Token: 0x060026C9 RID: 9929 RVA: 0x00039671 File Offset: 0x00037871
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

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x00039684 File Offset: 0x00037884
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0003969C File Offset: 0x0003789C
		public void Set(ref CreateDeviceIdCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000396BC File Offset: 0x000378BC
		public void Set(ref CreateDeviceIdCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x00039700 File Offset: 0x00037900
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x0003970F File Offset: 0x0003790F
		public void Get(out CreateDeviceIdCallbackInfo output)
		{
			output = default(CreateDeviceIdCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040010D6 RID: 4310
		private Result m_ResultCode;

		// Token: 0x040010D7 RID: 4311
		private IntPtr m_ClientData;
	}
}
