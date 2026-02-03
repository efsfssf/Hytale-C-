using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000243 RID: 579
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnSetOutputDeviceSettingsCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnSetOutputDeviceSettingsCallbackInfo>, ISettable<OnSetOutputDeviceSettingsCallbackInfo>, IDisposable
	{
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x000175B4 File Offset: 0x000157B4
		// (set) Token: 0x06001040 RID: 4160 RVA: 0x000175CC File Offset: 0x000157CC
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

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x000175D8 File Offset: 0x000157D8
		// (set) Token: 0x06001042 RID: 4162 RVA: 0x000175F9 File Offset: 0x000157F9
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

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0001760C File Offset: 0x0001580C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00017624 File Offset: 0x00015824
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x00017645 File Offset: 0x00015845
		public Utf8String RealDeviceId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RealDeviceId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RealDeviceId);
			}
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00017655 File Offset: 0x00015855
		public void Set(ref OnSetOutputDeviceSettingsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RealDeviceId = other.RealDeviceId;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00017680 File Offset: 0x00015880
		public void Set(ref OnSetOutputDeviceSettingsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.RealDeviceId = other.Value.RealDeviceId;
			}
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x000176D9 File Offset: 0x000158D9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_RealDeviceId);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000176F4 File Offset: 0x000158F4
		public void Get(out OnSetOutputDeviceSettingsCallbackInfo output)
		{
			output = default(OnSetOutputDeviceSettingsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400071B RID: 1819
		private Result m_ResultCode;

		// Token: 0x0400071C RID: 1820
		private IntPtr m_ClientData;

		// Token: 0x0400071D RID: 1821
		private IntPtr m_RealDeviceId;
	}
}
