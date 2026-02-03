using System;
using System.Runtime.CompilerServices;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A2 RID: 2466
	internal class FXMemoryPool<T> where T : IFXDataStorage, new()
	{
		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06004F30 RID: 20272 RVA: 0x0016438B File Offset: 0x0016258B
		public ref T Storage
		{
			get
			{
				return ref this._storage;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06004F31 RID: 20273 RVA: 0x00164393 File Offset: 0x00162593
		public int SlotCount
		{
			get
			{
				return this._slotCount;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06004F32 RID: 20274 RVA: 0x0016439B File Offset: 0x0016259B
		public int ItemMaxCount
		{
			get
			{
				return this._itemMaxCount;
			}
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x001643A4 File Offset: 0x001625A4
		public void Initialize(int requestedMaxCount)
		{
			int memorySlotsCount = requestedMaxCount / this.SlotSize;
			this._memoryPoolHelper = new MemoryPoolHelper(memorySlotsCount);
			this._slotCount = this._memoryPoolHelper.MemorySlotCount;
			this._itemMaxCount = this._slotCount * this.SlotSize;
			this._storage = Activator.CreateInstance<T>();
			this._storage.Initialize(this._itemMaxCount);
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x0016440E File Offset: 0x0016260E
		public void Release()
		{
			this._storage.Release();
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x00164424 File Offset: 0x00162624
		public int TakeSlots(int itemCount)
		{
			int slotCount = this.ComputeRequiredItemSlotCount(itemCount);
			int num = this._memoryPoolHelper.TakeMemorySlots(slotCount);
			bool flag = num < 0;
			if (flag)
			{
				FXMemoryPool<T>.Logger.Warn("Could not find consecutive free slots for {0} items!", itemCount);
			}
			return num * this.SlotSize;
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x0016446C File Offset: 0x0016266C
		public void ReleaseSlots(int itemStartIndex, int itemCount)
		{
			bool flag = itemStartIndex % this.SlotSize != 0;
			if (flag)
			{
				throw new Exception(string.Format("Error detected in the item buffer management - invalid 'itemStartIndex' :{0}", itemStartIndex));
			}
			int slot = itemStartIndex / this.SlotSize;
			int slotCount = this.ComputeRequiredItemSlotCount(itemCount);
			this._memoryPoolHelper.ReleaseMemorySlots(slot, slotCount);
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x001644BE File Offset: 0x001626BE
		public void Clear()
		{
			this._memoryPoolHelper.ClearMemorySlots();
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x001644D0 File Offset: 0x001626D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int ComputeRequiredItemSlotCount(int itemCount)
		{
			int num = itemCount / this.SlotSize;
			return num + ((itemCount % this.SlotSize != 0) ? 1 : 0);
		}

		// Token: 0x04002A72 RID: 10866
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002A73 RID: 10867
		public readonly int SlotSize = 32;

		// Token: 0x04002A74 RID: 10868
		private int _itemMaxCount;

		// Token: 0x04002A75 RID: 10869
		private int _slotCount;

		// Token: 0x04002A76 RID: 10870
		private T _storage;

		// Token: 0x04002A77 RID: 10871
		private MemoryPoolHelper _memoryPoolHelper;
	}
}
