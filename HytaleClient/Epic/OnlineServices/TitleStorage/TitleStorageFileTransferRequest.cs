using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000C0 RID: 192
	public sealed class TitleStorageFileTransferRequest : Handle
	{
		// Token: 0x06000723 RID: 1827 RVA: 0x0000A2F8 File Offset: 0x000084F8
		public TitleStorageFileTransferRequest()
		{
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0000A302 File Offset: 0x00008502
		public TitleStorageFileTransferRequest(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0000A310 File Offset: 0x00008510
		public Result CancelRequest()
		{
			return Bindings.EOS_TitleStorageFileTransferRequest_CancelRequest(base.InnerHandle);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0000A330 File Offset: 0x00008530
		public Result GetFileRequestState()
		{
			return Bindings.EOS_TitleStorageFileTransferRequest_GetFileRequestState(base.InnerHandle);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000A350 File Offset: 0x00008550
		public Result GetFilename(out Utf8String outStringBuffer)
		{
			int num = 64;
			IntPtr intPtr = Helper.AddAllocation(num);
			Result result = Bindings.EOS_TitleStorageFileTransferRequest_GetFilename(base.InnerHandle, (uint)num, intPtr, ref num);
			Helper.Get(intPtr, out outStringBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0000A38D File Offset: 0x0000858D
		public void Release()
		{
			Bindings.EOS_TitleStorageFileTransferRequest_Release(base.InnerHandle);
		}
	}
}
