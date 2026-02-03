using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200020E RID: 526
	public struct AudioOutputDeviceInfo
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x000166BF File Offset: 0x000148BF
		// (set) Token: 0x06000F3F RID: 3903 RVA: 0x000166C7 File Offset: 0x000148C7
		public bool DefaultDevice { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x000166D0 File Offset: 0x000148D0
		// (set) Token: 0x06000F41 RID: 3905 RVA: 0x000166D8 File Offset: 0x000148D8
		public Utf8String DeviceId { get; set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x000166E1 File Offset: 0x000148E1
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x000166E9 File Offset: 0x000148E9
		public Utf8String DeviceName { get; set; }

		// Token: 0x06000F44 RID: 3908 RVA: 0x000166F2 File Offset: 0x000148F2
		internal void Set(ref AudioOutputDeviceInfoInternal other)
		{
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}
	}
}
