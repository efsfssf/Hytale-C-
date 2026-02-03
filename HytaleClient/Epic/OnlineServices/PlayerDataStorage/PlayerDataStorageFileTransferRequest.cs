using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000315 RID: 789
	public sealed class PlayerDataStorageFileTransferRequest : Handle
	{
		// Token: 0x06001568 RID: 5480 RVA: 0x0001ED51 File Offset: 0x0001CF51
		public PlayerDataStorageFileTransferRequest()
		{
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0001ED5B File Offset: 0x0001CF5B
		public PlayerDataStorageFileTransferRequest(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0001ED68 File Offset: 0x0001CF68
		public Result CancelRequest()
		{
			return Bindings.EOS_PlayerDataStorageFileTransferRequest_CancelRequest(base.InnerHandle);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0001ED88 File Offset: 0x0001CF88
		public Result GetFileRequestState()
		{
			return Bindings.EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState(base.InnerHandle);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0001EDA8 File Offset: 0x0001CFA8
		public Result GetFilename(out Utf8String outStringBuffer)
		{
			int num = 64;
			IntPtr intPtr = Helper.AddAllocation(num);
			Result result = Bindings.EOS_PlayerDataStorageFileTransferRequest_GetFilename(base.InnerHandle, (uint)num, intPtr, ref num);
			Helper.Get(intPtr, out outStringBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0001EDE5 File Offset: 0x0001CFE5
		public void Release()
		{
			Bindings.EOS_PlayerDataStorageFileTransferRequest_Release(base.InnerHandle);
		}
	}
}
