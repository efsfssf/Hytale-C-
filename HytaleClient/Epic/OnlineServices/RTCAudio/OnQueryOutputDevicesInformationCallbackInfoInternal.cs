using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000237 RID: 567
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryOutputDevicesInformationCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryOutputDevicesInformationCallbackInfo>, ISettable<OnQueryOutputDevicesInformationCallbackInfo>, IDisposable
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x000170A4 File Offset: 0x000152A4
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x000170BC File Offset: 0x000152BC
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

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x000170C8 File Offset: 0x000152C8
		// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x000170E9 File Offset: 0x000152E9
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

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x000170FC File Offset: 0x000152FC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00017114 File Offset: 0x00015314
		public void Set(ref OnQueryOutputDevicesInformationCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00017134 File Offset: 0x00015334
		public void Set(ref OnQueryOutputDevicesInformationCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00017178 File Offset: 0x00015378
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00017187 File Offset: 0x00015387
		public void Get(out OnQueryOutputDevicesInformationCallbackInfo output)
		{
			output = default(OnQueryOutputDevicesInformationCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400070A RID: 1802
		private Result m_ResultCode;

		// Token: 0x0400070B RID: 1803
		private IntPtr m_ClientData;
	}
}
