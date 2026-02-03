using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000233 RID: 563
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryInputDevicesInformationCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryInputDevicesInformationCallbackInfo>, ISettable<OnQueryInputDevicesInformationCallbackInfo>, IDisposable
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00016F50 File Offset: 0x00015150
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x00016F68 File Offset: 0x00015168
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

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x00016F74 File Offset: 0x00015174
		// (set) Token: 0x06000FDC RID: 4060 RVA: 0x00016F95 File Offset: 0x00015195
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

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x00016FA8 File Offset: 0x000151A8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00016FC0 File Offset: 0x000151C0
		public void Set(ref OnQueryInputDevicesInformationCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00016FE0 File Offset: 0x000151E0
		public void Set(ref OnQueryInputDevicesInformationCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00017024 File Offset: 0x00015224
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00017033 File Offset: 0x00015233
		public void Get(out OnQueryInputDevicesInformationCallbackInfo output)
		{
			output = default(OnQueryInputDevicesInformationCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000706 RID: 1798
		private Result m_ResultCode;

		// Token: 0x04000707 RID: 1799
		private IntPtr m_ClientData;
	}
}
