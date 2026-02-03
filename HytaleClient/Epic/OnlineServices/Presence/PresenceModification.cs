using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D3 RID: 723
	public sealed class PresenceModification : Handle
	{
		// Token: 0x06001405 RID: 5125 RVA: 0x0001D3D4 File Offset: 0x0001B5D4
		public PresenceModification()
		{
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0001D3DE File Offset: 0x0001B5DE
		public PresenceModification(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0001D3EC File Offset: 0x0001B5EC
		public Result DeleteData(ref PresenceModificationDeleteDataOptions options)
		{
			PresenceModificationDeleteDataOptionsInternal presenceModificationDeleteDataOptionsInternal = default(PresenceModificationDeleteDataOptionsInternal);
			presenceModificationDeleteDataOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_PresenceModification_DeleteData(base.InnerHandle, ref presenceModificationDeleteDataOptionsInternal);
			Helper.Dispose<PresenceModificationDeleteDataOptionsInternal>(ref presenceModificationDeleteDataOptionsInternal);
			return result;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0001D426 File Offset: 0x0001B626
		public void Release()
		{
			Bindings.EOS_PresenceModification_Release(base.InnerHandle);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0001D438 File Offset: 0x0001B638
		public Result SetData(ref PresenceModificationSetDataOptions options)
		{
			PresenceModificationSetDataOptionsInternal presenceModificationSetDataOptionsInternal = default(PresenceModificationSetDataOptionsInternal);
			presenceModificationSetDataOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_PresenceModification_SetData(base.InnerHandle, ref presenceModificationSetDataOptionsInternal);
			Helper.Dispose<PresenceModificationSetDataOptionsInternal>(ref presenceModificationSetDataOptionsInternal);
			return result;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0001D474 File Offset: 0x0001B674
		public Result SetJoinInfo(ref PresenceModificationSetJoinInfoOptions options)
		{
			PresenceModificationSetJoinInfoOptionsInternal presenceModificationSetJoinInfoOptionsInternal = default(PresenceModificationSetJoinInfoOptionsInternal);
			presenceModificationSetJoinInfoOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_PresenceModification_SetJoinInfo(base.InnerHandle, ref presenceModificationSetJoinInfoOptionsInternal);
			Helper.Dispose<PresenceModificationSetJoinInfoOptionsInternal>(ref presenceModificationSetJoinInfoOptionsInternal);
			return result;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0001D4B0 File Offset: 0x0001B6B0
		public Result SetRawRichText(ref PresenceModificationSetRawRichTextOptions options)
		{
			PresenceModificationSetRawRichTextOptionsInternal presenceModificationSetRawRichTextOptionsInternal = default(PresenceModificationSetRawRichTextOptionsInternal);
			presenceModificationSetRawRichTextOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_PresenceModification_SetRawRichText(base.InnerHandle, ref presenceModificationSetRawRichTextOptionsInternal);
			Helper.Dispose<PresenceModificationSetRawRichTextOptionsInternal>(ref presenceModificationSetRawRichTextOptionsInternal);
			return result;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0001D4EC File Offset: 0x0001B6EC
		public Result SetStatus(ref PresenceModificationSetStatusOptions options)
		{
			PresenceModificationSetStatusOptionsInternal presenceModificationSetStatusOptionsInternal = default(PresenceModificationSetStatusOptionsInternal);
			presenceModificationSetStatusOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_PresenceModification_SetStatus(base.InnerHandle, ref presenceModificationSetStatusOptionsInternal);
			Helper.Dispose<PresenceModificationSetStatusOptionsInternal>(ref presenceModificationSetStatusOptionsInternal);
			return result;
		}

		// Token: 0x040008C7 RID: 2247
		public const int PresencemodificationDatarecordidApiLatest = 1;

		// Token: 0x040008C8 RID: 2248
		public const int PresencemodificationDeletedataApiLatest = 1;

		// Token: 0x040008C9 RID: 2249
		public const int PresencemodificationJoininfoMaxLength = 255;

		// Token: 0x040008CA RID: 2250
		public const int PresencemodificationSetdataApiLatest = 1;

		// Token: 0x040008CB RID: 2251
		public const int PresencemodificationSetjoininfoApiLatest = 1;

		// Token: 0x040008CC RID: 2252
		public const int PresencemodificationSetrawrichtextApiLatest = 1;

		// Token: 0x040008CD RID: 2253
		public const int PresencemodificationSetstatusApiLatest = 1;
	}
}
