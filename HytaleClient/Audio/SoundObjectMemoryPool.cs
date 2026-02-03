using System;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Audio
{
	// Token: 0x02000B85 RID: 2949
	internal class SoundObjectMemoryPool
	{
		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x001C4FB5 File Offset: 0x001C31B5
		public SoundObjectBuffers SoundObjects
		{
			get
			{
				return this._soundObjects;
			}
		}

		// Token: 0x06005AD0 RID: 23248 RVA: 0x001C4FBD File Offset: 0x001C31BD
		public void Initialize()
		{
			this._soundObjectMaxCount = 2048;
			this._memoryPoolHelper = new MemoryPoolHelper(this._soundObjectMaxCount);
			this._soundObjects = new SoundObjectBuffers(this._soundObjectMaxCount);
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x001C4FED File Offset: 0x001C31ED
		public void Release()
		{
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x001C4FF0 File Offset: 0x001C31F0
		public int TakeSlot()
		{
			int num = this._memoryPoolHelper.ThreadSafeTakeMemorySlot(1);
			bool flag = num < 0;
			if (flag)
			{
				SoundObjectMemoryPool.Logger.Warn("Could not find a free slot for sound object!");
			}
			return num;
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x001C5028 File Offset: 0x001C3228
		public void ReleaseSlot(int slot)
		{
			this._memoryPoolHelper.ThreadSafeReleaseMemorySlot(slot, 1);
		}

		// Token: 0x040038D8 RID: 14552
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040038D9 RID: 14553
		private const int SoundObjectMaxCount = 2048;

		// Token: 0x040038DA RID: 14554
		private int _soundObjectMaxCount;

		// Token: 0x040038DB RID: 14555
		private SoundObjectBuffers _soundObjects;

		// Token: 0x040038DC RID: 14556
		private MemoryPoolHelper _memoryPoolHelper;
	}
}
