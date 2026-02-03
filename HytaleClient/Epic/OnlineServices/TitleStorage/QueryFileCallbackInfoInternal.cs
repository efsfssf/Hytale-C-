using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B2 RID: 178
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryFileCallbackInfo>, ISettable<QueryFileCallbackInfo>, IDisposable
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000095CC File Offset: 0x000077CC
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x000095E4 File Offset: 0x000077E4
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

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000095F0 File Offset: 0x000077F0
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x00009611 File Offset: 0x00007811
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00009624 File Offset: 0x00007824
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0000963C File Offset: 0x0000783C
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x0000965D File Offset: 0x0000785D
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

		// Token: 0x060006AE RID: 1710 RVA: 0x0000966D File Offset: 0x0000786D
		public void Set(ref QueryFileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00009698 File Offset: 0x00007898
		public void Set(ref QueryFileCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000096F1 File Offset: 0x000078F1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0000970C File Offset: 0x0000790C
		public void Get(out QueryFileCallbackInfo output)
		{
			output = default(QueryFileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400032E RID: 814
		private Result m_ResultCode;

		// Token: 0x0400032F RID: 815
		private IntPtr m_ClientData;

		// Token: 0x04000330 RID: 816
		private IntPtr m_LocalUserId;
	}
}
