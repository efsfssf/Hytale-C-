using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001CF RID: 463
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ParticipantMetadataInternal : IGettable<ParticipantMetadata>, ISettable<ParticipantMetadata>, IDisposable
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00013444 File Offset: 0x00011644
		// (set) Token: 0x06000D51 RID: 3409 RVA: 0x00013465 File Offset: 0x00011665
		public Utf8String Key
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Key, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00013478 File Offset: 0x00011678
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x00013499 File Offset: 0x00011699
		public Utf8String Value
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Value, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Value);
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000134A9 File Offset: 0x000116A9
		public void Set(ref ParticipantMetadata other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
			this.Value = other.Value;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000134D0 File Offset: 0x000116D0
		public void Set(ref ParticipantMetadata? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
				this.Value = other.Value.Value;
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0001351B File Offset: 0x0001171B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
			Helper.Dispose(ref this.m_Value);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00013536 File Offset: 0x00011736
		public void Get(out ParticipantMetadata output)
		{
			output = default(ParticipantMetadata);
			output.Set(ref this);
		}

		// Token: 0x04000607 RID: 1543
		private int m_ApiVersion;

		// Token: 0x04000608 RID: 1544
		private IntPtr m_Key;

		// Token: 0x04000609 RID: 1545
		private IntPtr m_Value;
	}
}
