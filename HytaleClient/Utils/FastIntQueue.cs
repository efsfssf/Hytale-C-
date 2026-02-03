using System;

namespace HytaleClient.Utils
{
	// Token: 0x020007C1 RID: 1985
	public class FastIntQueue
	{
		// Token: 0x0600336E RID: 13166 RVA: 0x0004F753 File Offset: 0x0004D953
		public FastIntQueue(int maxCount)
		{
			this._maxCount = maxCount;
			this._values = new int[this._maxCount];
			this._startIndex = 0;
			this._endIndex = 0;
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x0004F784 File Offset: 0x0004D984
		public void Push(int value)
		{
			this._values[this._endIndex] = value;
			this.Count++;
			this._endIndex++;
			bool flag = this._endIndex >= this._maxCount;
			if (flag)
			{
				this._endIndex = 0;
			}
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x0004F7D8 File Offset: 0x0004D9D8
		public int Pop()
		{
			int result = this._values[this._startIndex];
			this.Count--;
			this._startIndex++;
			bool flag = this._startIndex >= this._maxCount;
			if (flag)
			{
				this._startIndex = 0;
			}
			return result;
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x0004F831 File Offset: 0x0004DA31
		public void Clear()
		{
			this.Count = 0;
		}

		// Token: 0x04001718 RID: 5912
		public int Count;

		// Token: 0x04001719 RID: 5913
		private int _maxCount;

		// Token: 0x0400171A RID: 5914
		private int[] _values;

		// Token: 0x0400171B RID: 5915
		private int _startIndex;

		// Token: 0x0400171C RID: 5916
		private int _endIndex;
	}
}
