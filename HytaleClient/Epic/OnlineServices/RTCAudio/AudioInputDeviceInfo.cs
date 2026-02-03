using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200020A RID: 522
	public struct AudioInputDeviceInfo
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x000162B4 File Offset: 0x000144B4
		// (set) Token: 0x06000F17 RID: 3863 RVA: 0x000162BC File Offset: 0x000144BC
		public bool DefaultDevice { get; set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x000162C5 File Offset: 0x000144C5
		// (set) Token: 0x06000F19 RID: 3865 RVA: 0x000162CD File Offset: 0x000144CD
		public Utf8String DeviceId { get; set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x000162D6 File Offset: 0x000144D6
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x000162DE File Offset: 0x000144DE
		public Utf8String DeviceName { get; set; }

		// Token: 0x06000F1C RID: 3868 RVA: 0x000162E7 File Offset: 0x000144E7
		internal void Set(ref AudioInputDeviceInfoInternal other)
		{
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}
	}
}
