using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C1 RID: 705
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DataRecordInternal : IGettable<DataRecord>, ISettable<DataRecord>, IDisposable
	{
		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0001C3B8 File Offset: 0x0001A5B8
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0001C3D9 File Offset: 0x0001A5D9
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

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0001C3EC File Offset: 0x0001A5EC
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x0001C40D File Offset: 0x0001A60D
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

		// Token: 0x0600136F RID: 4975 RVA: 0x0001C41D File Offset: 0x0001A61D
		public void Set(ref DataRecord other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
			this.Value = other.Value;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0001C444 File Offset: 0x0001A644
		public void Set(ref DataRecord? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
				this.Value = other.Value.Value;
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0001C48F File Offset: 0x0001A68F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
			Helper.Dispose(ref this.m_Value);
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0001C4AA File Offset: 0x0001A6AA
		public void Get(out DataRecord output)
		{
			output = default(DataRecord);
			output.Set(ref this);
		}

		// Token: 0x04000883 RID: 2179
		private int m_ApiVersion;

		// Token: 0x04000884 RID: 2180
		private IntPtr m_Key;

		// Token: 0x04000885 RID: 2181
		private IntPtr m_Value;
	}
}
