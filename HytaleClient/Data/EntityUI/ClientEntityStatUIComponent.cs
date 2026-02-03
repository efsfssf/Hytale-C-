using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.InGame.EntityUI;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityUI
{
	// Token: 0x02000B02 RID: 2818
	internal class ClientEntityStatUIComponent : ClientEntityUIComponent
	{
		// Token: 0x0600586E RID: 22638 RVA: 0x001AFD0F File Offset: 0x001ADF0F
		private ClientEntityStatUIComponent()
		{
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x001AFD19 File Offset: 0x001ADF19
		public ClientEntityStatUIComponent(int id, EntityUIComponent component, EntityUIComponentRenderer<ClientEntityStatUIComponent> renderer) : base(id, component)
		{
			this.EntityStatIndex = component.EntityStatIndex;
			this.Renderer = renderer;
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x001AFD38 File Offset: 0x001ADF38
		public override ClientEntityUIComponent Clone()
		{
			return new ClientEntityStatUIComponent
			{
				Id = this.Id,
				HitboxOffset = this.HitboxOffset,
				Unknown = this.Unknown,
				EntityStatIndex = this.EntityStatIndex,
				Renderer = this.Renderer
			};
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x001AFD8D File Offset: 0x001ADF8D
		public override void RegisterDrawTasksForEntity(Entity entity, Matrix transformationMatrix, float distanceToCamera, int entitiesCount, ref EntityUIDrawTask[] drawTasks, ref int drawTasksCount)
		{
			this.Renderer.RegisterDrawTasksForEntity(this, entity, transformationMatrix, distanceToCamera, entitiesCount, ref drawTasks, ref drawTasksCount);
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x001AFDA6 File Offset: 0x001ADFA6
		public override void PrepareForDraw(EntityUIDrawTask task)
		{
			this.Renderer.PrepareForDraw(this, task);
		}

		// Token: 0x040036E9 RID: 14057
		public int EntityStatIndex;

		// Token: 0x040036EA RID: 14058
		public EntityUIComponentRenderer<ClientEntityStatUIComponent> Renderer;
	}
}
