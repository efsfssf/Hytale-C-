using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005D6 RID: 1494
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateDeviceIdOptionsInternal : ISettable<CreateDeviceIdOptions>, IDisposable
	{
		// Token: 0x17000B47 RID: 2887
		// (set) Token: 0x060026D1 RID: 9937 RVA: 0x00039732 File Offset: 0x00037932
		public Utf8String DeviceModel
		{
			set
			{
				Helper.Set(value, ref this.m_DeviceModel);
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00039742 File Offset: 0x00037942
		public void Set(ref CreateDeviceIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.DeviceModel = other.DeviceModel;
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0003975C File Offset: 0x0003795C
		public void Set(ref CreateDeviceIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DeviceModel = other.Value.DeviceModel;
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x00039792 File Offset: 0x00037992
		public void Dispose()
		{
			Helper.Dispose(ref this.m_DeviceModel);
		}

		// Token: 0x040010D9 RID: 4313
		private int m_ApiVersion;

		// Token: 0x040010DA RID: 4314
		private IntPtr m_DeviceModel;
	}
}
