using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000562 RID: 1378
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOwnershipCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipCallbackInfo>, ISettable<QueryOwnershipCallbackInfo>, IDisposable
	{
		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x00034DA0 File Offset: 0x00032FA0
		// (set) Token: 0x060023F0 RID: 9200 RVA: 0x00034DB8 File Offset: 0x00032FB8
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

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x00034DC4 File Offset: 0x00032FC4
		// (set) Token: 0x060023F2 RID: 9202 RVA: 0x00034DE5 File Offset: 0x00032FE5
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

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x00034DF8 File Offset: 0x00032FF8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x00034E10 File Offset: 0x00033010
		// (set) Token: 0x060023F5 RID: 9205 RVA: 0x00034E31 File Offset: 0x00033031
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x00034E44 File Offset: 0x00033044
		// (set) Token: 0x060023F7 RID: 9207 RVA: 0x00034E6B File Offset: 0x0003306B
		public ItemOwnership[] ItemOwnership
		{
			get
			{
				ItemOwnership[] result;
				Helper.Get<ItemOwnershipInternal, ItemOwnership>(this.m_ItemOwnership, out result, this.m_ItemOwnershipCount);
				return result;
			}
			set
			{
				Helper.Set<ItemOwnership, ItemOwnershipInternal>(ref value, ref this.m_ItemOwnership, out this.m_ItemOwnershipCount);
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x00034E82 File Offset: 0x00033082
		public void Set(ref QueryOwnershipCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.ItemOwnership = other.ItemOwnership;
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x00034EBC File Offset: 0x000330BC
		public void Set(ref QueryOwnershipCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.ItemOwnership = other.Value.ItemOwnership;
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x00034F2A File Offset: 0x0003312A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ItemOwnership);
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00034F51 File Offset: 0x00033151
		public void Get(out QueryOwnershipCallbackInfo output)
		{
			output = default(QueryOwnershipCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000FC1 RID: 4033
		private Result m_ResultCode;

		// Token: 0x04000FC2 RID: 4034
		private IntPtr m_ClientData;

		// Token: 0x04000FC3 RID: 4035
		private IntPtr m_LocalUserId;

		// Token: 0x04000FC4 RID: 4036
		private IntPtr m_ItemOwnership;

		// Token: 0x04000FC5 RID: 4037
		private uint m_ItemOwnershipCount;
	}
}
