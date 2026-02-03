using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000252 RID: 594
	public struct OutputDeviceInformation
	{
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x000178D6 File Offset: 0x00015AD6
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x000178DE File Offset: 0x00015ADE
		public bool DefaultDevice { get; set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x000178E7 File Offset: 0x00015AE7
		// (set) Token: 0x06001090 RID: 4240 RVA: 0x000178EF File Offset: 0x00015AEF
		public Utf8String DeviceId { get; set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x000178F8 File Offset: 0x00015AF8
		// (set) Token: 0x06001092 RID: 4242 RVA: 0x00017900 File Offset: 0x00015B00
		public Utf8String DeviceName { get; set; }

		// Token: 0x06001093 RID: 4243 RVA: 0x00017909 File Offset: 0x00015B09
		internal void Set(ref OutputDeviceInformationInternal other)
		{
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}
	}
}
