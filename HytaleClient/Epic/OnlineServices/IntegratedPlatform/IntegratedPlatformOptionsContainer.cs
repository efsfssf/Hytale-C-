using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004BB RID: 1211
	public sealed class IntegratedPlatformOptionsContainer : Handle
	{
		// Token: 0x06001F83 RID: 8067 RVA: 0x0002E316 File Offset: 0x0002C516
		public IntegratedPlatformOptionsContainer()
		{
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0002E320 File Offset: 0x0002C520
		public IntegratedPlatformOptionsContainer(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0002E32C File Offset: 0x0002C52C
		public Result Add(ref IntegratedPlatformOptionsContainerAddOptions inOptions)
		{
			IntegratedPlatformOptionsContainerAddOptionsInternal integratedPlatformOptionsContainerAddOptionsInternal = default(IntegratedPlatformOptionsContainerAddOptionsInternal);
			integratedPlatformOptionsContainerAddOptionsInternal.Set(ref inOptions);
			Result result = Bindings.EOS_IntegratedPlatformOptionsContainer_Add(base.InnerHandle, ref integratedPlatformOptionsContainerAddOptionsInternal);
			Helper.Dispose<IntegratedPlatformOptionsContainerAddOptionsInternal>(ref integratedPlatformOptionsContainerAddOptionsInternal);
			return result;
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0002E366 File Offset: 0x0002C566
		public void Release()
		{
			Bindings.EOS_IntegratedPlatformOptionsContainer_Release(base.InnerHandle);
		}

		// Token: 0x04000DBC RID: 3516
		public const int IntegratedplatformoptionscontainerAddApiLatest = 1;
	}
}
