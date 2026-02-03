using System;
using System.Linq;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud.Abilities
{
	// Token: 0x020008D0 RID: 2256
	internal class AbilitiesHudComponent : InterfaceComponent
	{
		// Token: 0x06004188 RID: 16776 RVA: 0x000C1DA5 File Offset: 0x000BFFA5
		public AbilitiesHudComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x000C1DC4 File Offset: 0x000BFFC4
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/Abilities/AbilitiesHud.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._hudContainer = uifragment.Get<Group>("AbilitiesHudContainer");
			this._mainAbility = new MainAbility(this.InGameView, this.Desktop, uifragment.Get<Group>("MainAbility"));
			this._mainAbility.Build();
			this._secondaryAbility = new SecondaryAbility(this.InGameView, this.Desktop, uifragment.Get<Group>("SecondaryAbility"));
			this._secondaryAbility.Build();
			this._signatureAbility = new SignatureAbility(this.InGameView, this.Desktop, uifragment.Get<Group>("SignatureAbility"));
			this._signatureAbility.Build();
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x000C1E98 File Offset: 0x000C0098
		public void ShowOrHideHud()
		{
			ClientItemStack activeHotbarItem = this.InGameView.InGame.Instance.InventoryModule.GetActiveHotbarItem();
			ClientItemBase item = this.InGameView.InGame.Instance.ItemLibraryModule.GetItem((activeHotbarItem != null) ? activeHotbarItem.Id : null);
			bool? flag;
			if (item == null)
			{
				flag = null;
			}
			else
			{
				string[] categories = item.Categories;
				flag = ((categories != null) ? new bool?(Enumerable.Contains<string>(categories, "Items.Weapons")) : null);
			}
			bool? flag2 = flag;
			bool valueOrDefault = flag2.GetValueOrDefault();
			bool flag3 = !valueOrDefault;
			if (flag3)
			{
				ClientItemStack utilityItem = this.InGameView.InGame.Instance.InventoryModule.GetUtilityItem(this.InGameView.InGame.Instance.InventoryModule.UtilityActiveSlot);
				item = this.InGameView.InGame.Instance.ItemLibraryModule.GetItem((utilityItem != null) ? utilityItem.Id : null);
				bool? flag4;
				if (item == null)
				{
					flag4 = null;
				}
				else
				{
					string[] categories2 = item.Categories;
					flag4 = ((categories2 != null) ? new bool?(Enumerable.Contains<string>(categories2, "Items.Weapons")) : null);
				}
				flag2 = flag4;
				valueOrDefault = flag2.GetValueOrDefault();
			}
			this._hudContainer.Visible = valueOrDefault;
			this._mainAbility.SetCharges();
			this._secondaryAbility.SetCharges();
			this._hudContainer.Layout(new Rectangle?(base.RectangleAfterPadding), true);
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x000C2009 File Offset: 0x000C0209
		public void CooldownError(ClientRootInteraction rootInteraction)
		{
			this._mainAbility.CooldownError(rootInteraction.Id);
			this._secondaryAbility.CooldownError(rootInteraction.Id);
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x000C2030 File Offset: 0x000C0230
		public void OnSignatureEnergyStatChanged(ClientEntityStatValue entityStatValue)
		{
			bool flag = !this._hudContainer.Visible;
			if (!flag)
			{
				this._signatureAbility.OnSignatureEnergyStatChanged(entityStatValue);
			}
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x000C205F File Offset: 0x000C025F
		protected override void OnMounted()
		{
			this.Interface.InGameView.UpdateAbilitiesHudVisibility(true);
			this.ShowOrHideHud();
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x000C207B File Offset: 0x000C027B
		public void OnEffectRemoved(int effectIndex)
		{
			this._mainAbility.OnEffectRemoved(effectIndex);
			this._secondaryAbility.OnEffectRemoved(effectIndex);
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x000C2098 File Offset: 0x000C0298
		public void OnStartChain(string rootInteractionId)
		{
			this._mainAbility.OnStartChain(rootInteractionId);
			this._secondaryAbility.OnStartChain(rootInteractionId);
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x000C20B5 File Offset: 0x000C02B5
		public void OnTertiaryAction()
		{
			this._signatureAbility.OnSignatureAction();
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x000C20C4 File Offset: 0x000C02C4
		public void OnUpdateInputBindings()
		{
			this._mainAbility.UpdateInputBiding();
			this._secondaryAbility.UpdateInputBiding();
			this._signatureAbility.UpdateInputBiding();
		}

		// Token: 0x04001FD5 RID: 8149
		public readonly InGameView InGameView;

		// Token: 0x04001FD6 RID: 8150
		private Group _hudContainer;

		// Token: 0x04001FD7 RID: 8151
		private MainAbility _mainAbility;

		// Token: 0x04001FD8 RID: 8152
		private SecondaryAbility _secondaryAbility;

		// Token: 0x04001FD9 RID: 8153
		private SignatureAbility _signatureAbility;
	}
}
