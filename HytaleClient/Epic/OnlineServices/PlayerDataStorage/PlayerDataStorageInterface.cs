using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000316 RID: 790
	public sealed class PlayerDataStorageInterface : Handle
	{
		// Token: 0x0600156E RID: 5486 RVA: 0x0001EDF4 File Offset: 0x0001CFF4
		public PlayerDataStorageInterface()
		{
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0001EDFE File Offset: 0x0001CFFE
		public PlayerDataStorageInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0001EE0C File Offset: 0x0001D00C
		public Result CopyFileMetadataAtIndex(ref CopyFileMetadataAtIndexOptions copyFileMetadataOptions, out FileMetadata? outMetadata)
		{
			CopyFileMetadataAtIndexOptionsInternal copyFileMetadataAtIndexOptionsInternal = default(CopyFileMetadataAtIndexOptionsInternal);
			copyFileMetadataAtIndexOptionsInternal.Set(ref copyFileMetadataOptions);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_PlayerDataStorage_CopyFileMetadataAtIndex(base.InnerHandle, ref copyFileMetadataAtIndexOptionsInternal, ref zero);
			Helper.Dispose<CopyFileMetadataAtIndexOptionsInternal>(ref copyFileMetadataAtIndexOptionsInternal);
			Helper.Get<FileMetadataInternal, FileMetadata>(zero, out outMetadata);
			bool flag = outMetadata != null;
			if (flag)
			{
				Bindings.EOS_PlayerDataStorage_FileMetadata_Release(zero);
			}
			return result;
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0001EE6C File Offset: 0x0001D06C
		public Result CopyFileMetadataByFilename(ref CopyFileMetadataByFilenameOptions copyFileMetadataOptions, out FileMetadata? outMetadata)
		{
			CopyFileMetadataByFilenameOptionsInternal copyFileMetadataByFilenameOptionsInternal = default(CopyFileMetadataByFilenameOptionsInternal);
			copyFileMetadataByFilenameOptionsInternal.Set(ref copyFileMetadataOptions);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_PlayerDataStorage_CopyFileMetadataByFilename(base.InnerHandle, ref copyFileMetadataByFilenameOptionsInternal, ref zero);
			Helper.Dispose<CopyFileMetadataByFilenameOptionsInternal>(ref copyFileMetadataByFilenameOptionsInternal);
			Helper.Get<FileMetadataInternal, FileMetadata>(zero, out outMetadata);
			bool flag = outMetadata != null;
			if (flag)
			{
				Bindings.EOS_PlayerDataStorage_FileMetadata_Release(zero);
			}
			return result;
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0001EECC File Offset: 0x0001D0CC
		public Result DeleteCache(ref DeleteCacheOptions options, object clientData, OnDeleteCacheCompleteCallback completionCallback)
		{
			DeleteCacheOptionsInternal deleteCacheOptionsInternal = default(DeleteCacheOptionsInternal);
			deleteCacheOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnDeleteCacheCompleteCallbackInternal onDeleteCacheCompleteCallbackInternal = new OnDeleteCacheCompleteCallbackInternal(PlayerDataStorageInterface.OnDeleteCacheCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onDeleteCacheCompleteCallbackInternal, Array.Empty<Delegate>());
			Result result = Bindings.EOS_PlayerDataStorage_DeleteCache(base.InnerHandle, ref deleteCacheOptionsInternal, zero, onDeleteCacheCompleteCallbackInternal);
			Helper.Dispose<DeleteCacheOptionsInternal>(ref deleteCacheOptionsInternal);
			return result;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0001EF30 File Offset: 0x0001D130
		public void DeleteFile(ref DeleteFileOptions deleteOptions, object clientData, OnDeleteFileCompleteCallback completionCallback)
		{
			DeleteFileOptionsInternal deleteFileOptionsInternal = default(DeleteFileOptionsInternal);
			deleteFileOptionsInternal.Set(ref deleteOptions);
			IntPtr zero = IntPtr.Zero;
			OnDeleteFileCompleteCallbackInternal onDeleteFileCompleteCallbackInternal = new OnDeleteFileCompleteCallbackInternal(PlayerDataStorageInterface.OnDeleteFileCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onDeleteFileCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_PlayerDataStorage_DeleteFile(base.InnerHandle, ref deleteFileOptionsInternal, zero, onDeleteFileCompleteCallbackInternal);
			Helper.Dispose<DeleteFileOptionsInternal>(ref deleteFileOptionsInternal);
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0001EF8C File Offset: 0x0001D18C
		public void DuplicateFile(ref DuplicateFileOptions duplicateOptions, object clientData, OnDuplicateFileCompleteCallback completionCallback)
		{
			DuplicateFileOptionsInternal duplicateFileOptionsInternal = default(DuplicateFileOptionsInternal);
			duplicateFileOptionsInternal.Set(ref duplicateOptions);
			IntPtr zero = IntPtr.Zero;
			OnDuplicateFileCompleteCallbackInternal onDuplicateFileCompleteCallbackInternal = new OnDuplicateFileCompleteCallbackInternal(PlayerDataStorageInterface.OnDuplicateFileCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onDuplicateFileCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_PlayerDataStorage_DuplicateFile(base.InnerHandle, ref duplicateFileOptionsInternal, zero, onDuplicateFileCompleteCallbackInternal);
			Helper.Dispose<DuplicateFileOptionsInternal>(ref duplicateFileOptionsInternal);
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0001EFE8 File Offset: 0x0001D1E8
		public Result GetFileMetadataCount(ref GetFileMetadataCountOptions getFileMetadataCountOptions, out int outFileMetadataCount)
		{
			GetFileMetadataCountOptionsInternal getFileMetadataCountOptionsInternal = default(GetFileMetadataCountOptionsInternal);
			getFileMetadataCountOptionsInternal.Set(ref getFileMetadataCountOptions);
			outFileMetadataCount = Helper.GetDefault<int>();
			Result result = Bindings.EOS_PlayerDataStorage_GetFileMetadataCount(base.InnerHandle, ref getFileMetadataCountOptionsInternal, ref outFileMetadataCount);
			Helper.Dispose<GetFileMetadataCountOptionsInternal>(ref getFileMetadataCountOptionsInternal);
			return result;
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0001F02C File Offset: 0x0001D22C
		public void QueryFile(ref QueryFileOptions queryFileOptions, object clientData, OnQueryFileCompleteCallback completionCallback)
		{
			QueryFileOptionsInternal queryFileOptionsInternal = default(QueryFileOptionsInternal);
			queryFileOptionsInternal.Set(ref queryFileOptions);
			IntPtr zero = IntPtr.Zero;
			OnQueryFileCompleteCallbackInternal onQueryFileCompleteCallbackInternal = new OnQueryFileCompleteCallbackInternal(PlayerDataStorageInterface.OnQueryFileCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onQueryFileCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_PlayerDataStorage_QueryFile(base.InnerHandle, ref queryFileOptionsInternal, zero, onQueryFileCompleteCallbackInternal);
			Helper.Dispose<QueryFileOptionsInternal>(ref queryFileOptionsInternal);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0001F088 File Offset: 0x0001D288
		public void QueryFileList(ref QueryFileListOptions queryFileListOptions, object clientData, OnQueryFileListCompleteCallback completionCallback)
		{
			QueryFileListOptionsInternal queryFileListOptionsInternal = default(QueryFileListOptionsInternal);
			queryFileListOptionsInternal.Set(ref queryFileListOptions);
			IntPtr zero = IntPtr.Zero;
			OnQueryFileListCompleteCallbackInternal onQueryFileListCompleteCallbackInternal = new OnQueryFileListCompleteCallbackInternal(PlayerDataStorageInterface.OnQueryFileListCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onQueryFileListCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_PlayerDataStorage_QueryFileList(base.InnerHandle, ref queryFileListOptionsInternal, zero, onQueryFileListCompleteCallbackInternal);
			Helper.Dispose<QueryFileListOptionsInternal>(ref queryFileListOptionsInternal);
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0001F0E4 File Offset: 0x0001D2E4
		public PlayerDataStorageFileTransferRequest ReadFile(ref ReadFileOptions readOptions, object clientData, OnReadFileCompleteCallback completionCallback)
		{
			ReadFileOptionsInternal readFileOptionsInternal = default(ReadFileOptionsInternal);
			readFileOptionsInternal.Set(ref readOptions);
			IntPtr zero = IntPtr.Zero;
			OnReadFileCompleteCallbackInternal onReadFileCompleteCallbackInternal = new OnReadFileCompleteCallbackInternal(PlayerDataStorageInterface.OnReadFileCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onReadFileCompleteCallbackInternal, new Delegate[]
			{
				readOptions.ReadFileDataCallback,
				ReadFileOptionsInternal.ReadFileDataCallback,
				readOptions.FileTransferProgressCallback,
				ReadFileOptionsInternal.FileTransferProgressCallback
			});
			IntPtr from = Bindings.EOS_PlayerDataStorage_ReadFile(base.InnerHandle, ref readFileOptionsInternal, zero, onReadFileCompleteCallbackInternal);
			Helper.Dispose<ReadFileOptionsInternal>(ref readFileOptionsInternal);
			PlayerDataStorageFileTransferRequest result;
			Helper.Get<PlayerDataStorageFileTransferRequest>(from, out result);
			return result;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0001F174 File Offset: 0x0001D374
		public PlayerDataStorageFileTransferRequest WriteFile(ref WriteFileOptions writeOptions, object clientData, OnWriteFileCompleteCallback completionCallback)
		{
			WriteFileOptionsInternal writeFileOptionsInternal = default(WriteFileOptionsInternal);
			writeFileOptionsInternal.Set(ref writeOptions);
			IntPtr zero = IntPtr.Zero;
			OnWriteFileCompleteCallbackInternal onWriteFileCompleteCallbackInternal = new OnWriteFileCompleteCallbackInternal(PlayerDataStorageInterface.OnWriteFileCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionCallback, onWriteFileCompleteCallbackInternal, new Delegate[]
			{
				writeOptions.WriteFileDataCallback,
				WriteFileOptionsInternal.WriteFileDataCallback,
				writeOptions.FileTransferProgressCallback,
				WriteFileOptionsInternal.FileTransferProgressCallback
			});
			IntPtr from = Bindings.EOS_PlayerDataStorage_WriteFile(base.InnerHandle, ref writeFileOptionsInternal, zero, onWriteFileCompleteCallbackInternal);
			Helper.Dispose<WriteFileOptionsInternal>(ref writeFileOptionsInternal);
			PlayerDataStorageFileTransferRequest result;
			Helper.Get<PlayerDataStorageFileTransferRequest>(from, out result);
			return result;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0001F204 File Offset: 0x0001D404
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

		// Token: 0x0600157B RID: 5499 RVA: 0x0001F22C File Offset: 0x0001D42C
		[MonoPInvokeCallback(typeof(OnDeleteFileCompleteCallbackInternal))]
		internal static void OnDeleteFileCompleteCallbackInternalImplementation(ref DeleteFileCallbackInfoInternal data)
		{
			OnDeleteFileCompleteCallback onDeleteFileCompleteCallback;
			DeleteFileCallbackInfo deleteFileCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DeleteFileCallbackInfoInternal, OnDeleteFileCompleteCallback, DeleteFileCallbackInfo>(ref data, out onDeleteFileCompleteCallback, out deleteFileCallbackInfo);
			if (flag)
			{
				onDeleteFileCompleteCallback(ref deleteFileCallbackInfo);
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0001F254 File Offset: 0x0001D454
		[MonoPInvokeCallback(typeof(OnDuplicateFileCompleteCallbackInternal))]
		internal static void OnDuplicateFileCompleteCallbackInternalImplementation(ref DuplicateFileCallbackInfoInternal data)
		{
			OnDuplicateFileCompleteCallback onDuplicateFileCompleteCallback;
			DuplicateFileCallbackInfo duplicateFileCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<DuplicateFileCallbackInfoInternal, OnDuplicateFileCompleteCallback, DuplicateFileCallbackInfo>(ref data, out onDuplicateFileCompleteCallback, out duplicateFileCallbackInfo);
			if (flag)
			{
				onDuplicateFileCompleteCallback(ref duplicateFileCallbackInfo);
			}
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0001F27C File Offset: 0x0001D47C
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

		// Token: 0x0600157E RID: 5502 RVA: 0x0001F2A4 File Offset: 0x0001D4A4
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

		// Token: 0x0600157F RID: 5503 RVA: 0x0001F2CC File Offset: 0x0001D4CC
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

		// Token: 0x06001580 RID: 5504 RVA: 0x0001F2F4 File Offset: 0x0001D4F4
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

		// Token: 0x06001581 RID: 5505 RVA: 0x0001F31C File Offset: 0x0001D51C
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

		// Token: 0x06001582 RID: 5506 RVA: 0x0001F354 File Offset: 0x0001D554
		[MonoPInvokeCallback(typeof(OnWriteFileCompleteCallbackInternal))]
		internal static void OnWriteFileCompleteCallbackInternalImplementation(ref WriteFileCallbackInfoInternal data)
		{
			OnWriteFileCompleteCallback onWriteFileCompleteCallback;
			WriteFileCallbackInfo writeFileCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<WriteFileCallbackInfoInternal, OnWriteFileCompleteCallback, WriteFileCallbackInfo>(ref data, out onWriteFileCompleteCallback, out writeFileCallbackInfo);
			if (flag)
			{
				onWriteFileCompleteCallback(ref writeFileCallbackInfo);
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0001F37C File Offset: 0x0001D57C
		[MonoPInvokeCallback(typeof(OnWriteFileDataCallbackInternal))]
		internal static WriteResult OnWriteFileDataCallbackInternalImplementation(ref WriteFileDataCallbackInfoInternal data, IntPtr outDataBuffer, ref uint outDataWritten)
		{
			OnWriteFileDataCallback onWriteFileDataCallback;
			WriteFileDataCallbackInfo writeFileDataCallbackInfo;
			bool flag = Helper.TryGetStructCallback<WriteFileDataCallbackInfoInternal, OnWriteFileDataCallback, WriteFileDataCallbackInfo>(ref data, out onWriteFileDataCallback, out writeFileDataCallbackInfo);
			WriteResult result;
			if (flag)
			{
				ArraySegment<byte> from;
				WriteResult writeResult = onWriteFileDataCallback(ref writeFileDataCallbackInfo, out from);
				Helper.Get<byte>(from, out outDataWritten);
				Helper.Copy(from, outDataBuffer);
				result = writeResult;
			}
			else
			{
				result = Helper.GetDefault<WriteResult>();
			}
			return result;
		}

		// Token: 0x04000943 RID: 2371
		public const int CopyfilemetadataatindexApiLatest = 1;

		// Token: 0x04000944 RID: 2372
		public const int CopyfilemetadataatindexoptionsApiLatest = 1;

		// Token: 0x04000945 RID: 2373
		public const int CopyfilemetadatabyfilenameApiLatest = 1;

		// Token: 0x04000946 RID: 2374
		public const int CopyfilemetadatabyfilenameoptionsApiLatest = 1;

		// Token: 0x04000947 RID: 2375
		public const int DeletecacheApiLatest = 1;

		// Token: 0x04000948 RID: 2376
		public const int DeletecacheoptionsApiLatest = 1;

		// Token: 0x04000949 RID: 2377
		public const int DeletefileApiLatest = 1;

		// Token: 0x0400094A RID: 2378
		public const int DeletefileoptionsApiLatest = 1;

		// Token: 0x0400094B RID: 2379
		public const int DuplicatefileApiLatest = 1;

		// Token: 0x0400094C RID: 2380
		public const int DuplicatefileoptionsApiLatest = 1;

		// Token: 0x0400094D RID: 2381
		public const int FilemetadataApiLatest = 3;

		// Token: 0x0400094E RID: 2382
		public const int FilenameMaxLengthBytes = 64;

		// Token: 0x0400094F RID: 2383
		public const int GetfilemetadatacountApiLatest = 1;

		// Token: 0x04000950 RID: 2384
		public const int GetfilemetadatacountoptionsApiLatest = 1;

		// Token: 0x04000951 RID: 2385
		public const int QueryfileApiLatest = 1;

		// Token: 0x04000952 RID: 2386
		public const int QueryfilelistApiLatest = 2;

		// Token: 0x04000953 RID: 2387
		public const int QueryfilelistoptionsApiLatest = 2;

		// Token: 0x04000954 RID: 2388
		public const int QueryfileoptionsApiLatest = 1;

		// Token: 0x04000955 RID: 2389
		public const int ReadfileApiLatest = 2;

		// Token: 0x04000956 RID: 2390
		public const int ReadfileoptionsApiLatest = 2;

		// Token: 0x04000957 RID: 2391
		public const int TimeUndefined = -1;

		// Token: 0x04000958 RID: 2392
		public const int WritefileApiLatest = 2;

		// Token: 0x04000959 RID: 2393
		public const int WritefileoptionsApiLatest = 2;
	}
}
