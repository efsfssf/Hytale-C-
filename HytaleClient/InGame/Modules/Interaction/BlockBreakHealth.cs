using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000932 RID: 2354
	internal class BlockBreakHealth : Disposable
	{
		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x060047DC RID: 18396 RVA: 0x00110F2B File Offset: 0x0010F12B
		public bool IsEnabled
		{
			get
			{
				return this._gameInstance.App.Settings.BlockHealth;
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x00110F42 File Offset: 0x0010F142
		public BlockBreakHealth(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x00110F6C File Offset: 0x0010F16C
		private Entity SpawnEntity(Vector3 position)
		{
			Entity entity;
			this._gameInstance.EntityStoreModule.Spawn(-1, out entity);
			entity.SetIsTangible(false);
			entity.VisibilityPrediction = true;
			this._gameInstance.AudioModule.TryRegisterSoundObject(Vector3.Zero, Vector3.Zero, ref entity.SoundObjectReference, false);
			entity.SetPosition(new Vector3(position.X + this._offset.X, position.Y + this._offset.Y, position.Z + this._offset.Z));
			entity.PositionProgress = 1f;
			return entity;
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x00111014 File Offset: 0x0010F214
		protected override void DoDispose()
		{
			using (Dictionary<Vector3, Entity>.Enumerator enumerator = this._entities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Vector3, Entity> entry = enumerator.Current;
					this._gameInstance.Engine.RunOnMainThread(this, delegate
					{
						this._gameInstance.EntityStoreModule.Despawn(entry.Value.NetworkId);
					}, true, false);
				}
			}
			this._entities.Clear();
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x001110A4 File Offset: 0x0010F2A4
		public bool NeedsDrawing()
		{
			return this.IsEnabled && this._entities.Count > 0;
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x001110D0 File Offset: 0x0010F2D0
		public void Draw()
		{
			SceneRenderer.SceneData data = this._gameInstance.SceneRenderer.Data;
			float x = data.ViewportSize.X;
			float y = data.ViewportSize.Y;
			foreach (KeyValuePair<Vector3, Entity> keyValuePair in this._entities)
			{
				Entity value = keyValuePair.Value;
				Vector3 position = value.Position;
				Vector2 vector = Vector3.WorldToScreenPos(ref data.ViewProjectionMatrix, x, y, position);
				Matrix matrix;
				Matrix.CreateTranslation(vector.X - x / 2f, -(vector.Y - y / 2f), 0f, out matrix);
				Vector3 position2 = this._gameInstance.CameraModule.Controller.Position;
				float distanceToCamera = Vector3.Distance(value.RenderPosition, position2);
				this._gameInstance.App.Interface.InGameView.RegisterEntityUIDrawTasks(ref matrix, value, distanceToCamera);
			}
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x001111EC File Offset: 0x0010F3EC
		public Entity GetEntity(Vector3 position, int blockId)
		{
			Entity entity;
			this._entities.TryGetValue(position, out entity);
			bool flag = entity == null && blockId > 0;
			if (flag)
			{
				entity = this.SpawnEntity(position);
				this._entities[position] = entity;
			}
			return entity;
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x00111234 File Offset: 0x0010F434
		public void UpdateHealth(int blockId, int worldX, int worldY, int worldZ, float maxBlockHealth, float health)
		{
			Vector3 vector = new Vector3((float)worldX, (float)worldY, (float)worldZ);
			Entity entity = this.GetEntity(vector, blockId);
			bool flag = entity == null;
			if (!flag)
			{
				bool flag2 = health == maxBlockHealth || blockId == -1;
				if (flag2)
				{
					this._gameInstance.EntityStoreModule.Despawn(entity.NetworkId);
					this._entities.Remove(vector);
				}
				else
				{
					entity.SmoothHealth = health;
				}
			}
		}

		// Token: 0x0400242C RID: 9260
		private readonly GameInstance _gameInstance;

		// Token: 0x0400242D RID: 9261
		private static readonly Vector3 baseOffset = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x0400242E RID: 9262
		private readonly Dictionary<Vector3, Entity> _entities = new Dictionary<Vector3, Entity>();

		// Token: 0x0400242F RID: 9263
		private readonly Vector3 _offset = BlockBreakHealth.baseOffset;
	}
}
