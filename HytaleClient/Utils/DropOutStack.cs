using System;
using System.Collections;
using System.Collections.Generic;

namespace HytaleClient.Utils
{
	// Token: 0x020007BF RID: 1983
	public class DropOutStack<T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06003364 RID: 13156 RVA: 0x0004F493 File Offset: 0x0004D693
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x0004F49B File Offset: 0x0004D69B
		public DropOutStack(int capacity)
		{
			this._array = new T[capacity];
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x0004F4C0 File Offset: 0x0004D6C0
		public void Push(T item)
		{
			bool flag = this._count < this._array.Length;
			if (flag)
			{
				this._count++;
			}
			this._array[this._top] = item;
			this._top = (this._top + 1) % this._array.Length;
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x0004F51C File Offset: 0x0004D71C
		public T Pop()
		{
			bool flag = this._count == 0;
			T result;
			if (flag)
			{
				result = default(T);
			}
			else
			{
				this._count--;
				this._top = (this._array.Length + this._top - 1) % this._array.Length;
				T t = this._array[this._top];
				this._array[this._top] = default(T);
				result = t;
			}
			return result;
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x0004F5A4 File Offset: 0x0004D7A4
		public T Peek()
		{
			return this._array[(this._array.Length + this._top - 1) % this._array.Length];
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x0004F5DC File Offset: 0x0004D7DC
		public T PeekAt(int index)
		{
			bool flag = this._count == 0;
			T result;
			if (flag)
			{
				result = default(T);
			}
			else
			{
				bool flag2 = this._count < this._array.Length;
				if (flag2)
				{
					result = this._array[index];
				}
				else
				{
					result = this._array[(this._array.Length + this._top + index) % this._array.Length];
				}
			}
			return result;
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x0004F650 File Offset: 0x0004D850
		public void Clear()
		{
			for (int i = 0; i < this._array.Length; i++)
			{
				this._array[i] = default(T);
			}
			this._top = 0;
			this._count = 0;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x0004F69A File Offset: 0x0004D89A
		public IEnumerator<T> GetEnumerator()
		{
			DropOutStack<T>.<GetEnumerator>d__11 <GetEnumerator>d__ = new DropOutStack<T>.<GetEnumerator>d__11(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x0004F6AC File Offset: 0x0004D8AC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04001715 RID: 5909
		private readonly T[] _array;

		// Token: 0x04001716 RID: 5910
		private int _top = 0;

		// Token: 0x04001717 RID: 5911
		private int _count = 0;
	}
}
