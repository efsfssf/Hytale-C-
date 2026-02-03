using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B4 RID: 180
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileListCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryFileListCallbackInfo>, ISettable<QueryFileListCallbackInfo>, IDisposable
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x000097B8 File Offset: 0x000079B8
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x000097D0 File Offset: 0x000079D0
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x000097DC File Offset: 0x000079DC
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x000097FD File Offset: 0x000079FD
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00009810 File Offset: 0x00007A10
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00009828 File Offset: 0x00007A28
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00009849 File Offset: 0x00007A49
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0000985C File Offset: 0x00007A5C
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x00009874 File Offset: 0x00007A74
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

		// Token: 0x060006C5 RID: 1733 RVA: 0x0000987E File Offset: 0x00007A7E
		public void Set(ref QueryFileListCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.FileCount = other.FileCount;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000098B8 File Offset: 0x00007AB8
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

		// Token: 0x060006C7 RID: 1735 RVA: 0x00009926 File Offset: 0x00007B26
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00009941 File Offset: 0x00007B41
		public void Get(out QueryFileListCallbackInfo output)
		{
			output = default(QueryFileListCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000335 RID: 821
		private Result m_ResultCode;

		// Token: 0x04000336 RID: 822
		private IntPtr m_ClientData;

		// Token: 0x04000337 RID: 823
		private IntPtr m_LocalUserId;

		// Token: 0x04000338 RID: 824
		private uint m_FileCount;
	}
}
