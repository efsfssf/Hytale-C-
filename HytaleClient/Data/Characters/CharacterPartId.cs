using System;

namespace HytaleClient.Data.Characters
{
	// Token: 0x02000B55 RID: 2901
	public class CharacterPartId
	{
		// Token: 0x0600599C RID: 22940 RVA: 0x001BABC7 File Offset: 0x001B8DC7
		public CharacterPartId(string partId, string colorId = null)
		{
			this.PartId = partId;
			this.ColorId = colorId;
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x001BABDF File Offset: 0x001B8DDF
		public CharacterPartId(string partId, string variantId, string colorId)
		{
			this.PartId = partId;
			this.ColorId = colorId;
			this.VariantId = variantId;
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x001BAC00 File Offset: 0x001B8E00
		public static CharacterPartId FromString(string id)
		{
			bool flag = id == null;
			CharacterPartId result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string[] array = id.Split(new char[]
				{
					'.'
				});
				result = new CharacterPartId(array[0], (array.Length > 2) ? array[2] : null, (array.Length > 1) ? array[1] : null);
			}
			return result;
		}

		// Token: 0x0600599F RID: 22943 RVA: 0x001BAC50 File Offset: 0x001B8E50
		public static string BuildString(string partId, string variantId, string colorId)
		{
			bool flag = variantId != null;
			string result;
			if (flag)
			{
				result = string.Concat(new string[]
				{
					partId,
					".",
					colorId,
					".",
					variantId
				});
			}
			else
			{
				result = partId + "." + colorId;
			}
			return result;
		}

		// Token: 0x060059A0 RID: 22944 RVA: 0x001BACA1 File Offset: 0x001B8EA1
		public override string ToString()
		{
			return CharacterPartId.BuildString(this.PartId, this.VariantId, this.ColorId);
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x001BACBC File Offset: 0x001B8EBC
		protected bool Equals(CharacterPartId other)
		{
			return this.PartId == other.PartId && this.VariantId == other.VariantId && this.ColorId == other.ColorId;
		}

		// Token: 0x060059A2 RID: 22946 RVA: 0x001BAD08 File Offset: 0x001B8F08
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this == obj;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = obj.GetType() != base.GetType();
					result = (!flag3 && this.Equals((CharacterPartId)obj));
				}
			}
			return result;
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x001BAD58 File Offset: 0x001B8F58
		public override int GetHashCode()
		{
			int num = (this.PartId != null) ? this.PartId.GetHashCode() : 0;
			num = (num * 397 ^ ((this.VariantId != null) ? this.VariantId.GetHashCode() : 0));
			return num * 397 ^ ((this.ColorId != null) ? this.ColorId.GetHashCode() : 0);
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x001BADC4 File Offset: 0x001B8FC4
		public static bool Equals(CharacterPartId id1, CharacterPartId id2)
		{
			bool flag = id1 == id2;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = id1 == null || id2 == null;
				result = (!flag2 && id1.Equals(id2));
			}
			return result;
		}

		// Token: 0x0400378E RID: 14222
		public readonly string PartId;

		// Token: 0x0400378F RID: 14223
		public readonly string VariantId;

		// Token: 0x04003790 RID: 14224
		public readonly string ColorId;
	}
}
