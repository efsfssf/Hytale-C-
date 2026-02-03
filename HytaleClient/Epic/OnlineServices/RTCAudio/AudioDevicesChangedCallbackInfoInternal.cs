using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000209 RID: 521
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioDevicesChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioDevicesChangedCallbackInfo>, ISettable<AudioDevicesChangedCallbackInfo>, IDisposable
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x00016208 File Offset: 0x00014408
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x00016229 File Offset: 0x00014429
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0001623C File Offset: 0x0001443C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00016254 File Offset: 0x00014454
		public void Set(ref AudioDevicesChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00016264 File Offset: 0x00014464
		public void Set(ref AudioDevicesChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00016293 File Offset: 0x00014493
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x000162A2 File Offset: 0x000144A2
		public void Get(out AudioDevicesChangedCallbackInfo output)
		{
			output = default(AudioDevicesChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040006CE RID: 1742
		private IntPtr m_ClientData;
	}
}
