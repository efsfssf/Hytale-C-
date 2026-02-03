using System;
using System.Collections.Generic;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Machinima.Actors
{
	// Token: 0x02000928 RID: 2344
	internal class ItemActor : EntityActor
	{
		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06004784 RID: 18308 RVA: 0x0010E4B3 File Offset: 0x0010C6B3
		// (set) Token: 0x06004785 RID: 18309 RVA: 0x0010E4BB File Offset: 0x0010C6BB
		public string ItemId { get; private set; } = "";

		// Token: 0x06004786 RID: 18310 RVA: 0x0010E4C4 File Offset: 0x0010C6C4
		protected override ActorType GetActorType()
		{
			return ActorType.Item;
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0010E4C7 File Offset: 0x0010C6C7
		public ItemActor(GameInstance gameInstance, string name, Entity entity, string itemId = "") : base(gameInstance, name, entity)
		{
			this.ItemId = itemId;
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0010E4E8 File Offset: 0x0010C6E8
		public override void Spawn(GameInstance gameInstance)
		{
			bool flag = base._entity != null;
			if (!flag)
			{
				Vector3 value = new Vector3(0f, 100f, 0f);
				Vector3 bodyOrientation = Vector3.Zero;
				Vector3 lookOrientation = Vector3.Zero;
				SceneTrack track = base.Track;
				bool flag2;
				if (track == null)
				{
					flag2 = false;
				}
				else
				{
					List<TrackKeyframe> keyframes = track.Keyframes;
					int? num = (keyframes != null) ? new int?(keyframes.Count) : null;
					int num2 = 0;
					flag2 = (num.GetValueOrDefault() > num2 & num != null);
				}
				bool flag3 = flag2;
				if (flag3)
				{
					TrackKeyframe trackKeyframe = base.Track.Keyframes[0];
					value = trackKeyframe.GetSetting<Vector3>("Position").Value;
					bodyOrientation = trackKeyframe.GetSetting<Vector3>("Rotation").Value;
					lookOrientation = trackKeyframe.GetSetting<Vector3>("Look").Value;
				}
				Item item = string.IsNullOrWhiteSpace(this.ItemId) ? null : new Item(this.ItemId, 1, 0.0, 0.0, false, new sbyte[0]);
				Entity entity;
				gameInstance.EntityStoreModule.Spawn(-1, out entity);
				entity.SetIsTangible(true);
				entity.SetItem(item);
				entity.SetSpawnTransform(value, bodyOrientation, lookOrientation);
				entity.Scale = base.Scale;
				base._entity = entity;
			}
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0010E63C File Offset: 0x0010C83C
		public void SetItemId(string itemId, GameInstance gameInstance = null)
		{
			this.ItemId = itemId;
			bool flag = gameInstance != null;
			if (flag)
			{
				bool flag2 = base._entity != null;
				if (flag2)
				{
					this.Despawn(gameInstance);
				}
				this.Spawn(gameInstance);
			}
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0010E67C File Offset: 0x0010C87C
		public override SceneActor Clone(GameInstance gameInstance)
		{
			SceneActor sceneActor = new ItemActor(gameInstance, "clone", null, "");
			base.Track.CopyToActor(ref sceneActor);
			ItemActor itemActor = sceneActor as ItemActor;
			itemActor.SetItemId(this.ItemId, null);
			itemActor.SetScale(base.Scale);
			return itemActor;
		}
	}
}
