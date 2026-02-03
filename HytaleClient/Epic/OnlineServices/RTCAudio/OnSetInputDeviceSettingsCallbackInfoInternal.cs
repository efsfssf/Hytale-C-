using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200023F RID: 575
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnSetInputDeviceSettingsCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnSetInputDeviceSettingsCallbackInfo>, ISettable<OnSetInputDeviceSettingsCallbackInfo>, IDisposable
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x000173E4 File Offset: 0x000155E4
		// (set) Token: 0x06001025 RID: 4133 RVA: 0x000173FC File Offset: 0x000155FC
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

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x00017408 File Offset: 0x00015608
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x00017429 File Offset: 0x00015629
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

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x0001743C File Offset: 0x0001563C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x00017454 File Offset: 0x00015654
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x00017475 File Offset: 0x00015675
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

		// Token: 0x0600102B RID: 4139 RVA: 0x00017485 File Offset: 0x00015685
		public void Set(ref OnSetInputDeviceSettingsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RealDeviceId = other.RealDeviceId;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000174B0 File Offset: 0x000156B0
		public void Set(ref OnSetInputDeviceSettingsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.RealDeviceId = other.Value.RealDeviceId;
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00017509 File Offset: 0x00015709
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_RealDeviceId);
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00017524 File Offset: 0x00015724
		public void Get(out OnSetInputDeviceSettingsCallbackInfo output)
		{
			output = default(OnSetInputDeviceSettingsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000715 RID: 1813
		private Result m_ResultCode;

		// Token: 0x04000716 RID: 1814
		private IntPtr m_ClientData;

		// Token: 0x04000717 RID: 1815
		private IntPtr m_RealDeviceId;
	}
}
