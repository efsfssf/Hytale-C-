using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002FA RID: 762
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DuplicateFileOptionsInternal : ISettable<DuplicateFileOptions>, IDisposable
	{
		// Token: 0x170005A3 RID: 1443
		// (set) Token: 0x060014D8 RID: 5336 RVA: 0x0001E689 File Offset: 0x0001C889
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (set) Token: 0x060014D9 RID: 5337 RVA: 0x0001E699 File Offset: 0x0001C899
		public Utf8String SourceFilename
		{
			set
			{
				Helper.Set(value, ref this.m_SourceFilename);
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (set) Token: 0x060014DA RID: 5338 RVA: 0x0001E6A9 File Offset: 0x0001C8A9
		public Utf8String DestinationFilename
		{
			set
			{
				Helper.Set(value, ref this.m_DestinationFilename);
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0001E6B9 File Offset: 0x0001C8B9
		public void Set(ref DuplicateFileOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.SourceFilename = other.SourceFilename;
			this.DestinationFilename = other.DestinationFilename;
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0001E6EC File Offset: 0x0001C8EC
		public void Set(ref DuplicateFileOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.SourceFilename = other.Value.SourceFilename;
				this.DestinationFilename = other.Value.DestinationFilename;
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0001E74C File Offset: 0x0001C94C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SourceFilename);
			Helper.Dispose(ref this.m_DestinationFilename);
		}

		// Token: 0x04000927 RID: 2343
		private int m_ApiVersion;

		// Token: 0x04000928 RID: 2344
		private IntPtr m_LocalUserId;

		// Token: 0x04000929 RID: 2345
		private IntPtr m_SourceFilename;

		// Token: 0x0400092A RID: 2346
		private IntPtr m_DestinationFilename;
	}
}
