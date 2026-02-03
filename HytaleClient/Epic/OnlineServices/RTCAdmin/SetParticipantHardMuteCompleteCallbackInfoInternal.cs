using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000298 RID: 664
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetParticipantHardMuteCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<SetParticipantHardMuteCompleteCallbackInfo>, ISettable<SetParticipantHardMuteCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x0001B29C File Offset: 0x0001949C
		// (set) Token: 0x060012A0 RID: 4768 RVA: 0x0001B2B4 File Offset: 0x000194B4
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

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x0001B2C0 File Offset: 0x000194C0
		// (set) Token: 0x060012A2 RID: 4770 RVA: 0x0001B2E1 File Offset: 0x000194E1
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

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x0001B2F4 File Offset: 0x000194F4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0001B30C File Offset: 0x0001950C
		public void Set(ref SetParticipantHardMuteCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0001B32C File Offset: 0x0001952C
		public void Set(ref SetParticipantHardMuteCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0001B370 File Offset: 0x00019570
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0001B37F File Offset: 0x0001957F
		public void Get(out SetParticipantHardMuteCompleteCallbackInfo output)
		{
			output = default(SetParticipantHardMuteCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000829 RID: 2089
		private Result m_ResultCode;

		// Token: 0x0400082A RID: 2090
		private IntPtr m_ClientData;
	}
}
