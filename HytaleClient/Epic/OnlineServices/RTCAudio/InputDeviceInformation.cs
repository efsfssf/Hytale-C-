using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000222 RID: 546
	public struct InputDeviceInformation
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00016D38 File Offset: 0x00014F38
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x00016D40 File Offset: 0x00014F40
		public bool DefaultDevice { get; set; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00016D49 File Offset: 0x00014F49
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x00016D51 File Offset: 0x00014F51
		public Utf8String DeviceId { get; set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x00016D5A File Offset: 0x00014F5A
		// (set) Token: 0x06000F8F RID: 3983 RVA: 0x00016D62 File Offset: 0x00014F62
		public Utf8String DeviceName { get; set; }

		// Token: 0x06000F90 RID: 3984 RVA: 0x00016D6B File Offset: 0x00014F6B
		internal void Set(ref InputDeviceInformationInternal other)
		{
			this.DefaultDevice = other.DefaultDevice;
			this.DeviceId = other.DeviceId;
			this.DeviceName = other.DeviceName;
		}
	}
}
