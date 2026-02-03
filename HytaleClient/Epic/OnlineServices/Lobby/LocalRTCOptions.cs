using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000407 RID: 1031
	public struct LocalRTCOptions
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x000295AD File Offset: 0x000277AD
		// (set) Token: 0x06001B90 RID: 7056 RVA: 0x000295B5 File Offset: 0x000277B5
		public uint Flags { get; set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x000295BE File Offset: 0x000277BE
		// (set) Token: 0x06001B92 RID: 7058 RVA: 0x000295C6 File Offset: 0x000277C6
		public bool UseManualAudioInput { get; set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06001B93 RID: 7059 RVA: 0x000295CF File Offset: 0x000277CF
		// (set) Token: 0x06001B94 RID: 7060 RVA: 0x000295D7 File Offset: 0x000277D7
		public bool UseManualAudioOutput { get; set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06001B95 RID: 7061 RVA: 0x000295E0 File Offset: 0x000277E0
		// (set) Token: 0x06001B96 RID: 7062 RVA: 0x000295E8 File Offset: 0x000277E8
		public bool LocalAudioDeviceInputStartsMuted { get; set; }

		// Token: 0x06001B97 RID: 7063 RVA: 0x000295F1 File Offset: 0x000277F1
		internal void Set(ref LocalRTCOptionsInternal other)
		{
			this.Flags = other.Flags;
			this.UseManualAudioInput = other.UseManualAudioInput;
			this.UseManualAudioOutput = other.UseManualAudioOutput;
			this.LocalAudioDeviceInputStartsMuted = other.LocalAudioDeviceInputStartsMuted;
		}
	}
}
