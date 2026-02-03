using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002AA RID: 682
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteSnapshotCallbackInfoInternal : ICallbackInfoInternal, IGettable<DeleteSnapshotCallbackInfo>, ISettable<DeleteSnapshotCallbackInfo>, IDisposable
	{
		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x0001BB90 File Offset: 0x00019D90
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x0001BBA8 File Offset: 0x00019DA8
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

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x0001BBB4 File Offset: 0x00019DB4
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x0001BBD5 File Offset: 0x00019DD5
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x0001BBE8 File Offset: 0x00019DE8
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x0001BC09 File Offset: 0x00019E09
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

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x0001BC1C File Offset: 0x00019E1C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0001BC34 File Offset: 0x00019E34
		public void Set(ref DeleteSnapshotCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0001BC60 File Offset: 0x00019E60
		public void Set(ref DeleteSnapshotCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.LocalUserId = other.Value.LocalUserId;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0001BCB9 File Offset: 0x00019EB9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0001BCD4 File Offset: 0x00019ED4
		public void Get(out DeleteSnapshotCallbackInfo output)
		{
			output = default(DeleteSnapshotCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400085F RID: 2143
		private Result m_ResultCode;

		// Token: 0x04000860 RID: 2144
		private IntPtr m_LocalUserId;

		// Token: 0x04000861 RID: 2145
		private IntPtr m_ClientData;
	}
}
