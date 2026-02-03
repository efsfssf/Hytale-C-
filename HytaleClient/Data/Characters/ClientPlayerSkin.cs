using System;

namespace HytaleClient.Data.Characters
{
	// Token: 0x02000B5A RID: 2906
	public class ClientPlayerSkin
	{
		// Token: 0x060059DA RID: 23002 RVA: 0x001BD5C7 File Offset: 0x001BB7C7
		public ClientPlayerSkin()
		{
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x001BD5D4 File Offset: 0x001BB7D4
		public ClientPlayerSkin(ClientPlayerSkin other)
		{
			this.BodyType = other.BodyType;
			this.SkinTone = other.SkinTone;
			this.Eyes = other.Eyes;
			this.FacialHair = other.FacialHair;
			this.Haircut = other.Haircut;
			this.Eyebrows = other.Eyebrows;
			this.Face = other.Face;
			this.Pants = other.Pants;
			this.Overpants = other.Overpants;
			this.Undertop = other.Undertop;
			this.Overtop = other.Overtop;
			this.Shoes = other.Shoes;
			this.HeadAccessory = other.HeadAccessory;
			this.FaceAccessory = other.FaceAccessory;
			this.EarAccessory = other.EarAccessory;
			this.SkinFeature = other.SkinFeature;
			this.Gloves = other.Gloves;
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x001BD6B8 File Offset: 0x001BB8B8
		protected bool Equals(ClientPlayerSkin other)
		{
			return this.BodyType == other.BodyType && string.Equals(this.SkinTone, other.SkinTone) && CharacterPartId.Equals(this.Eyes, other.Eyes) && CharacterPartId.Equals(this.FacialHair, other.FacialHair) && CharacterPartId.Equals(this.Haircut, other.Haircut) && CharacterPartId.Equals(this.Eyebrows, other.Eyebrows) && object.Equals(this.Face, other.Face) && CharacterPartId.Equals(this.Pants, other.Pants) && CharacterPartId.Equals(this.Overpants, other.Overpants) && CharacterPartId.Equals(this.Undertop, other.Undertop) && CharacterPartId.Equals(this.Overtop, other.Overtop) && CharacterPartId.Equals(this.Shoes, other.Shoes) && CharacterPartId.Equals(this.HeadAccessory, other.HeadAccessory) && CharacterPartId.Equals(this.FaceAccessory, other.FaceAccessory) && CharacterPartId.Equals(this.EarAccessory, other.EarAccessory) && CharacterPartId.Equals(this.SkinFeature, other.SkinFeature) && CharacterPartId.Equals(this.Gloves, other.Gloves);
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x001BD828 File Offset: 0x001BBA28
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
					result = (!flag3 && this.Equals((ClientPlayerSkin)obj));
				}
			}
			return result;
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x001BD878 File Offset: 0x001BBA78
		public override int GetHashCode()
		{
			int num = (int)this.BodyType;
			num = (num * 397 ^ ((this.SkinTone != null) ? this.SkinTone.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Eyes != null) ? this.Eyes.GetHashCode() : 0));
			num = (num * 397 ^ ((this.FacialHair != null) ? this.FacialHair.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Haircut != null) ? this.Haircut.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Eyebrows != null) ? this.Eyebrows.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Face != null) ? this.Face.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Pants != null) ? this.Pants.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Overpants != null) ? this.Overpants.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Undertop != null) ? this.Undertop.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Overtop != null) ? this.Overtop.GetHashCode() : 0));
			num = (num * 397 ^ ((this.Shoes != null) ? this.Shoes.GetHashCode() : 0));
			num = (num * 397 ^ ((this.HeadAccessory != null) ? this.HeadAccessory.GetHashCode() : 0));
			num = (num * 397 ^ ((this.FaceAccessory != null) ? this.FaceAccessory.GetHashCode() : 0));
			num = (num * 397 ^ ((this.EarAccessory != null) ? this.EarAccessory.GetHashCode() : 0));
			num = (num * 397 ^ ((this.SkinFeature != null) ? this.SkinFeature.GetHashCode() : 0));
			return num * 397 ^ ((this.Gloves != null) ? this.Gloves.GetHashCode() : 0);
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x001BDA84 File Offset: 0x001BBC84
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("{0}: {1}, ", "BodyType", this.BodyType),
				"SkinTone: ",
				this.SkinTone,
				", Face: ",
				this.Face,
				", ",
				string.Format("{0}: {1}, ", "Eyes", this.Eyes),
				string.Format("{0}: {1}, ", "FacialHair", this.FacialHair),
				string.Format("{0}: {1}, ", "Haircut", this.Haircut),
				string.Format("{0}: {1}, ", "Eyebrows", this.Eyebrows),
				"Face: ",
				this.Face,
				", ",
				string.Format("{0}: {1}, ", "Pants", this.Pants),
				string.Format("{0}: {1}, ", "Overpants", this.Overpants),
				string.Format("{0}: {1}, ", "Undertop", this.Undertop),
				string.Format("{0}: {1}, ", "Overtop", this.Overtop),
				string.Format("{0}: {1}, ", "Shoes", this.Shoes),
				string.Format("{0}: {1}, ", "HeadAccessory", this.HeadAccessory),
				string.Format("{0}: {1}, ", "FaceAccessory", this.FaceAccessory),
				string.Format("{0}: {1}, ", "EarAccessory", this.EarAccessory),
				string.Format("{0}: {1}, ", "SkinFeature", this.SkinFeature),
				string.Format("{0}: {1}", "Gloves", this.Gloves)
			});
		}

		// Token: 0x040037C6 RID: 14278
		public CharacterBodyType BodyType;

		// Token: 0x040037C7 RID: 14279
		public string SkinTone;

		// Token: 0x040037C8 RID: 14280
		public string Face;

		// Token: 0x040037C9 RID: 14281
		public CharacterPartId Eyes;

		// Token: 0x040037CA RID: 14282
		public CharacterPartId Haircut;

		// Token: 0x040037CB RID: 14283
		public CharacterPartId FacialHair;

		// Token: 0x040037CC RID: 14284
		public CharacterPartId Eyebrows;

		// Token: 0x040037CD RID: 14285
		public CharacterPartId Pants;

		// Token: 0x040037CE RID: 14286
		public CharacterPartId Overpants;

		// Token: 0x040037CF RID: 14287
		public CharacterPartId Undertop;

		// Token: 0x040037D0 RID: 14288
		public CharacterPartId Overtop;

		// Token: 0x040037D1 RID: 14289
		public CharacterPartId Shoes;

		// Token: 0x040037D2 RID: 14290
		public CharacterPartId HeadAccessory;

		// Token: 0x040037D3 RID: 14291
		public CharacterPartId FaceAccessory;

		// Token: 0x040037D4 RID: 14292
		public CharacterPartId EarAccessory;

		// Token: 0x040037D5 RID: 14293
		public CharacterPartId SkinFeature;

		// Token: 0x040037D6 RID: 14294
		public CharacterPartId Gloves;
	}
}
