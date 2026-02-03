using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000526 RID: 1318
	public sealed class EcomInterface : Handle
	{
		// Token: 0x0600227B RID: 8827 RVA: 0x00032996 File Offset: 0x00030B96
		public EcomInterface()
		{
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000329A0 File Offset: 0x00030BA0
		public EcomInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x000329AC File Offset: 0x00030BAC
		public void Checkout(ref CheckoutOptions options, object clientData, OnCheckoutCallback completionDelegate)
		{
			CheckoutOptionsInternal checkoutOptionsInternal = default(CheckoutOptionsInternal);
			checkoutOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnCheckoutCallbackInternal onCheckoutCallbackInternal = new OnCheckoutCallbackInternal(EcomInterface.OnCheckoutCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onCheckoutCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_Checkout(base.InnerHandle, ref checkoutOptionsInternal, zero, onCheckoutCallbackInternal);
			Helper.Dispose<CheckoutOptionsInternal>(ref checkoutOptionsInternal);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00032A08 File Offset: 0x00030C08
		public Result CopyEntitlementById(ref CopyEntitlementByIdOptions options, out Entitlement? outEntitlement)
		{
			CopyEntitlementByIdOptionsInternal copyEntitlementByIdOptionsInternal = default(CopyEntitlementByIdOptionsInternal);
			copyEntitlementByIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyEntitlementById(base.InnerHandle, ref copyEntitlementByIdOptionsInternal, ref zero);
			Helper.Dispose<CopyEntitlementByIdOptionsInternal>(ref copyEntitlementByIdOptionsInternal);
			Helper.Get<EntitlementInternal, Entitlement>(zero, out outEntitlement);
			bool flag = outEntitlement != null;
			if (flag)
			{
				Bindings.EOS_Ecom_Entitlement_Release(zero);
			}
			return result;
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x00032A68 File Offset: 0x00030C68
		public Result CopyEntitlementByIndex(ref CopyEntitlementByIndexOptions options, out Entitlement? outEntitlement)
		{
			CopyEntitlementByIndexOptionsInternal copyEntitlementByIndexOptionsInternal = default(CopyEntitlementByIndexOptionsInternal);
			copyEntitlementByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyEntitlementByIndex(base.InnerHandle, ref copyEntitlementByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyEntitlementByIndexOptionsInternal>(ref copyEntitlementByIndexOptionsInternal);
			Helper.Get<EntitlementInternal, Entitlement>(zero, out outEntitlement);
			bool flag = outEntitlement != null;
			if (flag)
			{
				Bindings.EOS_Ecom_Entitlement_Release(zero);
			}
			return result;
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x00032AC8 File Offset: 0x00030CC8
		public Result CopyEntitlementByNameAndIndex(ref CopyEntitlementByNameAndIndexOptions options, out Entitlement? outEntitlement)
		{
			CopyEntitlementByNameAndIndexOptionsInternal copyEntitlementByNameAndIndexOptionsInternal = default(CopyEntitlementByNameAndIndexOptionsInternal);
			copyEntitlementByNameAndIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyEntitlementByNameAndIndex(base.InnerHandle, ref copyEntitlementByNameAndIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyEntitlementByNameAndIndexOptionsInternal>(ref copyEntitlementByNameAndIndexOptionsInternal);
			Helper.Get<EntitlementInternal, Entitlement>(zero, out outEntitlement);
			bool flag = outEntitlement != null;
			if (flag)
			{
				Bindings.EOS_Ecom_Entitlement_Release(zero);
			}
			return result;
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00032B28 File Offset: 0x00030D28
		public Result CopyItemById(ref CopyItemByIdOptions options, out CatalogItem? outItem)
		{
			CopyItemByIdOptionsInternal copyItemByIdOptionsInternal = default(CopyItemByIdOptionsInternal);
			copyItemByIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyItemById(base.InnerHandle, ref copyItemByIdOptionsInternal, ref zero);
			Helper.Dispose<CopyItemByIdOptionsInternal>(ref copyItemByIdOptionsInternal);
			Helper.Get<CatalogItemInternal, CatalogItem>(zero, out outItem);
			bool flag = outItem != null;
			if (flag)
			{
				Bindings.EOS_Ecom_CatalogItem_Release(zero);
			}
			return result;
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00032B88 File Offset: 0x00030D88
		public Result CopyItemImageInfoByIndex(ref CopyItemImageInfoByIndexOptions options, out KeyImageInfo? outImageInfo)
		{
			CopyItemImageInfoByIndexOptionsInternal copyItemImageInfoByIndexOptionsInternal = default(CopyItemImageInfoByIndexOptionsInternal);
			copyItemImageInfoByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyItemImageInfoByIndex(base.InnerHandle, ref copyItemImageInfoByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyItemImageInfoByIndexOptionsInternal>(ref copyItemImageInfoByIndexOptionsInternal);
			Helper.Get<KeyImageInfoInternal, KeyImageInfo>(zero, out outImageInfo);
			bool flag = outImageInfo != null;
			if (flag)
			{
				Bindings.EOS_Ecom_KeyImageInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00032BE8 File Offset: 0x00030DE8
		public Result CopyItemReleaseByIndex(ref CopyItemReleaseByIndexOptions options, out CatalogRelease? outRelease)
		{
			CopyItemReleaseByIndexOptionsInternal copyItemReleaseByIndexOptionsInternal = default(CopyItemReleaseByIndexOptionsInternal);
			copyItemReleaseByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyItemReleaseByIndex(base.InnerHandle, ref copyItemReleaseByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyItemReleaseByIndexOptionsInternal>(ref copyItemReleaseByIndexOptionsInternal);
			Helper.Get<CatalogReleaseInternal, CatalogRelease>(zero, out outRelease);
			bool flag = outRelease != null;
			if (flag)
			{
				Bindings.EOS_Ecom_CatalogRelease_Release(zero);
			}
			return result;
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00032C48 File Offset: 0x00030E48
		public Result CopyLastRedeemedEntitlementByIndex(ref CopyLastRedeemedEntitlementByIndexOptions options, out Utf8String outRedeemedEntitlementId)
		{
			CopyLastRedeemedEntitlementByIndexOptionsInternal copyLastRedeemedEntitlementByIndexOptionsInternal = default(CopyLastRedeemedEntitlementByIndexOptionsInternal);
			copyLastRedeemedEntitlementByIndexOptionsInternal.Set(ref options);
			int size = 33;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Ecom_CopyLastRedeemedEntitlementByIndex(base.InnerHandle, ref copyLastRedeemedEntitlementByIndexOptionsInternal, intPtr, ref size);
			Helper.Dispose<CopyLastRedeemedEntitlementByIndexOptionsInternal>(ref copyLastRedeemedEntitlementByIndexOptionsInternal);
			Helper.Get(intPtr, out outRedeemedEntitlementId);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00032CA4 File Offset: 0x00030EA4
		public Result CopyOfferById(ref CopyOfferByIdOptions options, out CatalogOffer? outOffer)
		{
			CopyOfferByIdOptionsInternal copyOfferByIdOptionsInternal = default(CopyOfferByIdOptionsInternal);
			copyOfferByIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyOfferById(base.InnerHandle, ref copyOfferByIdOptionsInternal, ref zero);
			Helper.Dispose<CopyOfferByIdOptionsInternal>(ref copyOfferByIdOptionsInternal);
			Helper.Get<CatalogOfferInternal, CatalogOffer>(zero, out outOffer);
			bool flag = outOffer != null;
			if (flag)
			{
				Bindings.EOS_Ecom_CatalogOffer_Release(zero);
			}
			return result;
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00032D04 File Offset: 0x00030F04
		public Result CopyOfferByIndex(ref CopyOfferByIndexOptions options, out CatalogOffer? outOffer)
		{
			CopyOfferByIndexOptionsInternal copyOfferByIndexOptionsInternal = default(CopyOfferByIndexOptionsInternal);
			copyOfferByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyOfferByIndex(base.InnerHandle, ref copyOfferByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyOfferByIndexOptionsInternal>(ref copyOfferByIndexOptionsInternal);
			Helper.Get<CatalogOfferInternal, CatalogOffer>(zero, out outOffer);
			bool flag = outOffer != null;
			if (flag)
			{
				Bindings.EOS_Ecom_CatalogOffer_Release(zero);
			}
			return result;
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00032D64 File Offset: 0x00030F64
		public Result CopyOfferImageInfoByIndex(ref CopyOfferImageInfoByIndexOptions options, out KeyImageInfo? outImageInfo)
		{
			CopyOfferImageInfoByIndexOptionsInternal copyOfferImageInfoByIndexOptionsInternal = default(CopyOfferImageInfoByIndexOptionsInternal);
			copyOfferImageInfoByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyOfferImageInfoByIndex(base.InnerHandle, ref copyOfferImageInfoByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyOfferImageInfoByIndexOptionsInternal>(ref copyOfferImageInfoByIndexOptionsInternal);
			Helper.Get<KeyImageInfoInternal, KeyImageInfo>(zero, out outImageInfo);
			bool flag = outImageInfo != null;
			if (flag)
			{
				Bindings.EOS_Ecom_KeyImageInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00032DC4 File Offset: 0x00030FC4
		public Result CopyOfferItemByIndex(ref CopyOfferItemByIndexOptions options, out CatalogItem? outItem)
		{
			CopyOfferItemByIndexOptionsInternal copyOfferItemByIndexOptionsInternal = default(CopyOfferItemByIndexOptionsInternal);
			copyOfferItemByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyOfferItemByIndex(base.InnerHandle, ref copyOfferItemByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyOfferItemByIndexOptionsInternal>(ref copyOfferItemByIndexOptionsInternal);
			Helper.Get<CatalogItemInternal, CatalogItem>(zero, out outItem);
			bool flag = outItem != null;
			if (flag)
			{
				Bindings.EOS_Ecom_CatalogItem_Release(zero);
			}
			return result;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00032E24 File Offset: 0x00031024
		public Result CopyTransactionById(ref CopyTransactionByIdOptions options, out Transaction outTransaction)
		{
			CopyTransactionByIdOptionsInternal copyTransactionByIdOptionsInternal = default(CopyTransactionByIdOptionsInternal);
			copyTransactionByIdOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyTransactionById(base.InnerHandle, ref copyTransactionByIdOptionsInternal, ref zero);
			Helper.Dispose<CopyTransactionByIdOptionsInternal>(ref copyTransactionByIdOptionsInternal);
			Helper.Get<Transaction>(zero, out outTransaction);
			return result;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00032E70 File Offset: 0x00031070
		public Result CopyTransactionByIndex(ref CopyTransactionByIndexOptions options, out Transaction outTransaction)
		{
			CopyTransactionByIndexOptionsInternal copyTransactionByIndexOptionsInternal = default(CopyTransactionByIndexOptionsInternal);
			copyTransactionByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Ecom_CopyTransactionByIndex(base.InnerHandle, ref copyTransactionByIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyTransactionByIndexOptionsInternal>(ref copyTransactionByIndexOptionsInternal);
			Helper.Get<Transaction>(zero, out outTransaction);
			return result;
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00032EBC File Offset: 0x000310BC
		public uint GetEntitlementsByNameCount(ref GetEntitlementsByNameCountOptions options)
		{
			GetEntitlementsByNameCountOptionsInternal getEntitlementsByNameCountOptionsInternal = default(GetEntitlementsByNameCountOptionsInternal);
			getEntitlementsByNameCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetEntitlementsByNameCount(base.InnerHandle, ref getEntitlementsByNameCountOptionsInternal);
			Helper.Dispose<GetEntitlementsByNameCountOptionsInternal>(ref getEntitlementsByNameCountOptionsInternal);
			return result;
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00032EF8 File Offset: 0x000310F8
		public uint GetEntitlementsCount(ref GetEntitlementsCountOptions options)
		{
			GetEntitlementsCountOptionsInternal getEntitlementsCountOptionsInternal = default(GetEntitlementsCountOptionsInternal);
			getEntitlementsCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetEntitlementsCount(base.InnerHandle, ref getEntitlementsCountOptionsInternal);
			Helper.Dispose<GetEntitlementsCountOptionsInternal>(ref getEntitlementsCountOptionsInternal);
			return result;
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00032F34 File Offset: 0x00031134
		public uint GetItemImageInfoCount(ref GetItemImageInfoCountOptions options)
		{
			GetItemImageInfoCountOptionsInternal getItemImageInfoCountOptionsInternal = default(GetItemImageInfoCountOptionsInternal);
			getItemImageInfoCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetItemImageInfoCount(base.InnerHandle, ref getItemImageInfoCountOptionsInternal);
			Helper.Dispose<GetItemImageInfoCountOptionsInternal>(ref getItemImageInfoCountOptionsInternal);
			return result;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00032F70 File Offset: 0x00031170
		public uint GetItemReleaseCount(ref GetItemReleaseCountOptions options)
		{
			GetItemReleaseCountOptionsInternal getItemReleaseCountOptionsInternal = default(GetItemReleaseCountOptionsInternal);
			getItemReleaseCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetItemReleaseCount(base.InnerHandle, ref getItemReleaseCountOptionsInternal);
			Helper.Dispose<GetItemReleaseCountOptionsInternal>(ref getItemReleaseCountOptionsInternal);
			return result;
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00032FAC File Offset: 0x000311AC
		public uint GetLastRedeemedEntitlementsCount(ref GetLastRedeemedEntitlementsCountOptions options)
		{
			GetLastRedeemedEntitlementsCountOptionsInternal getLastRedeemedEntitlementsCountOptionsInternal = default(GetLastRedeemedEntitlementsCountOptionsInternal);
			getLastRedeemedEntitlementsCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetLastRedeemedEntitlementsCount(base.InnerHandle, ref getLastRedeemedEntitlementsCountOptionsInternal);
			Helper.Dispose<GetLastRedeemedEntitlementsCountOptionsInternal>(ref getLastRedeemedEntitlementsCountOptionsInternal);
			return result;
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x00032FE8 File Offset: 0x000311E8
		public uint GetOfferCount(ref GetOfferCountOptions options)
		{
			GetOfferCountOptionsInternal getOfferCountOptionsInternal = default(GetOfferCountOptionsInternal);
			getOfferCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetOfferCount(base.InnerHandle, ref getOfferCountOptionsInternal);
			Helper.Dispose<GetOfferCountOptionsInternal>(ref getOfferCountOptionsInternal);
			return result;
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x00033024 File Offset: 0x00031224
		public uint GetOfferImageInfoCount(ref GetOfferImageInfoCountOptions options)
		{
			GetOfferImageInfoCountOptionsInternal getOfferImageInfoCountOptionsInternal = default(GetOfferImageInfoCountOptionsInternal);
			getOfferImageInfoCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetOfferImageInfoCount(base.InnerHandle, ref getOfferImageInfoCountOptionsInternal);
			Helper.Dispose<GetOfferImageInfoCountOptionsInternal>(ref getOfferImageInfoCountOptionsInternal);
			return result;
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x00033060 File Offset: 0x00031260
		public uint GetOfferItemCount(ref GetOfferItemCountOptions options)
		{
			GetOfferItemCountOptionsInternal getOfferItemCountOptionsInternal = default(GetOfferItemCountOptionsInternal);
			getOfferItemCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetOfferItemCount(base.InnerHandle, ref getOfferItemCountOptionsInternal);
			Helper.Dispose<GetOfferItemCountOptionsInternal>(ref getOfferItemCountOptionsInternal);
			return result;
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x0003309C File Offset: 0x0003129C
		public uint GetTransactionCount(ref GetTransactionCountOptions options)
		{
			GetTransactionCountOptionsInternal getTransactionCountOptionsInternal = default(GetTransactionCountOptionsInternal);
			getTransactionCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_Ecom_GetTransactionCount(base.InnerHandle, ref getTransactionCountOptionsInternal);
			Helper.Dispose<GetTransactionCountOptionsInternal>(ref getTransactionCountOptionsInternal);
			return result;
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000330D8 File Offset: 0x000312D8
		public void QueryEntitlementToken(ref QueryEntitlementTokenOptions options, object clientData, OnQueryEntitlementTokenCallback completionDelegate)
		{
			QueryEntitlementTokenOptionsInternal queryEntitlementTokenOptionsInternal = default(QueryEntitlementTokenOptionsInternal);
			queryEntitlementTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryEntitlementTokenCallbackInternal onQueryEntitlementTokenCallbackInternal = new OnQueryEntitlementTokenCallbackInternal(EcomInterface.OnQueryEntitlementTokenCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryEntitlementTokenCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_QueryEntitlementToken(base.InnerHandle, ref queryEntitlementTokenOptionsInternal, zero, onQueryEntitlementTokenCallbackInternal);
			Helper.Dispose<QueryEntitlementTokenOptionsInternal>(ref queryEntitlementTokenOptionsInternal);
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x00033134 File Offset: 0x00031334
		public void QueryEntitlements(ref QueryEntitlementsOptions options, object clientData, OnQueryEntitlementsCallback completionDelegate)
		{
			QueryEntitlementsOptionsInternal queryEntitlementsOptionsInternal = default(QueryEntitlementsOptionsInternal);
			queryEntitlementsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryEntitlementsCallbackInternal onQueryEntitlementsCallbackInternal = new OnQueryEntitlementsCallbackInternal(EcomInterface.OnQueryEntitlementsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryEntitlementsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_QueryEntitlements(base.InnerHandle, ref queryEntitlementsOptionsInternal, zero, onQueryEntitlementsCallbackInternal);
			Helper.Dispose<QueryEntitlementsOptionsInternal>(ref queryEntitlementsOptionsInternal);
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x00033190 File Offset: 0x00031390
		public void QueryOffers(ref QueryOffersOptions options, object clientData, OnQueryOffersCallback completionDelegate)
		{
			QueryOffersOptionsInternal queryOffersOptionsInternal = default(QueryOffersOptionsInternal);
			queryOffersOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryOffersCallbackInternal onQueryOffersCallbackInternal = new OnQueryOffersCallbackInternal(EcomInterface.OnQueryOffersCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryOffersCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_QueryOffers(base.InnerHandle, ref queryOffersOptionsInternal, zero, onQueryOffersCallbackInternal);
			Helper.Dispose<QueryOffersOptionsInternal>(ref queryOffersOptionsInternal);
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x000331EC File Offset: 0x000313EC
		public void QueryOwnership(ref QueryOwnershipOptions options, object clientData, OnQueryOwnershipCallback completionDelegate)
		{
			QueryOwnershipOptionsInternal queryOwnershipOptionsInternal = default(QueryOwnershipOptionsInternal);
			queryOwnershipOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryOwnershipCallbackInternal onQueryOwnershipCallbackInternal = new OnQueryOwnershipCallbackInternal(EcomInterface.OnQueryOwnershipCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryOwnershipCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_QueryOwnership(base.InnerHandle, ref queryOwnershipOptionsInternal, zero, onQueryOwnershipCallbackInternal);
			Helper.Dispose<QueryOwnershipOptionsInternal>(ref queryOwnershipOptionsInternal);
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x00033248 File Offset: 0x00031448
		public void QueryOwnershipBySandboxIds(ref QueryOwnershipBySandboxIdsOptions options, object clientData, OnQueryOwnershipBySandboxIdsCallback completionDelegate)
		{
			QueryOwnershipBySandboxIdsOptionsInternal queryOwnershipBySandboxIdsOptionsInternal = default(QueryOwnershipBySandboxIdsOptionsInternal);
			queryOwnershipBySandboxIdsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryOwnershipBySandboxIdsCallbackInternal onQueryOwnershipBySandboxIdsCallbackInternal = new OnQueryOwnershipBySandboxIdsCallbackInternal(EcomInterface.OnQueryOwnershipBySandboxIdsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryOwnershipBySandboxIdsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_QueryOwnershipBySandboxIds(base.InnerHandle, ref queryOwnershipBySandboxIdsOptionsInternal, zero, onQueryOwnershipBySandboxIdsCallbackInternal);
			Helper.Dispose<QueryOwnershipBySandboxIdsOptionsInternal>(ref queryOwnershipBySandboxIdsOptionsInternal);
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000332A4 File Offset: 0x000314A4
		public void QueryOwnershipToken(ref QueryOwnershipTokenOptions options, object clientData, OnQueryOwnershipTokenCallback completionDelegate)
		{
			QueryOwnershipTokenOptionsInternal queryOwnershipTokenOptionsInternal = default(QueryOwnershipTokenOptionsInternal);
			queryOwnershipTokenOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryOwnershipTokenCallbackInternal onQueryOwnershipTokenCallbackInternal = new OnQueryOwnershipTokenCallbackInternal(EcomInterface.OnQueryOwnershipTokenCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onQueryOwnershipTokenCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_QueryOwnershipToken(base.InnerHandle, ref queryOwnershipTokenOptionsInternal, zero, onQueryOwnershipTokenCallbackInternal);
			Helper.Dispose<QueryOwnershipTokenOptionsInternal>(ref queryOwnershipTokenOptionsInternal);
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x00033300 File Offset: 0x00031500
		public void RedeemEntitlements(ref RedeemEntitlementsOptions options, object clientData, OnRedeemEntitlementsCallback completionDelegate)
		{
			RedeemEntitlementsOptionsInternal redeemEntitlementsOptionsInternal = default(RedeemEntitlementsOptionsInternal);
			redeemEntitlementsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnRedeemEntitlementsCallbackInternal onRedeemEntitlementsCallbackInternal = new OnRedeemEntitlementsCallbackInternal(EcomInterface.OnRedeemEntitlementsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onRedeemEntitlementsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Ecom_RedeemEntitlements(base.InnerHandle, ref redeemEntitlementsOptionsInternal, zero, onRedeemEntitlementsCallbackInternal);
			Helper.Dispose<RedeemEntitlementsOptionsInternal>(ref redeemEntitlementsOptionsInternal);
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x0003335C File Offset: 0x0003155C
		[MonoPInvokeCallback(typeof(OnCheckoutCallbackInternal))]
		internal static void OnCheckoutCallbackInternalImplementation(ref CheckoutCallbackInfoInternal data)
		{
			OnCheckoutCallback onCheckoutCallback;
			CheckoutCallbackInfo checkoutCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<CheckoutCallbackInfoInternal, OnCheckoutCallback, CheckoutCallbackInfo>(ref data, out onCheckoutCallback, out checkoutCallbackInfo);
			if (flag)
			{
				onCheckoutCallback(ref checkoutCallbackInfo);
			}
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00033384 File Offset: 0x00031584
		[MonoPInvokeCallback(typeof(OnQueryEntitlementTokenCallbackInternal))]
		internal static void OnQueryEntitlementTokenCallbackInternalImplementation(ref QueryEntitlementTokenCallbackInfoInternal data)
		{
			OnQueryEntitlementTokenCallback onQueryEntitlementTokenCallback;
			QueryEntitlementTokenCallbackInfo queryEntitlementTokenCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryEntitlementTokenCallbackInfoInternal, OnQueryEntitlementTokenCallback, QueryEntitlementTokenCallbackInfo>(ref data, out onQueryEntitlementTokenCallback, out queryEntitlementTokenCallbackInfo);
			if (flag)
			{
				onQueryEntitlementTokenCallback(ref queryEntitlementTokenCallbackInfo);
			}
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x000333AC File Offset: 0x000315AC
		[MonoPInvokeCallback(typeof(OnQueryEntitlementsCallbackInternal))]
		internal static void OnQueryEntitlementsCallbackInternalImplementation(ref QueryEntitlementsCallbackInfoInternal data)
		{
			OnQueryEntitlementsCallback onQueryEntitlementsCallback;
			QueryEntitlementsCallbackInfo queryEntitlementsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryEntitlementsCallbackInfoInternal, OnQueryEntitlementsCallback, QueryEntitlementsCallbackInfo>(ref data, out onQueryEntitlementsCallback, out queryEntitlementsCallbackInfo);
			if (flag)
			{
				onQueryEntitlementsCallback(ref queryEntitlementsCallbackInfo);
			}
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x000333D4 File Offset: 0x000315D4
		[MonoPInvokeCallback(typeof(OnQueryOffersCallbackInternal))]
		internal static void OnQueryOffersCallbackInternalImplementation(ref QueryOffersCallbackInfoInternal data)
		{
			OnQueryOffersCallback onQueryOffersCallback;
			QueryOffersCallbackInfo queryOffersCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryOffersCallbackInfoInternal, OnQueryOffersCallback, QueryOffersCallbackInfo>(ref data, out onQueryOffersCallback, out queryOffersCallbackInfo);
			if (flag)
			{
				onQueryOffersCallback(ref queryOffersCallbackInfo);
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x000333FC File Offset: 0x000315FC
		[MonoPInvokeCallback(typeof(OnQueryOwnershipBySandboxIdsCallbackInternal))]
		internal static void OnQueryOwnershipBySandboxIdsCallbackInternalImplementation(ref QueryOwnershipBySandboxIdsCallbackInfoInternal data)
		{
			OnQueryOwnershipBySandboxIdsCallback onQueryOwnershipBySandboxIdsCallback;
			QueryOwnershipBySandboxIdsCallbackInfo queryOwnershipBySandboxIdsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryOwnershipBySandboxIdsCallbackInfoInternal, OnQueryOwnershipBySandboxIdsCallback, QueryOwnershipBySandboxIdsCallbackInfo>(ref data, out onQueryOwnershipBySandboxIdsCallback, out queryOwnershipBySandboxIdsCallbackInfo);
			if (flag)
			{
				onQueryOwnershipBySandboxIdsCallback(ref queryOwnershipBySandboxIdsCallbackInfo);
			}
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x00033424 File Offset: 0x00031624
		[MonoPInvokeCallback(typeof(OnQueryOwnershipCallbackInternal))]
		internal static void OnQueryOwnershipCallbackInternalImplementation(ref QueryOwnershipCallbackInfoInternal data)
		{
			OnQueryOwnershipCallback onQueryOwnershipCallback;
			QueryOwnershipCallbackInfo queryOwnershipCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryOwnershipCallbackInfoInternal, OnQueryOwnershipCallback, QueryOwnershipCallbackInfo>(ref data, out onQueryOwnershipCallback, out queryOwnershipCallbackInfo);
			if (flag)
			{
				onQueryOwnershipCallback(ref queryOwnershipCallbackInfo);
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x0003344C File Offset: 0x0003164C
		[MonoPInvokeCallback(typeof(OnQueryOwnershipTokenCallbackInternal))]
		internal static void OnQueryOwnershipTokenCallbackInternalImplementation(ref QueryOwnershipTokenCallbackInfoInternal data)
		{
			OnQueryOwnershipTokenCallback onQueryOwnershipTokenCallback;
			QueryOwnershipTokenCallbackInfo queryOwnershipTokenCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryOwnershipTokenCallbackInfoInternal, OnQueryOwnershipTokenCallback, QueryOwnershipTokenCallbackInfo>(ref data, out onQueryOwnershipTokenCallback, out queryOwnershipTokenCallbackInfo);
			if (flag)
			{
				onQueryOwnershipTokenCallback(ref queryOwnershipTokenCallbackInfo);
			}
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x00033474 File Offset: 0x00031674
		[MonoPInvokeCallback(typeof(OnRedeemEntitlementsCallbackInternal))]
		internal static void OnRedeemEntitlementsCallbackInternalImplementation(ref RedeemEntitlementsCallbackInfoInternal data)
		{
			OnRedeemEntitlementsCallback onRedeemEntitlementsCallback;
			RedeemEntitlementsCallbackInfo redeemEntitlementsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<RedeemEntitlementsCallbackInfoInternal, OnRedeemEntitlementsCallback, RedeemEntitlementsCallbackInfo>(ref data, out onRedeemEntitlementsCallback, out redeemEntitlementsCallbackInfo);
			if (flag)
			{
				onRedeemEntitlementsCallback(ref redeemEntitlementsCallbackInfo);
			}
		}

		// Token: 0x04000F0B RID: 3851
		public const int CatalogitemApiLatest = 1;

		// Token: 0x04000F0C RID: 3852
		public const int CatalogitemEntitlementendtimestampUndefined = -1;

		// Token: 0x04000F0D RID: 3853
		public const int CatalogofferApiLatest = 5;

		// Token: 0x04000F0E RID: 3854
		public const int CatalogofferEffectivedatetimestampUndefined = -1;

		// Token: 0x04000F0F RID: 3855
		public const int CatalogofferExpirationtimestampUndefined = -1;

		// Token: 0x04000F10 RID: 3856
		public const int CatalogofferReleasedatetimestampUndefined = -1;

		// Token: 0x04000F11 RID: 3857
		public const int CatalogreleaseApiLatest = 1;

		// Token: 0x04000F12 RID: 3858
		public const int CheckoutApiLatest = 2;

		// Token: 0x04000F13 RID: 3859
		public const int CheckoutMaxEntries = 10;

		// Token: 0x04000F14 RID: 3860
		public const int CheckoutentryApiLatest = 1;

		// Token: 0x04000F15 RID: 3861
		public const int CopyentitlementbyidApiLatest = 2;

		// Token: 0x04000F16 RID: 3862
		public const int CopyentitlementbyindexApiLatest = 1;

		// Token: 0x04000F17 RID: 3863
		public const int CopyentitlementbynameandindexApiLatest = 1;

		// Token: 0x04000F18 RID: 3864
		public const int CopyitembyidApiLatest = 1;

		// Token: 0x04000F19 RID: 3865
		public const int CopyitemimageinfobyindexApiLatest = 1;

		// Token: 0x04000F1A RID: 3866
		public const int CopyitemreleasebyindexApiLatest = 1;

		// Token: 0x04000F1B RID: 3867
		public const int CopylastredeemedentitlementbyindexApiLatest = 1;

		// Token: 0x04000F1C RID: 3868
		public const int CopyofferbyidApiLatest = 3;

		// Token: 0x04000F1D RID: 3869
		public const int CopyofferbyindexApiLatest = 3;

		// Token: 0x04000F1E RID: 3870
		public const int CopyofferimageinfobyindexApiLatest = 1;

		// Token: 0x04000F1F RID: 3871
		public const int CopyofferitembyindexApiLatest = 1;

		// Token: 0x04000F20 RID: 3872
		public const int CopytransactionbyidApiLatest = 1;

		// Token: 0x04000F21 RID: 3873
		public const int CopytransactionbyindexApiLatest = 1;

		// Token: 0x04000F22 RID: 3874
		public const int EntitlementApiLatest = 2;

		// Token: 0x04000F23 RID: 3875
		public const int EntitlementEndtimestampUndefined = -1;

		// Token: 0x04000F24 RID: 3876
		public const int EntitlementidMaxLength = 32;

		// Token: 0x04000F25 RID: 3877
		public const int GetentitlementsbynamecountApiLatest = 1;

		// Token: 0x04000F26 RID: 3878
		public const int GetentitlementscountApiLatest = 1;

		// Token: 0x04000F27 RID: 3879
		public const int GetitemimageinfocountApiLatest = 1;

		// Token: 0x04000F28 RID: 3880
		public const int GetitemreleasecountApiLatest = 1;

		// Token: 0x04000F29 RID: 3881
		public const int GetlastredeemedentitlementscountApiLatest = 1;

		// Token: 0x04000F2A RID: 3882
		public const int GetoffercountApiLatest = 1;

		// Token: 0x04000F2B RID: 3883
		public const int GetofferimageinfocountApiLatest = 1;

		// Token: 0x04000F2C RID: 3884
		public const int GetofferitemcountApiLatest = 1;

		// Token: 0x04000F2D RID: 3885
		public const int GettransactioncountApiLatest = 1;

		// Token: 0x04000F2E RID: 3886
		public const int ItemownershipApiLatest = 1;

		// Token: 0x04000F2F RID: 3887
		public const int KeyimageinfoApiLatest = 1;

		// Token: 0x04000F30 RID: 3888
		public const int QueryentitlementsApiLatest = 3;

		// Token: 0x04000F31 RID: 3889
		public const int QueryentitlementsMaxEntitlementIds = 256;

		// Token: 0x04000F32 RID: 3890
		public const int QueryentitlementtokenApiLatest = 1;

		// Token: 0x04000F33 RID: 3891
		public const int QueryentitlementtokenMaxEntitlementIds = 32;

		// Token: 0x04000F34 RID: 3892
		public const int QueryoffersApiLatest = 1;

		// Token: 0x04000F35 RID: 3893
		public const int QueryownershipApiLatest = 2;

		// Token: 0x04000F36 RID: 3894
		public const int QueryownershipMaxCatalogIds = 400;

		// Token: 0x04000F37 RID: 3895
		public const int QueryownershipMaxSandboxIds = 10;

		// Token: 0x04000F38 RID: 3896
		public const int QueryownershipbysandboxidsoptionsApiLatest = 1;

		// Token: 0x04000F39 RID: 3897
		public const int QueryownershiptokenApiLatest = 2;

		// Token: 0x04000F3A RID: 3898
		public const int QueryownershiptokenMaxCatalogitemIds = 32;

		// Token: 0x04000F3B RID: 3899
		public const int RedeementitlementsApiLatest = 2;

		// Token: 0x04000F3C RID: 3900
		public const int RedeementitlementsMaxIds = 32;

		// Token: 0x04000F3D RID: 3901
		public const int TransactionidMaximumLength = 64;
	}
}
