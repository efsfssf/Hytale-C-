using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200031A RID: 794
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileListCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryFileListCallbackInfo>, ISettable<QueryFileListCallbackInfo>, IDisposable
	{
		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060015A1 RID: 5537 RVA: 0x0001F630 File Offset: 0x0001D830
		// (set) Token: 0x060015A2 RID: 5538 RVA: 0x0001F648 File Offset: 0x0001D848
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

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x0001F654 File Offset: 0x0001D854
		// (set) Token: 0x060015A4 RID: 5540 RVA: 0x0001F675 File Offset: 0x0001D875
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

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x0001F688 File Offset: 0x0001D888
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0001F6A0 File Offset: 0x0001D8A0
		// (set) Token: 0x060015A7 RID: 5543 RVA: 0x0001F6C1 File Offset: 0x0001D8C1
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

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0001F6D4 File Offset: 0x0001D8D4
		// (set) Token: 0x060015A9 RID: 5545 RVA: 0x0001F6EC File Offset: 0x0001D8EC
		public uint FileCount
		{
			get
			{
				return this.m_FileCount;
			}
			set
			{
				this.m_FileCount = value;
			}
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0001F6F6 File Offset: 0x0001D8F6
		public void Set(ref QueryFileListCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.FileCount = other.FileCount;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0001F730 File Offset: 0x0001D930
		public void Set(ref QueryFileListCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.FileCount = other.Value.FileCount;
			}
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0001F79E File Offset: 0x0001D99E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0001F7B9 File Offset: 0x0001D9B9
		public void Get(out QueryFileListCallbackInfo output)
		{
			output = default(QueryFileListCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000964 RID: 2404
		private Result m_ResultCode;

		// Token: 0x04000965 RID: 2405
		private IntPtr m_ClientData;

		// Token: 0x04000966 RID: 2406
		private IntPtr m_LocalUserId;

		// Token: 0x04000967 RID: 2407
		private uint m_FileCount;
	}
}
