using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000206 RID: 518
	public struct AudioBuffer
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x00016033 File Offset: 0x00014233
		// (set) Token: 0x06000EFB RID: 3835 RVA: 0x0001603B File Offset: 0x0001423B
		public short[] Frames { get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x00016044 File Offset: 0x00014244
		// (set) Token: 0x06000EFD RID: 3837 RVA: 0x0001604C File Offset: 0x0001424C
		public uint SampleRate { get; set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x00016055 File Offset: 0x00014255
		// (set) Token: 0x06000EFF RID: 3839 RVA: 0x0001605D File Offset: 0x0001425D
		public uint Channels { get; set; }

		// Token: 0x06000F00 RID: 3840 RVA: 0x00016066 File Offset: 0x00014266
		internal void Set(ref AudioBufferInternal other)
		{
			this.Frames = other.Frames;
			this.SampleRate = other.SampleRate;
			this.Channels = other.Channels;
		}
	}
}
