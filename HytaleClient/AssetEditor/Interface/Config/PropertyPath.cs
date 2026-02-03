using System;
using System.Diagnostics;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BCB RID: 3019
	public struct PropertyPath : IEquatable<PropertyPath>
	{
		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x06005EFE RID: 24318 RVA: 0x001E96F5 File Offset: 0x001E78F5
		public int ElementCount
		{
			get
			{
				return this.Elements.Length;
			}
		}

		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x06005EFF RID: 24319 RVA: 0x001E96FF File Offset: 0x001E78FF
		public string LastElement
		{
			get
			{
				return this.Elements[this.Elements.Length - 1];
			}
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x001E9712 File Offset: 0x001E7912
		private PropertyPath(string[] elements, int length)
		{
			this.Elements = elements;
			this._stringLength = length;
		}

		// Token: 0x06005F01 RID: 24321 RVA: 0x001E9724 File Offset: 0x001E7924
		private PropertyPath(string[] elements)
		{
			this.Elements = elements;
			int num = (elements.Length != 0) ? (elements.Length - 1) : 0;
			foreach (string text in elements)
			{
				num += text.Length;
			}
			this._stringLength = num;
		}

		// Token: 0x06005F02 RID: 24322 RVA: 0x001E976C File Offset: 0x001E796C
		public PropertyPath GetParent()
		{
			string[] array = new string[this.Elements.Length - 1];
			Array.Copy(this.Elements, 0, array, 0, array.Length);
			int num = this._stringLength - this.Elements[this.Elements.Length - 1].Length;
			bool flag = array.Length != 0;
			if (flag)
			{
				num--;
			}
			return new PropertyPath(array, num);
		}

		// Token: 0x06005F03 RID: 24323 RVA: 0x001E97D4 File Offset: 0x001E79D4
		public PropertyPath GetChild(string key)
		{
			Debug.Assert(!key.Contains("."));
			string[] array = new string[this.Elements.Length + 1];
			Array.Copy(this.Elements, 0, array, 0, this.Elements.Length);
			array[this.Elements.Length] = key;
			int num = this._stringLength + key.Length;
			bool flag = array.Length > 1;
			if (flag)
			{
				num++;
			}
			return new PropertyPath(array, num);
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x001E9854 File Offset: 0x001E7A54
		public bool IsDescendantOf(PropertyPath ancestor)
		{
			bool flag = ancestor._stringLength >= this._stringLength || ancestor.ElementCount >= this.ElementCount;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < ancestor.Elements.Length; i++)
				{
					bool flag2 = this.Elements[i] != ancestor.Elements[i];
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x001E98CC File Offset: 0x001E7ACC
		public bool Equals(PropertyPath other)
		{
			bool flag = this._stringLength != other._stringLength;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.Elements.Length != other.Elements.Length;
				if (flag2)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < this.Elements.Length; i++)
					{
						bool flag3 = this.Elements[i] != other.Elements[i];
						if (flag3)
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x001E9950 File Offset: 0x001E7B50
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is PropertyPath)
			{
				PropertyPath other = (PropertyPath)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x001E997C File Offset: 0x001E7B7C
		public override int GetHashCode()
		{
			int num = 17;
			foreach (string text in this.Elements)
			{
				num = num * 23 + ((text != null) ? text.GetHashCode() : 0);
			}
			return num;
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x001E99C3 File Offset: 0x001E7BC3
		public override string ToString()
		{
			return string.Join(".", this.Elements);
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x001E99D8 File Offset: 0x001E7BD8
		public static PropertyPath FromString(string path)
		{
			return new PropertyPath(path.Split(new char[]
			{
				'.'
			}, StringSplitOptions.RemoveEmptyEntries));
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x001E9A04 File Offset: 0x001E7C04
		public static PropertyPath FromElements(string[] pathElements)
		{
			return new PropertyPath(pathElements);
		}

		// Token: 0x04003B35 RID: 15157
		public static readonly PropertyPath Root = new PropertyPath(new string[0], 0);

		// Token: 0x04003B36 RID: 15158
		public readonly string[] Elements;

		// Token: 0x04003B37 RID: 15159
		private readonly int _stringLength;
	}
}
