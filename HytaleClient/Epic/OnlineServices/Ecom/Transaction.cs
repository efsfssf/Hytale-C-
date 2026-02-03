using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200056F RID: 1391
	public sealed class Transaction : Handle
	{
		// Token: 0x06002458 RID: 9304 RVA: 0x00035841 File Offset: 0x00033A41
		public Transaction()
		{
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0003584B File Offset: 0x00033A4B
		public Transaction(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x00035858 File Offset: 0x00033A58
		public Result CopyEntitlementByIndex(ref TransactionCopyEntitlementByIndexOptions options, out Entitlement? outEntitlement)
		{
			TransactionCopyEntitlementByIndexOptionsInternal transactionCopyEntitlementByIndexOptionsInternal = default(TransactionCopyEntitlementByIndexOptionsInternal);
			transactionCopyEntitlementByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_Transaction_CopyEntitlementByIndex(base.InnerHandle, ref transactionCopyEntitlementByIndexOptionsInternal, ref zero);
			Helper.Dispose<TransactionCopyEntitlementByIndexOptionsInternal>(ref transactionCopyEntitlementByIndexOptionsInternal);
			Helper.Get<EntitlementInternal, Entitlement>(zero, out outEntitlement);
			bool flag = outEntitlement != null;
			if (flag)
			{
				Bindings.EOS_Ecom_Entitlement_Release(zero);
			}
			return result;
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000358B8 File Offset: 0x00033AB8
		public uint GetEntitlementsCount(ref TransactionGetEntitlementsCountOptions options)
		{
			TransactionGetEntitlementsCountOptionsInternal transactionGetEntitlementsCountOptionsInternal = default(TransactionGetEntitlementsCountOptionsInternal);
			transactionGetEntitlementsCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_Transaction_GetEntitlementsCount(base.InnerHandle, ref transactionGetEntitlementsCountOptionsInternal);
			Helper.Dispose<TransactionGetEntitlementsCountOptionsInternal>(ref transactionGetEntitlementsCountOptionsInternal);
			return result;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000358F4 File Offset: 0x00033AF4
		public Result GetTransactionId(out Utf8String outBuffer)
		{
			int size = 65;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Ecom_Transaction_GetTransactionId(base.InnerHandle, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x00035930 File Offset: 0x00033B30
		public void Release()
		{
			Bindings.EOS_Ecom_Transaction_Release(base.InnerHandle);
		}

		// Token: 0x04000FF1 RID: 4081
		public const int TransactionCopyentitlementbyindexApiLatest = 1;

		// Token: 0x04000FF2 RID: 4082
		public const int TransactionGetentitlementscountApiLatest = 1;
	}
}
