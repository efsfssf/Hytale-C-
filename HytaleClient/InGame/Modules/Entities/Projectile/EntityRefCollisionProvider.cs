using System;
using System.Collections.Generic;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000959 RID: 2393
	internal class EntityRefCollisionProvider
	{
		// Token: 0x06004AD2 RID: 19154 RVA: 0x00131AD8 File Offset: 0x0012FCD8
		public EntityRefCollisionProvider()
		{
			this._contacts = new EntityContactData[4];
			this._sortBuffer = new EntityContactData[4];
			for (int i = 0; i < this._contacts.Length; i++)
			{
				this._contacts[i] = new EntityContactData();
			}
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x00131B43 File Offset: 0x0012FD43
		public EntityContactData GetContact(int i)
		{
			return this._contacts[i];
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x00131B50 File Offset: 0x0012FD50
		public void Clear()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this._contacts[i].Clear();
			}
			this.Count = 0;
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x00131B8C File Offset: 0x0012FD8C
		public float ComputeNearest(GameInstance gameInstance, BoundingBox entityBoundingBox, Vector3 pos, Vector3 dir, Entity ignoreSelf, Entity ignore)
		{
			return this.ComputeNearest(gameInstance, pos, dir, entityBoundingBox, dir.Length() + 8f, new Func<Entity, bool>(EntityRefCollisionProvider.DefaultEntityFilter), ignoreSelf, ignore);
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x00131BC8 File Offset: 0x0012FDC8
		public float ComputeNearest(GameInstance gameInstance, Vector3 pos, Vector3 dir, BoundingBox boundingBox, float radius, Func<Entity, bool> entityFilter, Entity ignoreSelf, Entity ignoreOther)
		{
			this._ignoreSelf = ignoreSelf;
			this._ignoreOther = ignoreOther;
			this._nearestCollisionStart = float.MaxValue;
			this._entityFilter = entityFilter;
			this.IterateEntitiesInSphere(gameInstance, pos, dir, boundingBox, radius, delegate(EntityRefCollisionProvider provider, Entity entity)
			{
				provider.AcceptNearestIgnore(entity);
			});
			bool flag = this.Count == 0;
			if (flag)
			{
				this._nearestCollisionStart = float.MinValue;
			}
			this.ClearRefs();
			this._ignoreSelf = null;
			this._ignoreOther = null;
			return this._nearestCollisionStart;
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x00131C60 File Offset: 0x0012FE60
		protected void IterateEntitiesInSphere(GameInstance gameInstance, Vector3 pos, Vector3 dir, BoundingBox boundingBox, float radius, Action<EntityRefCollisionProvider, Entity> consumer)
		{
			this._position = pos;
			this._direction = dir;
			this._boundingBox = boundingBox;
			List<Entity> entitiesInSphere = gameInstance.EntityStoreModule.GetEntitiesInSphere(pos, radius);
			for (int i = 0; i < entitiesInSphere.Count; i++)
			{
				Entity arg = entitiesInSphere[i];
				consumer(this, arg);
			}
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x00131CBC File Offset: 0x0012FEBC
		protected void SetContact(Entity entity, string detailName)
		{
			this._collisionPosition = this._position + this._direction * this._minMax.X;
			this._contacts[0].Assign(this._collisionPosition, this._minMax.X, this._minMax.Y, entity, detailName);
			this.Count = 1;
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x00131D24 File Offset: 0x0012FF24
		protected bool IsColliding(Entity entity, ref Vector2 minMax, out string hitDetail)
		{
			bool flag = entity.DetailBoundingBoxes.Count > 0;
			bool result;
			if (flag)
			{
				Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.Up, entity.BodyOrientation.Y);
				foreach (KeyValuePair<string, Entity.DetailBoundingBox[]> keyValuePair in entity.DetailBoundingBoxes)
				{
					foreach (Entity.DetailBoundingBox detailBoundingBox in keyValuePair.Value)
					{
						Vector3 vector = detailBoundingBox.Offset;
						Vector3.Transform(ref vector, ref quaternion, out vector);
						vector += entity.NextPosition;
						bool flag2 = CollisionMath.IntersectSweptAABBs(this._position, this._direction, this._boundingBox, vector, detailBoundingBox.Box, ref minMax) && minMax.X <= 1f;
						if (flag2)
						{
							hitDetail = keyValuePair.Key;
							return true;
						}
					}
				}
				hitDetail = null;
				result = false;
			}
			else
			{
				hitDetail = null;
				result = (CollisionMath.IntersectSweptAABBs(this._position, this._direction, this._boundingBox, entity.NextPosition, entity.Hitbox, ref minMax) && minMax.X <= 1f);
			}
			return result;
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x00131E90 File Offset: 0x00130090
		protected void ClearRefs()
		{
			this._position = Vector3.Zero;
			this._direction = Vector3.Zero;
			this._entityFilter = null;
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00131EB0 File Offset: 0x001300B0
		public static bool DefaultEntityFilter(Entity entity)
		{
			bool flag = entity.IsDead(true);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool predictable = entity.Predictable;
				if (predictable)
				{
					result = false;
				}
				else
				{
					bool flag2 = !entity.IsTangible();
					result = !flag2;
				}
			}
			return result;
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00131EF4 File Offset: 0x001300F4
		protected void AcceptNearestIgnore(Entity entity)
		{
			string detailName;
			bool flag = this._entityFilter(entity) && !entity.Equals(this._ignoreSelf) && !entity.Equals(this._ignoreOther) && this.IsColliding(entity, ref this._minMax, out detailName);
			if (flag)
			{
				bool flag2 = this._minMax.X < this._nearestCollisionStart;
				if (flag2)
				{
					this._nearestCollisionStart = this._minMax.X;
					this.SetContact(entity, detailName);
				}
			}
		}

		// Token: 0x0400267E RID: 9854
		protected const int AllocSize = 4;

		// Token: 0x0400267F RID: 9855
		protected const float ExtraDistance = 8f;

		// Token: 0x04002680 RID: 9856
		protected EntityContactData[] _contacts;

		// Token: 0x04002681 RID: 9857
		protected EntityContactData[] _sortBuffer;

		// Token: 0x04002682 RID: 9858
		public int Count;

		// Token: 0x04002683 RID: 9859
		protected Vector2 _minMax = default(Vector2);

		// Token: 0x04002684 RID: 9860
		protected Vector3 _collisionPosition = default(Vector3);

		// Token: 0x04002685 RID: 9861
		protected float _nearestCollisionStart;

		// Token: 0x04002686 RID: 9862
		protected Vector3 _position;

		// Token: 0x04002687 RID: 9863
		protected Vector3 _direction;

		// Token: 0x04002688 RID: 9864
		protected BoundingBox _boundingBox;

		// Token: 0x04002689 RID: 9865
		protected Func<Entity, bool> _entityFilter;

		// Token: 0x0400268A RID: 9866
		protected Entity _ignoreSelf;

		// Token: 0x0400268B RID: 9867
		protected Entity _ignoreOther;
	}
}
