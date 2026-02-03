using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200074D RID: 1869
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryDefinitionsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryDefinitionsCompleteCallbackInfo>, ISettable<OnQueryDefinitionsCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x00048190 File Offset: 0x00046390
		// (set) Token: 0x06003085 RID: 12421 RVA: 0x000481A8 File Offset: 0x000463A8
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000481B4 File Offset: 0x000463B4
		// (set) Token: 0x06003087 RID: 12423 RVA: 0x000481D5 File Offset: 0x000463D5
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

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000481E8 File Offset: 0x000463E8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x00048200 File Offset: 0x00046400
		public void Set(ref OnQueryDefinitionsCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x00048220 File Offset: 0x00046420
		public void Set(ref OnQueryDefinitionsCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x00048264 File Offset: 0x00046464
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x00048273 File Offset: 0x00046473
		public void Get(out OnQueryDefinitionsCompleteCallbackInfo output)
		{
			output = default(OnQueryDefinitionsCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040015AC RID: 5548
		private Result m_ResultCode;

		// Token: 0x040015AD RID: 5549
		private IntPtr m_ClientData;
	}
}
