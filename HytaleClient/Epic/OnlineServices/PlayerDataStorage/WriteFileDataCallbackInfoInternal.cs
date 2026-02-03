using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000329 RID: 809
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct WriteFileDataCallbackInfoInternal : ICallbackInfoInternal, IGettable<WriteFileDataCallbackInfo>, ISettable<WriteFileDataCallbackInfo>, IDisposable
	{
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001626 RID: 5670 RVA: 0x00020400 File Offset: 0x0001E600
		// (set) Token: 0x06001627 RID: 5671 RVA: 0x00020421 File Offset: 0x0001E621
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

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001628 RID: 5672 RVA: 0x00020434 File Offset: 0x0001E634
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x0002044C File Offset: 0x0001E64C
		// (set) Token: 0x0600162A RID: 5674 RVA: 0x0002046D File Offset: 0x0001E66D
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x00020480 File Offset: 0x0001E680
		// (set) Token: 0x0600162C RID: 5676 RVA: 0x000204A1 File Offset: 0x0001E6A1
		public Utf8String Filename
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Filename, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x000204B4 File Offset: 0x0001E6B4
		// (set) Token: 0x0600162E RID: 5678 RVA: 0x000204CC File Offset: 0x0001E6CC
		public uint DataBufferLengthBytes
		{
			get
			{
				return this.m_DataBufferLengthBytes;
			}
			set
			{
				this.m_DataBufferLengthBytes = value;
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000204D6 File Offset: 0x0001E6D6
		public void Set(ref WriteFileDataCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.DataBufferLengthBytes = other.DataBufferLengthBytes;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00020510 File Offset: 0x0001E710
		public void Set(ref WriteFileDataCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
				this.DataBufferLengthBytes = other.Value.DataBufferLengthBytes;
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0002057E File Offset: 0x0001E77E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000205A5 File Offset: 0x0001E7A5
		public void Get(out WriteFileDataCallbackInfo output)
		{
			output = default(WriteFileDataCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040009A2 RID: 2466
		private IntPtr m_ClientData;

		// Token: 0x040009A3 RID: 2467
		private IntPtr m_LocalUserId;

		// Token: 0x040009A4 RID: 2468
		private IntPtr m_Filename;

		// Token: 0x040009A5 RID: 2469
		private uint m_DataBufferLengthBytes;
	}
}
