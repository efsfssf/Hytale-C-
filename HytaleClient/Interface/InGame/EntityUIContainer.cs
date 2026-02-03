using System;
using HytaleClient.Data.EntityUI;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.InGame.EntityUI;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000880 RID: 2176
	internal class EntityUIContainer : Group
	{
		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x0009EE7D File Offset: 0x0009D07D
		private ClientEntityUIComponent[] _components
		{
			get
			{
				return this._inGameView.InGame.Instance.ServerSettings.EntityUIComponents;
			}
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x0009EE99 File Offset: 0x0009D099
		public EntityUIContainer(Desktop desktop, InGameView inGameView) : base(desktop, inGameView)
		{
			this._inGameView = inGameView;
			this.CombatTextUIComponentRenderer = new CombatTextUIComponentRenderer(inGameView);
			this.EntityStatUIComponentRenderer = new EntityStatUIComponentRenderer(inGameView);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x0009EED1 File Offset: 0x0009D0D1
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x0009EEEB File Offset: 0x0009D0EB
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x0009EF05 File Offset: 0x0009D105
		private void Animate(float deltaTime)
		{
			this.CombatTextUIComponentRenderer.Animate(deltaTime);
			this.EntityStatUIComponentRenderer.Animate(deltaTime);
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x0009EF22 File Offset: 0x0009D122
		public void Build()
		{
			base.Clear();
			this.CombatTextUIComponentRenderer.Build(this);
			this.EntityStatUIComponentRenderer.Build(this);
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x0009EF48 File Offset: 0x0009D148
		public void RegisterDrawTasksForEntity(ref Matrix transformationMatrix, Entity entity, float distanceToCamera)
		{
			ArrayUtils.GrowArrayIfNecessary<EntityUIDrawTask>(ref this._drawTasks, this._drawTasksCount, 50);
			int drawTasksCount = this._drawTasksCount;
			foreach (int id in entity.UIComponents)
			{
				ClientEntityUIComponent clientEntityUIComponent;
				bool flag = !entity.TryGetUIComponent(id, out clientEntityUIComponent) || clientEntityUIComponent.Unknown;
				if (!flag)
				{
					clientEntityUIComponent.RegisterDrawTasksForEntity(entity, transformationMatrix, distanceToCamera, this._entitiesCount, ref this._drawTasks, ref this._drawTasksCount);
				}
			}
			bool flag2 = this._drawTasksCount > drawTasksCount;
			if (flag2)
			{
				this._entitiesCount++;
			}
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x0009EFEC File Offset: 0x0009D1EC
		protected override void PrepareForDrawContent()
		{
			for (int i = this._drawTasksCount - 1; i >= 0; i--)
			{
				EntityUIDrawTask entityUIDrawTask = this._drawTasks[i];
				ClientEntityUIComponent clientEntityUIComponent = this._components[entityUIDrawTask.ComponentId];
				this.Desktop.Batcher2D.SetOpacityOverride(entityUIDrawTask.Opacity);
				this.Desktop.Batcher2D.SetTransformationMatrix(entityUIDrawTask.TransformationMatrix);
				clientEntityUIComponent.PrepareForDraw(entityUIDrawTask);
			}
			this._drawTasksCount = 0;
			this._entitiesCount = 0;
			this.Desktop.Batcher2D.SetOpacityOverride(null);
			this.Desktop.Batcher2D.SetTransformationMatrix(Matrix.Identity);
		}

		// Token: 0x04001CA0 RID: 7328
		private readonly InGameView _inGameView;

		// Token: 0x04001CA1 RID: 7329
		private EntityUIDrawTask[] _drawTasks = new EntityUIDrawTask[100];

		// Token: 0x04001CA2 RID: 7330
		private int _drawTasksCount;

		// Token: 0x04001CA3 RID: 7331
		private int _entitiesCount;

		// Token: 0x04001CA4 RID: 7332
		private const int DrawTasksDefaultSize = 100;

		// Token: 0x04001CA5 RID: 7333
		private const int DrawTasksGrowth = 50;

		// Token: 0x04001CA6 RID: 7334
		public CombatTextUIComponentRenderer CombatTextUIComponentRenderer;

		// Token: 0x04001CA7 RID: 7335
		public EntityStatUIComponentRenderer EntityStatUIComponentRenderer;
	}
}
