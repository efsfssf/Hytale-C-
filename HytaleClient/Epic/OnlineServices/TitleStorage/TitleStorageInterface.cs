using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000C1 RID: 193
	public sealed class TitleStorageInterface : Handle
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x0000A39C File Offset: 0x0000859C
		public TitleStorageInterface()
		{
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0000A3A6 File Offset: 0x000085A6
		public TitleStorageInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0000A3B4 File Offset: 0x000085B4
		public Result CopyFileMetadataAtIndex(ref CopyFileMetadataAtIndexOptions options, out FileMetadata? outMetadata)
		{
			CopyFileMetadataAtIndexOptionsInternal copyFileMetadataAtIndexOptionsInternal = default(CopyFileMetadataAtIndexOptionsInternal);
			copyFileMetadataAtIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_TitleStorage_CopyFileMetadataAtIndex(base.InnerHandle, ref copyFileMetadataAtIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyFileMetadataAtIndexOptionsInternal>(ref copyFileMetadataAtIndexOptionsInternal);
			Helper.Get<FileMetadataInternal, FileMetadata>(zero, out outMetadata);
			bool flag = outMetadata != null;
			if (flag)
			{
				Bindings.EOS_TitleStorage_FileMetadata_Release(zero);
			}
			return result;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0000A414 File Offset: 0x00008614
		public Result CopyFileMetadataByFilename(ref CopyFileMetadataByFilenameOptions options, out FileMetadata? outMetadata)
		{
			CopyFileMetadataByFilenameOptionsInternal copyFileMetadataByFilenameOptionsInternal = default(CopyFileMetadataByFilenameOptionsInternal);
			copyFileMetadataByFilenameOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_TitleStorage_CopyFileMetadataByFilename(base.InnerHandle, ref copyFileMetadataByFilenameOptionsInternal, ref zero);
			Helper.Dispose<CopyFileMetadataByFilenameOptionsInternal>(ref copyFileMetadataByFilenameOptionsInternal);
			Helper.Get<FileMetadataInternal, FileMetadata>(zero, out outMetadata);
			bool flag = outMetadata != null;
			if (flag)
			{
				Bindings.EOS_TitleStorage_FileMetadata_Release(zero);
			}
			return result;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000A474 File Offset: 0x00008674
		public Result DeleteCache(ref DeleteCacheOptions options, object clientData, OnDeleteCacheCompleteCallback completionCallback)
		{
			DeleteCacheOptionsInternal deleteCacheOptionsInternal = default(DeleteCacheOptionsInternal);
			deleteCacheOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDeleteCacheCompleteCallbackInternal onDeleteCacheCompleteCallbackInternal = new OnDeleteCacheCompleteCallbackInternal(TitleStorageInterface.OnDeleteCacheCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onDeleteCacheCompleteCallbackInternal, Array.Empty<Delegate>());
			Result result = Bindings.EOS_TitleStorage_DeleteCache(base.InnerHandle, ref deleteCacheOptionsInternal, zero, onDeleteCacheCompleteCallbackInternal);
			Helper.Dispose<DeleteCacheOptionsInternal>(ref deleteCacheOptionsInternal);
			return result;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000A4D8 File Offset: 0x000086D8
		public uint GetFileMetadataCount(ref GetFileMetadataCountOptions options)
		{
			GetFileMetadataCountOptionsInternal getFileMetadataCountOptionsInternal = default(GetFileMetadataCountOptionsInternal);
			getFileMetadataCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_TitleStorage_GetFileMetadataCount(base.InnerHandle, ref getFileMetadataCountOptionsInternal);
			Helper.Dispose<GetFileMetadataCountOptionsInternal>(ref getFileMetadataCountOptionsInternal);
			return result;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000A514 File Offset: 0x00008714
		public void QueryFile(ref QueryFileOptions options, object clientData, OnQueryFileCompleteCallback completionCallback)
		{
			QueryFileOptionsInternal queryFileOptionsInternal = default(QueryFileOptionsInternal);
			queryFileOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryFileCompleteCallbackInternal onQueryFileCompleteCallbackInternal = new OnQueryFileCompleteCallbackInternal(TitleStorageInterface.OnQueryFileCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onQueryFileCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_TitleStorage_QueryFile(base.InnerHandle, ref queryFileOptionsInternal, zero, onQueryFileCompleteCallbackInternal);
			Helper.Dispose<QueryFileOptionsInternal>(ref queryFileOptionsInternal);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000A570 File Offset: 0x00008770
		public void QueryFileList(ref QueryFileListOptions options, object clientData, OnQueryFileListCompleteCallback completionCallback)
		{
			QueryFileListOptionsInternal queryFileListOptionsInternal = default(QueryFileListOptionsInternal);
			queryFileListOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnQueryFileListCompleteCallbackInternal onQueryFileListCompleteCallbackInternal = new OnQueryFileListCompleteCallbackInternal(TitleStorageInterface.OnQueryFileListCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onQueryFileListCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_TitleStorage_QueryFileList(base.InnerHandle, ref queryFileListOptionsInternal, zero, onQueryFileListCompleteCallbackInternal);
			Helper.Dispose<QueryFileListOptionsInternal>(ref queryFileListOptionsInternal);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0000A5CC File Offset: 0x000087CC
		public TitleStorageFileTransferRequest ReadFile(ref ReadFileOptions options, object clientData, OnReadFileCompleteCallback completionCallback)
		{
			ReadFileOptionsInternal readFileOptionsInternal = default(ReadFileOptionsInternal);
			readFileOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnReadFileCompleteCallbackInternal onReadFileCompleteCallbackInternal = new OnReadFileCompleteCallbackInternal(TitleStorageInterface.OnReadFileCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onReadFileCompleteCallbackInternal, new Delegate[]
			{
				options.ReadFileDataCallback,
				ReadFileOptionsInternal.ReadFileDataCallback,
				options.FileTransferProgressCallback,
				ReadFileOptionsInternal.FileTransferProgressCallback
			});
			IntPtr from = Bindings.EOS_TitleStorage_ReadFile(base.InnerHandle, ref readFileOptionsInternal, zero, onReadFileCompleteCallbackInternal);
			Helper.Dispose<ReadFileOptionsInternal>(ref readFileOptionsInternal);
			TitleStorageFileTransferRequest result;
			Helper.Get<TitleStorageFileTransferRequest>(from, out result);
			return result;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0000A65C File Offset: 0x0000885C
		[MonoPInvokeCallback(typeof(OnDeleteCacheCompleteCallbackInternal))]
		internal static void OnDeleteCacheCompleteCallbackInternalImplementation(ref DeleteCacheCallbackInfoInternal data)
		{
			OnDeleteCacheCompleteCallback onDeleteCacheCompleteCallback;
			DeleteCacheCallbackInfo deleteCacheCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DeleteCacheCallbackInfoInternal, OnDeleteCacheCompleteCallback, DeleteCacheCallbackInfo>(ref data, out onDeleteCacheCompleteCallback, out deleteCacheCallbackInfo);
			if (flag)
			{
				onDeleteCacheCompleteCallback(ref deleteCacheCallbackInfo);
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0000A684 File Offset: 0x00008884
		[MonoPInvokeCallback(typeof(OnFileTransferProgressCallbackInternal))]
		internal static void OnFileTransferProgressCallbackInternalImplementation(ref FileTransferProgressCallbackInfoInternal data)
		{
			OnFileTransferProgressCallback onFileTransferProgressCallback;
			FileTransferProgressCallbackInfo fileTransferProgressCallbackInfo;
			bool flag = Helper.TryGetStructCallback<FileTransferProgressCallbackInfoInternal, OnFileTransferProgressCallback, FileTransferProgressCallbackInfo>(ref data, out onFileTransferProgressCallback, out fileTransferProgressCallbackInfo);
			if (flag)
			{
				onFileTransferProgressCallback(ref fileTransferProgressCallbackInfo);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0000A6AC File Offset: 0x000088AC
		[MonoPInvokeCallback(typeof(OnQueryFileCompleteCallbackInternal))]
		internal static void OnQueryFileCompleteCallbackInternalImplementation(ref QueryFileCallbackInfoInternal data)
		{
			OnQueryFileCompleteCallback onQueryFileCompleteCallback;
			QueryFileCallbackInfo queryFileCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryFileCallbackInfoInternal, OnQueryFileCompleteCallback, QueryFileCallbackInfo>(ref data, out onQueryFileCompleteCallback, out queryFileCallbackInfo);
			if (flag)
			{
				onQueryFileCompleteCallback(ref queryFileCallbackInfo);
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0000A6D4 File Offset: 0x000088D4
		[MonoPInvokeCallback(typeof(OnQueryFileListCompleteCallbackInternal))]
		internal static void OnQueryFileListCompleteCallbackInternalImplementation(ref QueryFileListCallbackInfoInternal data)
		{
			OnQueryFileListCompleteCallback onQueryFileListCompleteCallback;
			QueryFileListCallbackInfo queryFileListCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<QueryFileListCallbackInfoInternal, OnQueryFileListCompleteCallback, QueryFileListCallbackInfo>(ref data, out onQueryFileListCompleteCallback, out queryFileListCallbackInfo);
			if (flag)
			{
				onQueryFileListCompleteCallback(ref queryFileListCallbackInfo);
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0000A6FC File Offset: 0x000088FC
		[MonoPInvokeCallback(typeof(OnReadFileCompleteCallbackInternal))]
		internal static void OnReadFileCompleteCallbackInternalImplementation(ref ReadFileCallbackInfoInternal data)
		{
			OnReadFileCompleteCallback onReadFileCompleteCallback;
			ReadFileCallbackInfo readFileCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<ReadFileCallbackInfoInternal, OnReadFileCompleteCallback, ReadFileCallbackInfo>(ref data, out onReadFileCompleteCallback, out readFileCallbackInfo);
			if (flag)
			{
				onReadFileCompleteCallback(ref readFileCallbackInfo);
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0000A724 File Offset: 0x00008924
		[MonoPInvokeCallback(typeof(OnReadFileDataCallbackInternal))]
		internal static ReadResult OnReadFileDataCallbackInternalImplementation(ref ReadFileDataCallbackInfoInternal data)
		{
			OnReadFileDataCallback onReadFileDataCallback;
			ReadFileDataCallbackInfo readFileDataCallbackInfo;
			bool flag = Helper.TryGetStructCallback<ReadFileDataCallbackInfoInternal, OnReadFileDataCallback, ReadFileDataCallbackInfo>(ref data, out onReadFileDataCallback, out readFileDataCallbackInfo);
			ReadResult result;
			if (flag)
			{
				ReadResult readResult = onReadFileDataCallback(ref readFileDataCallbackInfo);
				result = readResult;
			}
			else
			{
				result = Helper.GetDefault<ReadResult>();
			}
			return result;
		}

		// Token: 0x0400036A RID: 874
		public const int CopyfilemetadataatindexApiLatest = 1;

		// Token: 0x0400036B RID: 875
		public const int CopyfilemetadataatindexoptionsApiLatest = 1;

		// Token: 0x0400036C RID: 876
		public const int CopyfilemetadatabyfilenameApiLatest = 1;

		// Token: 0x0400036D RID: 877
		public const int CopyfilemetadatabyfilenameoptionsApiLatest = 1;

		// Token: 0x0400036E RID: 878
		public const int DeletecacheApiLatest = 1;

		// Token: 0x0400036F RID: 879
		public const int DeletecacheoptionsApiLatest = 1;

		// Token: 0x04000370 RID: 880
		public const int FilemetadataApiLatest = 2;

		// Token: 0x04000371 RID: 881
		public const int FilenameMaxLengthBytes = 64;

		// Token: 0x04000372 RID: 882
		public const int GetfilemetadatacountApiLatest = 1;

		// Token: 0x04000373 RID: 883
		public const int GetfilemetadatacountoptionsApiLatest = 1;

		// Token: 0x04000374 RID: 884
		public const int QueryfileApiLatest = 1;

		// Token: 0x04000375 RID: 885
		public const int QueryfilelistApiLatest = 1;

		// Token: 0x04000376 RID: 886
		public const int QueryfilelistoptionsApiLatest = 1;

		// Token: 0x04000377 RID: 887
		public const int QueryfileoptionsApiLatest = 1;

		// Token: 0x04000378 RID: 888
		public const int ReadfileApiLatest = 2;

		// Token: 0x04000379 RID: 889
		public const int ReadfileoptionsApiLatest = 2;
	}
}
