using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000A0 RID: 160
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct FileMetadataInternal : IGettable<FileMetadata>, ISettable<FileMetadata>, IDisposable
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0000907C File Offset: 0x0000727C
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x00009094 File Offset: 0x00007294
		public uint FileSizeBytes
		{
			get
			{
				return this.m_FileSizeBytes;
			}
			set
			{
				this.m_FileSizeBytes = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x000090A0 File Offset: 0x000072A0
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x000090C1 File Offset: 0x000072C1
		public Utf8String MD5Hash
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_MD5Hash, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_MD5Hash);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x000090D4 File Offset: 0x000072D4
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x000090F5 File Offset: 0x000072F5
		public Utf8String Filename
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Filename, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00009108 File Offset: 0x00007308
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x00009120 File Offset: 0x00007320
		public uint UnencryptedDataSizeBytes
		{
			get
			{
				return this.m_UnencryptedDataSizeBytes;
			}
			set
			{
				this.m_UnencryptedDataSizeBytes = value;
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0000912A File Offset: 0x0000732A
		public void Set(ref FileMetadata other)
		{
			this.m_ApiVersion = 2;
			this.FileSizeBytes = other.FileSizeBytes;
			this.MD5Hash = other.MD5Hash;
			this.Filename = other.Filename;
			this.UnencryptedDataSizeBytes = other.UnencryptedDataSizeBytes;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00009168 File Offset: 0x00007368
		public void Set(ref FileMetadata? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.FileSizeBytes = other.Value.FileSizeBytes;
				this.MD5Hash = other.Value.MD5Hash;
				this.Filename = other.Value.Filename;
				this.UnencryptedDataSizeBytes = other.Value.UnencryptedDataSizeBytes;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000091DD File Offset: 0x000073DD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_MD5Hash);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x000091F8 File Offset: 0x000073F8
		public void Get(out FileMetadata output)
		{
			output = default(FileMetadata);
			output.Set(ref this);
		}

		// Token: 0x04000319 RID: 793
		private int m_ApiVersion;

		// Token: 0x0400031A RID: 794
		private uint m_FileSizeBytes;

		// Token: 0x0400031B RID: 795
		private IntPtr m_MD5Hash;

		// Token: 0x0400031C RID: 796
		private IntPtr m_Filename;

		// Token: 0x0400031D RID: 797
		private uint m_UnencryptedDataSizeBytes;
	}
}
