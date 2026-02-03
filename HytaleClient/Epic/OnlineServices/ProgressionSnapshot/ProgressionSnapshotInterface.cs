using System;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002B3 RID: 691
	public sealed class ProgressionSnapshotInterface : Handle
	{
		// Token: 0x0600132F RID: 4911 RVA: 0x0001BDD1 File Offset: 0x00019FD1
		public ProgressionSnapshotInterface()
		{
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0001BDDB File Offset: 0x00019FDB
		public ProgressionSnapshotInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0001BDE8 File Offset: 0x00019FE8
		public Result AddProgression(ref AddProgressionOptions options)
		{
			AddProgressionOptionsInternal addProgressionOptionsInternal = default(AddProgressionOptionsInternal);
			addProgressionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_ProgressionSnapshot_AddProgression(base.InnerHandle, ref addProgressionOptionsInternal);
			Helper.Dispose<AddProgressionOptionsInternal>(ref addProgressionOptionsInternal);
			return result;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0001BE24 File Offset: 0x0001A024
		public Result BeginSnapshot(ref BeginSnapshotOptions options, out uint outSnapshotId)
		{
			BeginSnapshotOptionsInternal beginSnapshotOptionsInternal = default(BeginSnapshotOptionsInternal);
			beginSnapshotOptionsInternal.Set(ref options);
			outSnapshotId = Helper.GetDefault<uint>();
			Result result = Bindings.EOS_ProgressionSnapshot_BeginSnapshot(base.InnerHandle, ref beginSnapshotOptionsInternal, ref outSnapshotId);
			Helper.Dispose<BeginSnapshotOptionsInternal>(ref beginSnapshotOptionsInternal);
			return result;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0001BE68 File Offset: 0x0001A068
		public void DeleteSnapshot(ref DeleteSnapshotOptions options, object clientData, OnDeleteSnapshotCallback completionDelegate)
		{
			DeleteSnapshotOptionsInternal deleteSnapshotOptionsInternal = default(DeleteSnapshotOptionsInternal);
			deleteSnapshotOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDeleteSnapshotCallbackInternal onDeleteSnapshotCallbackInternal = new OnDeleteSnapshotCallbackInternal(ProgressionSnapshotInterface.OnDeleteSnapshotCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onDeleteSnapshotCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_ProgressionSnapshot_DeleteSnapshot(base.InnerHandle, ref deleteSnapshotOptionsInternal, zero, onDeleteSnapshotCallbackInternal);
			Helper.Dispose<DeleteSnapshotOptionsInternal>(ref deleteSnapshotOptionsInternal);
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0001BEC4 File Offset: 0x0001A0C4
		public Result EndSnapshot(ref EndSnapshotOptions options)
		{
			EndSnapshotOptionsInternal endSnapshotOptionsInternal = default(EndSnapshotOptionsInternal);
			endSnapshotOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_ProgressionSnapshot_EndSnapshot(base.InnerHandle, ref endSnapshotOptionsInternal);
			Helper.Dispose<EndSnapshotOptionsInternal>(ref endSnapshotOptionsInternal);
			return result;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0001BF00 File Offset: 0x0001A100
		public void SubmitSnapshot(ref SubmitSnapshotOptions options, object clientData, OnSubmitSnapshotCallback completionDelegate)
		{
			SubmitSnapshotOptionsInternal submitSnapshotOptionsInternal = default(SubmitSnapshotOptionsInternal);
			submitSnapshotOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSubmitSnapshotCallbackInternal onSubmitSnapshotCallbackInternal = new OnSubmitSnapshotCallbackInternal(ProgressionSnapshotInterface.OnSubmitSnapshotCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSubmitSnapshotCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_ProgressionSnapshot_SubmitSnapshot(base.InnerHandle, ref submitSnapshotOptionsInternal, zero, onSubmitSnapshotCallbackInternal);
			Helper.Dispose<SubmitSnapshotOptionsInternal>(ref submitSnapshotOptionsInternal);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0001BF5C File Offset: 0x0001A15C
		[MonoPInvokeCallback(typeof(OnDeleteSnapshotCallbackInternal))]
		internal static void OnDeleteSnapshotCallbackInternalImplementation(ref DeleteSnapshotCallbackInfoInternal data)
		{
			OnDeleteSnapshotCallback onDeleteSnapshotCallback;
			DeleteSnapshotCallbackInfo deleteSnapshotCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DeleteSnapshotCallbackInfoInternal, OnDeleteSnapshotCallback, DeleteSnapshotCallbackInfo>(ref data, out onDeleteSnapshotCallback, out deleteSnapshotCallbackInfo);
			if (flag)
			{
				onDeleteSnapshotCallback(ref deleteSnapshotCallbackInfo);
			}
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0001BF84 File Offset: 0x0001A184
		[MonoPInvokeCallback(typeof(OnSubmitSnapshotCallbackInternal))]
		internal static void OnSubmitSnapshotCallbackInternalImplementation(ref SubmitSnapshotCallbackInfoInternal data)
		{
			OnSubmitSnapshotCallback onSubmitSnapshotCallback;
			SubmitSnapshotCallbackInfo submitSnapshotCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SubmitSnapshotCallbackInfoInternal, OnSubmitSnapshotCallback, SubmitSnapshotCallbackInfo>(ref data, out onSubmitSnapshotCallback, out submitSnapshotCallbackInfo);
			if (flag)
			{
				onSubmitSnapshotCallback(ref submitSnapshotCallbackInfo);
			}
		}

		// Token: 0x04000868 RID: 2152
		public const int AddprogressionApiLatest = 1;

		// Token: 0x04000869 RID: 2153
		public const int BeginsnapshotApiLatest = 1;

		// Token: 0x0400086A RID: 2154
		public const int DeletesnapshotApiLatest = 1;

		// Token: 0x0400086B RID: 2155
		public const int EndsnapshotApiLatest = 1;

		// Token: 0x0400086C RID: 2156
		public const int InvalidProgressionsnapshotid = 0;

		// Token: 0x0400086D RID: 2157
		public const int SubmitsnapshotApiLatest = 1;
	}
}
