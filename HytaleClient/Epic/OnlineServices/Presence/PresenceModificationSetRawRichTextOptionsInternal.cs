using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002DD RID: 733
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PresenceModificationSetRawRichTextOptionsInternal : ISettable<PresenceModificationSetRawRichTextOptions>, IDisposable
	{
		// Token: 0x17000562 RID: 1378
		// (set) Token: 0x0600142A RID: 5162 RVA: 0x0001D782 File Offset: 0x0001B982
		public Utf8String RichText
		{
			set
			{
				Helper.Set(value, ref this.m_RichText);
			}
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0001D792 File Offset: 0x0001B992
		public void Set(ref PresenceModificationSetRawRichTextOptions other)
		{
			this.m_ApiVersion = 1;
			this.RichText = other.RichText;
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0001D7AC File Offset: 0x0001B9AC
		public void Set(ref PresenceModificationSetRawRichTextOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.RichText = other.Value.RichText;
			}
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0001D7E2 File Offset: 0x0001B9E2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_RichText);
		}

		// Token: 0x040008DD RID: 2269
		private int m_ApiVersion;

		// Token: 0x040008DE RID: 2270
		private IntPtr m_RichText;
	}
}
