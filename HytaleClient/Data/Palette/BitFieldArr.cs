using System;

namespace HytaleClient.Data.Palette
{
	// Token: 0x02000ADB RID: 2779
	internal class BitFieldArr
	{
		// Token: 0x06005792 RID: 22418 RVA: 0x001A97CC File Offset: 0x001A79CC
		public BitFieldArr(int bits, int length)
		{
			this._bits = bits;
			this._array = new byte[length * bits / 8];
			this._length = length;
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x001A97F4 File Offset: 0x001A79F4
		public int GetLength()
		{
			return this._length;
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x001A980C File Offset: 0x001A7A0C
		public uint Get(int index)
		{
			int num = index * this._bits;
			uint num2 = 0U;
			int i = 0;
			while (i < this._bits)
			{
				num2 |= (uint)((uint)this.GetBit(num) << i);
				i++;
				num++;
			}
			return num2;
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x001A9858 File Offset: 0x001A7A58
		private int GetBit(int bitIndex)
		{
			return this._array[bitIndex / 8] >> bitIndex % 8 & 1;
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x001A9880 File Offset: 0x001A7A80
		public void Set(int index, int value)
		{
			int num = index * this._bits;
			int i = 0;
			while (i < this._bits)
			{
				this.SetBit(num, value >> i & 1);
				i++;
				num++;
			}
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x001A98C4 File Offset: 0x001A7AC4
		private void SetBit(int bitIndex, int bit)
		{
			bool flag = bit == 0;
			if (flag)
			{
				byte[] array = this._array;
				int num = bitIndex / 8;
				array[num] &= (byte)(~(byte)(1 << bitIndex % 8));
			}
			else
			{
				byte[] array2 = this._array;
				int num2 = bitIndex / 8;
				array2[num2] |= (byte)(1 << bitIndex % 8);
			}
		}

		// Token: 0x06005798 RID: 22424 RVA: 0x001A991C File Offset: 0x001A7B1C
		public byte[] Get()
		{
			byte[] array = new byte[this._array.Length];
			Buffer.BlockCopy(this._array, 0, array, 0, this._array.Length);
			return array;
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x001A9954 File Offset: 0x001A7B54
		public void Set(byte[] bytes)
		{
			Buffer.BlockCopy(bytes, 0, this._array, 0, (bytes.Length < this._array.Length) ? bytes.Length : this._array.Length);
		}

		// Token: 0x040035DF RID: 13791
		private readonly int _bits;

		// Token: 0x040035E0 RID: 13792
		private readonly int _length;

		// Token: 0x040035E1 RID: 13793
		private readonly byte[] _array;
	}
}
