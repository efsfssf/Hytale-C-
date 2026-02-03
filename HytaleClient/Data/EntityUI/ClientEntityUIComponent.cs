using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityUI
{
	// Token: 0x02000B03 RID: 2819
	internal abstract class ClientEntityUIComponent
	{
		// Token: 0x06005873 RID: 22643 RVA: 0x001AFDB7 File Offset: 0x001ADFB7
		protected ClientEntityUIComponent()
		{
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x001AFDC1 File Offset: 0x001ADFC1
		protected ClientEntityUIComponent(int id, EntityUIComponent component)
		{
			this.Id = id;
			this.HitboxOffset = component.HitboxOffset;
			this.Unknown = component.Unknown;
		}

		// Token: 0x06005875 RID: 22645
		public abstract ClientEntityUIComponent Clone();

		// Token: 0x06005876 RID: 22646
		public abstract void RegisterDrawTasksForEntity(Entity entity, Matrix transformationMatrix, float distanceToCamera, int entitiesCount, ref EntityUIDrawTask[] drawTasks, ref int drawTasksCount);

		// Token: 0x06005877 RID: 22647
		public abstract void PrepareForDraw(EntityUIDrawTask task);

		// Token: 0x06005878 RID: 22648 RVA: 0x001AFDEC File Offset: 0x001ADFEC
		public Matrix ApplyHitboxOffset(Matrix transformationMatrix)
		{
			bool flag = this.HitboxOffset.X != 0f;
			if (flag)
			{
				transformationMatrix.M41 += this.HitboxOffset.X;
			}
			bool flag2 = this.HitboxOffset.Y != 0f;
			if (flag2)
			{
				transformationMatrix.M42 += this.HitboxOffset.Y;
			}
			return transformationMatrix;
		}

		// Token: 0x040036EB RID: 14059
		public int Id;

		// Token: 0x040036EC RID: 14060
		public Vector2f HitboxOffset;

		// Token: 0x040036ED RID: 14061
		public bool Unknown;
	}
}
