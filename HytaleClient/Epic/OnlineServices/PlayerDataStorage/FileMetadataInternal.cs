using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002FC RID: 764
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct FileMetadataInternal : IGettable<FileMetadata>, ISettable<FileMetadata>, IDisposable
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0001E818 File Offset: 0x0001CA18
		// (set) Token: 0x060014EA RID: 5354 RVA: 0x0001E830 File Offset: 0x0001CA30
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

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x0001E83C File Offset: 0x0001CA3C
		// (set) Token: 0x060014EC RID: 5356 RVA: 0x0001E85D File Offset: 0x0001CA5D
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

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0001E870 File Offset: 0x0001CA70
		// (set) Token: 0x060014EE RID: 5358 RVA: 0x0001E891 File Offset: 0x0001CA91
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

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0001E8A4 File Offset: 0x0001CAA4
		// (set) Token: 0x060014F0 RID: 5360 RVA: 0x0001E8C5 File Offset: 0x0001CAC5
		public DateTimeOffset? LastModifiedTime
		{
			get
			{
				DateTimeOffset? result;
				Helper.Get(this.m_LastModifiedTime, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LastModifiedTime);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0001E8D8 File Offset: 0x0001CAD8
		// (set) Token: 0x060014F2 RID: 5362 RVA: 0x0001E8F0 File Offset: 0x0001CAF0
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

		// Token: 0x060014F3 RID: 5363 RVA: 0x0001E8FC File Offset: 0x0001CAFC
		public void Set(ref FileMetadata other)
		{
			this.m_ApiVersion = 3;
			this.FileSizeBytes = other.FileSizeBytes;
			this.MD5Hash = other.MD5Hash;
			this.Filename = other.Filename;
			this.LastModifiedTime = other.LastModifiedTime;
			this.UnencryptedDataSizeBytes = other.UnencryptedDataSizeBytes;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0001E954 File Offset: 0x0001CB54
		public void Set(ref FileMetadata? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.FileSizeBytes = other.Value.FileSizeBytes;
				this.MD5Hash = other.Value.MD5Hash;
				this.Filename = other.Value.Filename;
				this.LastModifiedTime = other.Value.LastModifiedTime;
				this.UnencryptedDataSizeBytes = other.Value.UnencryptedDataSizeBytes;
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0001E9DE File Offset: 0x0001CBDE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_MD5Hash);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0001E9F9 File Offset: 0x0001CBF9
		public void Get(out FileMetadata output)
		{
			output = default(FileMetadata);
			output.Set(ref this);
		}

		// Token: 0x04000930 RID: 2352
		private int m_ApiVersion;

		// Token: 0x04000931 RID: 2353
		private uint m_FileSizeBytes;

		// Token: 0x04000932 RID: 2354
		private IntPtr m_MD5Hash;

		// Token: 0x04000933 RID: 2355
		private IntPtr m_Filename;

		// Token: 0x04000934 RID: 2356
		private long m_LastModifiedTime;

		// Token: 0x04000935 RID: 2357
		private uint m_UnencryptedDataSizeBytes;
	}
}
