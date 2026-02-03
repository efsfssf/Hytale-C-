using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000408 RID: 1032
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LocalRTCOptionsInternal : IGettable<LocalRTCOptions>, ISettable<LocalRTCOptions>, IDisposable
	{
		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06001B98 RID: 7064 RVA: 0x00029628 File Offset: 0x00027828
		// (set) Token: 0x06001B99 RID: 7065 RVA: 0x00029640 File Offset: 0x00027840
		public uint Flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x0002964C File Offset: 0x0002784C
		// (set) Token: 0x06001B9B RID: 7067 RVA: 0x0002966D File Offset: 0x0002786D
		public bool UseManualAudioInput
		{
			get
			{
				bool result;
				Helper.Get(this.m_UseManualAudioInput, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UseManualAudioInput);
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x00029680 File Offset: 0x00027880
		// (set) Token: 0x06001B9D RID: 7069 RVA: 0x000296A1 File Offset: 0x000278A1
		public bool UseManualAudioOutput
		{
			get
			{
				bool result;
				Helper.Get(this.m_UseManualAudioOutput, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UseManualAudioOutput);
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06001B9E RID: 7070 RVA: 0x000296B4 File Offset: 0x000278B4
		// (set) Token: 0x06001B9F RID: 7071 RVA: 0x000296D5 File Offset: 0x000278D5
		public bool LocalAudioDeviceInputStartsMuted
		{
			get
			{
				bool result;
				Helper.Get(this.m_LocalAudioDeviceInputStartsMuted, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalAudioDeviceInputStartsMuted);
			}
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000296E5 File Offset: 0x000278E5
		public void Set(ref LocalRTCOptions other)
		{
			this.m_ApiVersion = 1;
			this.Flags = other.Flags;
			this.UseManualAudioInput = other.UseManualAudioInput;
			this.UseManualAudioOutput = other.UseManualAudioOutput;
			this.LocalAudioDeviceInputStartsMuted = other.LocalAudioDeviceInputStartsMuted;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00029724 File Offset: 0x00027924
		public void Set(ref LocalRTCOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Flags = other.Value.Flags;
				this.UseManualAudioInput = other.Value.UseManualAudioInput;
				this.UseManualAudioOutput = other.Value.UseManualAudioOutput;
				this.LocalAudioDeviceInputStartsMuted = other.Value.LocalAudioDeviceInputStartsMuted;
			}
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00029799 File Offset: 0x00027999
		public void Dispose()
		{
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0002979C File Offset: 0x0002799C
		public void Get(out LocalRTCOptions output)
		{
			output = default(LocalRTCOptions);
			output.Set(ref this);
		}

		// Token: 0x04000C5F RID: 3167
		private int m_ApiVersion;

		// Token: 0x04000C60 RID: 3168
		private uint m_Flags;

		// Token: 0x04000C61 RID: 3169
		private int m_UseManualAudioInput;

		// Token: 0x04000C62 RID: 3170
		private int m_UseManualAudioOutput;

		// Token: 0x04000C63 RID: 3171
		private int m_LocalAudioDeviceInputStartsMuted;
	}
}
