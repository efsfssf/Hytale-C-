using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002B5 RID: 693
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SubmitSnapshotCallbackInfoInternal : ICallbackInfoInternal, IGettable<SubmitSnapshotCallbackInfo>, ISettable<SubmitSnapshotCallbackInfo>, IDisposable
	{
		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x0001C028 File Offset: 0x0001A228
		// (set) Token: 0x06001341 RID: 4929 RVA: 0x0001C040 File Offset: 0x0001A240
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001342 RID: 4930 RVA: 0x0001C04C File Offset: 0x0001A24C
		// (set) Token: 0x06001343 RID: 4931 RVA: 0x0001C064 File Offset: 0x0001A264
		public uint SnapshotId
		{
			get
			{
				return this.m_SnapshotId;
			}
			set
			{
				this.m_SnapshotId = value;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x0001C070 File Offset: 0x0001A270
		// (set) Token: 0x06001345 RID: 4933 RVA: 0x0001C091 File Offset: 0x0001A291
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0001C0A4 File Offset: 0x0001A2A4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0001C0BC File Offset: 0x0001A2BC
		public void Set(ref SubmitSnapshotCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.SnapshotId = other.SnapshotId;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0001C0E8 File Offset: 0x0001A2E8
		public void Set(ref SubmitSnapshotCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.SnapshotId = other.Value.SnapshotId;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0001C141 File Offset: 0x0001A341
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0001C150 File Offset: 0x0001A350
		public void Get(out SubmitSnapshotCallbackInfo output)
		{
			output = default(SubmitSnapshotCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000871 RID: 2161
		private Result m_ResultCode;

		// Token: 0x04000872 RID: 2162
		private uint m_SnapshotId;

		// Token: 0x04000873 RID: 2163
		private IntPtr m_ClientData;
	}
}
