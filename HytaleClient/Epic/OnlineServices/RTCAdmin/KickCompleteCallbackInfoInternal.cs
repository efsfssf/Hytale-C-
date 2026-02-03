using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000289 RID: 649
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct KickCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<KickCompleteCallbackInfo>, ISettable<KickCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x0001A974 File Offset: 0x00018B74
		// (set) Token: 0x06001238 RID: 4664 RVA: 0x0001A98C File Offset: 0x00018B8C
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

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x0001A998 File Offset: 0x00018B98
		// (set) Token: 0x0600123A RID: 4666 RVA: 0x0001A9B9 File Offset: 0x00018BB9
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

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x0001A9CC File Offset: 0x00018BCC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0001A9E4 File Offset: 0x00018BE4
		public void Set(ref KickCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0001AA04 File Offset: 0x00018C04
		public void Set(ref KickCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0001AA48 File Offset: 0x00018C48
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0001AA57 File Offset: 0x00018C57
		public void Get(out KickCompleteCallbackInfo output)
		{
			output = default(KickCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000804 RID: 2052
		private Result m_ResultCode;

		// Token: 0x04000805 RID: 2053
		private IntPtr m_ClientData;
	}
}
