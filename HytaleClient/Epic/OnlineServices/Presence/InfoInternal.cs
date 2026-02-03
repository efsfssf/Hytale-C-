using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C7 RID: 711
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct InfoInternal : IGettable<Info>, ISettable<Info>, IDisposable
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x0001C774 File Offset: 0x0001A974
		// (set) Token: 0x06001399 RID: 5017 RVA: 0x0001C78C File Offset: 0x0001A98C
		public Status Status
		{
			get
			{
				return this.m_Status;
			}
			set
			{
				this.m_Status = value;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x0001C798 File Offset: 0x0001A998
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x0001C7B9 File Offset: 0x0001A9B9
		public EpicAccountId UserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_UserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x0001C7CC File Offset: 0x0001A9CC
		// (set) Token: 0x0600139D RID: 5021 RVA: 0x0001C7ED File Offset: 0x0001A9ED
		public Utf8String ProductId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ProductId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductId);
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x0001C800 File Offset: 0x0001AA00
		// (set) Token: 0x0600139F RID: 5023 RVA: 0x0001C821 File Offset: 0x0001AA21
		public Utf8String ProductVersion
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ProductVersion, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductVersion);
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x0001C834 File Offset: 0x0001AA34
		// (set) Token: 0x060013A1 RID: 5025 RVA: 0x0001C855 File Offset: 0x0001AA55
		public Utf8String Platform
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Platform, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Platform);
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x0001C868 File Offset: 0x0001AA68
		// (set) Token: 0x060013A3 RID: 5027 RVA: 0x0001C889 File Offset: 0x0001AA89
		public Utf8String RichText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RichText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RichText);
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x0001C89C File Offset: 0x0001AA9C
		// (set) Token: 0x060013A5 RID: 5029 RVA: 0x0001C8C3 File Offset: 0x0001AAC3
		public DataRecord[] Records
		{
			get
			{
				DataRecord[] result;
				Helper.Get<DataRecordInternal, DataRecord>(this.m_Records, out result, this.m_RecordsCount);
				return result;
			}
			set
			{
				Helper.Set<DataRecord, DataRecordInternal>(ref value, ref this.m_Records, out this.m_RecordsCount);
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x0001C8DC File Offset: 0x0001AADC
		// (set) Token: 0x060013A7 RID: 5031 RVA: 0x0001C8FD File Offset: 0x0001AAFD
		public Utf8String ProductName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ProductName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductName);
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0001C910 File Offset: 0x0001AB10
		// (set) Token: 0x060013A9 RID: 5033 RVA: 0x0001C931 File Offset: 0x0001AB31
		public Utf8String IntegratedPlatform
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_IntegratedPlatform, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IntegratedPlatform);
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0001C944 File Offset: 0x0001AB44
		public void Set(ref Info other)
		{
			this.m_ApiVersion = 3;
			this.Status = other.Status;
			this.UserId = other.UserId;
			this.ProductId = other.ProductId;
			this.ProductVersion = other.ProductVersion;
			this.Platform = other.Platform;
			this.RichText = other.RichText;
			this.Records = other.Records;
			this.ProductName = other.ProductName;
			this.IntegratedPlatform = other.IntegratedPlatform;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0001C9D0 File Offset: 0x0001ABD0
		public void Set(ref Info? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.Status = other.Value.Status;
				this.UserId = other.Value.UserId;
				this.ProductId = other.Value.ProductId;
				this.ProductVersion = other.Value.ProductVersion;
				this.Platform = other.Value.Platform;
				this.RichText = other.Value.RichText;
				this.Records = other.Value.Records;
				this.ProductName = other.Value.ProductName;
				this.IntegratedPlatform = other.Value.IntegratedPlatform;
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_ProductId);
			Helper.Dispose(ref this.m_ProductVersion);
			Helper.Dispose(ref this.m_Platform);
			Helper.Dispose(ref this.m_RichText);
			Helper.Dispose(ref this.m_Records);
			Helper.Dispose(ref this.m_ProductName);
			Helper.Dispose(ref this.m_IntegratedPlatform);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0001CB22 File Offset: 0x0001AD22
		public void Get(out Info output)
		{
			output = default(Info);
			output.Set(ref this);
		}

		// Token: 0x04000899 RID: 2201
		private int m_ApiVersion;

		// Token: 0x0400089A RID: 2202
		private Status m_Status;

		// Token: 0x0400089B RID: 2203
		private IntPtr m_UserId;

		// Token: 0x0400089C RID: 2204
		private IntPtr m_ProductId;

		// Token: 0x0400089D RID: 2205
		private IntPtr m_ProductVersion;

		// Token: 0x0400089E RID: 2206
		private IntPtr m_Platform;

		// Token: 0x0400089F RID: 2207
		private IntPtr m_RichText;

		// Token: 0x040008A0 RID: 2208
		private int m_RecordsCount;

		// Token: 0x040008A1 RID: 2209
		private IntPtr m_Records;

		// Token: 0x040008A2 RID: 2210
		private IntPtr m_ProductName;

		// Token: 0x040008A3 RID: 2211
		private IntPtr m_IntegratedPlatform;
	}
}
