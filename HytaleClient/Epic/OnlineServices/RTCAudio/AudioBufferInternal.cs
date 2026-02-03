using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000207 RID: 519
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioBufferInternal : IGettable<AudioBuffer>, ISettable<AudioBuffer>, IDisposable
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x00016090 File Offset: 0x00014290
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x000160B7 File Offset: 0x000142B7
		public short[] Frames
		{
			get
			{
				short[] result;
				Helper.Get<short>(this.m_Frames, out result, this.m_FramesCount);
				return result;
			}
			set
			{
				Helper.Set<short>(value, ref this.m_Frames, out this.m_FramesCount);
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x000160D0 File Offset: 0x000142D0
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x000160E8 File Offset: 0x000142E8
		public uint SampleRate
		{
			get
			{
				return this.m_SampleRate;
			}
			set
			{
				this.m_SampleRate = value;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x000160F4 File Offset: 0x000142F4
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0001610C File Offset: 0x0001430C
		public uint Channels
		{
			get
			{
				return this.m_Channels;
			}
			set
			{
				this.m_Channels = value;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00016116 File Offset: 0x00014316
		public void Set(ref AudioBuffer other)
		{
			this.m_ApiVersion = 1;
			this.Frames = other.Frames;
			this.SampleRate = other.SampleRate;
			this.Channels = other.Channels;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00016148 File Offset: 0x00014348
		public void Set(ref AudioBuffer? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Frames = other.Value.Frames;
				this.SampleRate = other.Value.SampleRate;
				this.Channels = other.Value.Channels;
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x000161A8 File Offset: 0x000143A8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Frames);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x000161B7 File Offset: 0x000143B7
		public void Get(out AudioBuffer output)
		{
			output = default(AudioBuffer);
			output.Set(ref this);
		}

		// Token: 0x040006C8 RID: 1736
		private int m_ApiVersion;

		// Token: 0x040006C9 RID: 1737
		private IntPtr m_Frames;

		// Token: 0x040006CA RID: 1738
		private uint m_FramesCount;

		// Token: 0x040006CB RID: 1739
		private uint m_SampleRate;

		// Token: 0x040006CC RID: 1740
		private uint m_Channels;
	}
}
