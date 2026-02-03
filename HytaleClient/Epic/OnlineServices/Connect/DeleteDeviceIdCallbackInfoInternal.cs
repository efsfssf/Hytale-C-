using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005DE RID: 1502
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteDeviceIdCallbackInfoInternal : ICallbackInfoInternal, IGettable<DeleteDeviceIdCallbackInfo>, ISettable<DeleteDeviceIdCallbackInfo>, IDisposable
	{
		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x00039B74 File Offset: 0x00037D74
		// (set) Token: 0x06002702 RID: 9986 RVA: 0x00039B8C File Offset: 0x00037D8C
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

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x00039B98 File Offset: 0x00037D98
		// (set) Token: 0x06002704 RID: 9988 RVA: 0x00039BB9 File Offset: 0x00037DB9
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

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06002705 RID: 9989 RVA: 0x00039BCC File Offset: 0x00037DCC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x00039BE4 File Offset: 0x00037DE4
		public void Set(ref DeleteDeviceIdCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x00039C04 File Offset: 0x00037E04
		public void Set(ref DeleteDeviceIdCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x00039C48 File Offset: 0x00037E48
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x00039C57 File Offset: 0x00037E57
		public void Get(out DeleteDeviceIdCallbackInfo output)
		{
			output = default(DeleteDeviceIdCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040010EB RID: 4331
		private Result m_ResultCode;

		// Token: 0x040010EC RID: 4332
		private IntPtr m_ClientData;
	}
}
