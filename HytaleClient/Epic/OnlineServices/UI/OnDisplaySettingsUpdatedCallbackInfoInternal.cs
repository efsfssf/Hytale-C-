using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000069 RID: 105
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnDisplaySettingsUpdatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnDisplaySettingsUpdatedCallbackInfo>, ISettable<OnDisplaySettingsUpdatedCallbackInfo>, IDisposable
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00007080 File Offset: 0x00005280
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x000070A1 File Offset: 0x000052A1
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x000070B4 File Offset: 0x000052B4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x000070CC File Offset: 0x000052CC
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x000070ED File Offset: 0x000052ED
		public bool IsVisible
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsVisible, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsVisible);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00007100 File Offset: 0x00005300
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00007121 File Offset: 0x00005321
		public bool IsExclusiveInput
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsExclusiveInput, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsExclusiveInput);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00007131 File Offset: 0x00005331
		public void Set(ref OnDisplaySettingsUpdatedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.IsVisible = other.IsVisible;
			this.IsExclusiveInput = other.IsExclusiveInput;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000715C File Offset: 0x0000535C
		public void Set(ref OnDisplaySettingsUpdatedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.IsVisible = other.Value.IsVisible;
				this.IsExclusiveInput = other.Value.IsExclusiveInput;
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000071B5 File Offset: 0x000053B5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000071C4 File Offset: 0x000053C4
		public void Get(out OnDisplaySettingsUpdatedCallbackInfo output)
		{
			output = default(OnDisplaySettingsUpdatedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400027E RID: 638
		private IntPtr m_ClientData;

		// Token: 0x0400027F RID: 639
		private int m_IsVisible;

		// Token: 0x04000280 RID: 640
		private int m_IsExclusiveInput;
	}
}
