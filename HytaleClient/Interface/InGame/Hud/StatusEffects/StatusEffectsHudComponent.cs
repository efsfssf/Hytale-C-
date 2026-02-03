using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.Items;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Hud.StatusEffects
{
	// Token: 0x020008CC RID: 2252
	internal class StatusEffectsHudComponent : InterfaceComponent
	{
		// Token: 0x0600416D RID: 16749 RVA: 0x000C10EC File Offset: 0x000BF2EC
		public StatusEffectsHudComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x000C1144 File Offset: 0x000BF344
		public void Build()
		{
			base.Clear();
			this._trinketBuffs = new Dictionary<string, TrinketBuffStatusEffect>();
			this._debuffs = new Dictionary<int, DebuffStatusEffect>();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/StatusEffects/StatusEffectHud.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._buffHudContainer = uifragment.Get<Group>("StatusEffectsHudContainer");
			this._buffsContainer = uifragment.Get<Group>("BuffsContainer");
			this._debuffsContainer = uifragment.Get<Group>("DebuffsContainer");
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x000C11C4 File Offset: 0x000BF3C4
		public void UpdateTrinketsBuffs()
		{
			List<TrinketBuffStatusEffect> list = new List<TrinketBuffStatusEffect>();
			foreach (KeyValuePair<string, TrinketBuffStatusEffect> keyValuePair in this._trinketBuffs)
			{
				list.Add(keyValuePair.Value);
			}
			foreach (int num in StatusEffectsHudComponent._trinketsSlots)
			{
				ClientItemStack clientItemStack = this.InGameView.InGame.Instance.InventoryModule._armorInventory[num];
				bool flag = clientItemStack == null;
				if (!flag)
				{
					TrinketBuffStatusEffect trinketBuffStatusEffect;
					this._trinketBuffs.TryGetValue(clientItemStack.Id, out trinketBuffStatusEffect);
					bool flag2 = !Enumerable.Contains<string>(StatusEffectsHudComponent._permanentTrinketBuffs, clientItemStack.Id) && clientItemStack.Id != LastStandSkullStatusEffect._lastStandSkullName && clientItemStack.Id != VampireFangsStatusEffect.VampireFangsName;
					if (!flag2)
					{
						bool flag3 = trinketBuffStatusEffect == null;
						if (flag3)
						{
							this.AddTrinketBuff(clientItemStack.Id);
						}
						else
						{
							this._buffsContainer.Reorder(trinketBuffStatusEffect, this._buffsContainer.Children.Count - 1);
							list.Remove(trinketBuffStatusEffect);
						}
					}
				}
			}
			foreach (TrinketBuffStatusEffect trinketBuffStatusEffect2 in list)
			{
				this._buffsContainer.Remove(trinketBuffStatusEffect2);
				this._trinketBuffs.Remove(trinketBuffStatusEffect2.Id);
			}
			this._buffHudContainer.Layout(null, true);
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x000C1390 File Offset: 0x000BF590
		public void OnEffectAdded(int effectIndex)
		{
			LastStandSkullStatusEffect lastStandSkull = this._lastStandSkull;
			if (lastStandSkull != null)
			{
				lastStandSkull.OnEffectAdded(effectIndex);
			}
			VampireFangsStatusEffect vampireFangs = this._vampireFangs;
			if (vampireFangs != null)
			{
				vampireFangs.OnEffectAdded(effectIndex);
			}
			GameInstance instance = this.InGameView.InGame.Instance;
			PlayerEntity playerEntity = (instance != null) ? instance.LocalPlayer : null;
			bool flag = playerEntity == null;
			if (!flag)
			{
				Entity.UniqueEntityEffect[] entityEffects = playerEntity.EntityEffects;
				Entity.UniqueEntityEffect? uniqueEntityEffect = null;
				foreach (Entity.UniqueEntityEffect uniqueEntityEffect2 in entityEffects)
				{
					bool flag2 = uniqueEntityEffect2.NetworkEffectIndex == effectIndex;
					if (flag2)
					{
						uniqueEntityEffect = new Entity.UniqueEntityEffect?(uniqueEntityEffect2);
						break;
					}
				}
				bool flag3 = uniqueEntityEffect == null;
				if (!flag3)
				{
					bool isDebuff = uniqueEntityEffect.Value.IsDebuff;
					if (isDebuff)
					{
						this.AddDebuff(uniqueEntityEffect.Value);
					}
					else
					{
						bool flag4 = uniqueEntityEffect.Value.StatusEffectIcon != null;
						if (flag4)
						{
							this.AddEntityEffectBuff(uniqueEntityEffect.Value);
						}
					}
				}
			}
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x000C1498 File Offset: 0x000BF698
		private void AddDebuff(Entity.UniqueEntityEffect entityEffect)
		{
			bool flag = this._debuffs.ContainsKey(entityEffect.NetworkEffectIndex);
			if (flag)
			{
				DebuffStatusEffect debuffStatusEffect = this._debuffs[entityEffect.NetworkEffectIndex];
				debuffStatusEffect.SetInitialCountdown(entityEffect.RemainingDuration);
			}
			else
			{
				DebuffStatusEffect debuffStatusEffect2 = new DebuffStatusEffect(this.InGameView, this.Desktop, this._debuffsContainer, entityEffect, entityEffect.NetworkEffectIndex);
				this._debuffs.Add(debuffStatusEffect2.Id, debuffStatusEffect2);
				debuffStatusEffect2.Build();
				this._buffHudContainer.Layout(null, true);
			}
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x000C152C File Offset: 0x000BF72C
		private void AddEntityEffectBuff(Entity.UniqueEntityEffect entityEffect)
		{
			bool flag = this._entityEffectBuffs.ContainsKey(entityEffect.NetworkEffectIndex);
			if (flag)
			{
				EntityEffectBuff entityEffectBuff = this._entityEffectBuffs[entityEffect.NetworkEffectIndex];
				entityEffectBuff.SetInitialCountdown(entityEffect.RemainingDuration);
				this._buffsContainer.Reorder(entityEffectBuff, 0);
				this._buffHudContainer.Layout(null, true);
			}
			else
			{
				EntityEffectBuff entityEffectBuff2 = new EntityEffectBuff(this.InGameView, this.Desktop, this._buffsContainer, entityEffect, entityEffect.NetworkEffectIndex);
				this._entityEffectBuffs.Add(entityEffectBuff2.Id, entityEffectBuff2);
				entityEffectBuff2.Build();
				this._buffsContainer.Reorder(entityEffectBuff2, 0);
				this._buffHudContainer.Layout(null, true);
			}
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x000C15F4 File Offset: 0x000BF7F4
		public void OnEffectRemoved(int effectIndex)
		{
			LastStandSkullStatusEffect lastStandSkull = this._lastStandSkull;
			if (lastStandSkull != null)
			{
				lastStandSkull.OnEffectRemoved(effectIndex);
			}
			VampireFangsStatusEffect vampireFangs = this._vampireFangs;
			if (vampireFangs != null)
			{
				vampireFangs.OnEffectRemoved(effectIndex);
			}
			bool flag = this._debuffs.ContainsKey(effectIndex);
			if (flag)
			{
				DebuffStatusEffect debuffStatusEffect = this._debuffs[effectIndex];
				this._debuffsContainer.Remove(debuffStatusEffect);
				this._debuffs.Remove(debuffStatusEffect.Id);
				this._buffHudContainer.Layout(null, true);
			}
			else
			{
				bool flag2 = this._entityEffectBuffs.ContainsKey(effectIndex);
				if (flag2)
				{
					EntityEffectBuff entityEffectBuff = this._entityEffectBuffs[effectIndex];
					this._buffsContainer.Remove(entityEffectBuff);
					this._entityEffectBuffs.Remove(entityEffectBuff.Id);
					this._buffHudContainer.Layout(null, true);
				}
			}
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x000C16D4 File Offset: 0x000BF8D4
		private void AddTrinketBuff(string id)
		{
			bool flag = id == LastStandSkullStatusEffect._lastStandSkullName;
			if (flag)
			{
				this.AddLastStandSkullBuff(id);
			}
			else
			{
				bool flag2 = id == VampireFangsStatusEffect.VampireFangsName;
				if (flag2)
				{
					this.AddVampireFangsBuff(id);
				}
				else
				{
					TrinketPermanentBuff trinketPermanentBuff = new TrinketPermanentBuff(this.InGameView, this.Desktop, this._buffsContainer, id);
					this._trinketBuffs.Add(trinketPermanentBuff.Id, trinketPermanentBuff);
					trinketPermanentBuff.Build();
					this._buffsContainer.Reorder(trinketPermanentBuff, this._buffsContainer.Children.Count - 1);
				}
			}
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x000C1768 File Offset: 0x000BF968
		private void AddLastStandSkullBuff(string id)
		{
			LastStandSkullStatusEffect lastStandSkullStatusEffect = new LastStandSkullStatusEffect(this.InGameView, this.Desktop, this._buffsContainer, id);
			this._lastStandSkull = lastStandSkullStatusEffect;
			this._trinketBuffs.Add(lastStandSkullStatusEffect.Id, lastStandSkullStatusEffect);
			lastStandSkullStatusEffect.Build();
			this._buffsContainer.Reorder(lastStandSkullStatusEffect, this._buffsContainer.Children.Count - 1);
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x000C17D0 File Offset: 0x000BF9D0
		private void AddVampireFangsBuff(string id)
		{
			VampireFangsStatusEffect vampireFangsStatusEffect = new VampireFangsStatusEffect(this.InGameView, this.Desktop, this._buffsContainer, id);
			this._vampireFangs = vampireFangsStatusEffect;
			this._trinketBuffs.Add(vampireFangsStatusEffect.Id, vampireFangsStatusEffect);
			vampireFangsStatusEffect.Build();
			this._buffsContainer.Reorder(vampireFangsStatusEffect, this._buffsContainer.Children.Count - 1);
		}

		// Token: 0x04001FC1 RID: 8129
		public readonly InGameView InGameView;

		// Token: 0x04001FC2 RID: 8130
		private static readonly int[] _trinketsSlots = new int[]
		{
			5,
			6,
			7
		};

		// Token: 0x04001FC3 RID: 8131
		private Dictionary<string, TrinketBuffStatusEffect> _trinketBuffs = new Dictionary<string, TrinketBuffStatusEffect>();

		// Token: 0x04001FC4 RID: 8132
		private Dictionary<int, EntityEffectBuff> _entityEffectBuffs = new Dictionary<int, EntityEffectBuff>();

		// Token: 0x04001FC5 RID: 8133
		private Dictionary<int, DebuffStatusEffect> _debuffs = new Dictionary<int, DebuffStatusEffect>();

		// Token: 0x04001FC6 RID: 8134
		private Group _buffsContainer;

		// Token: 0x04001FC7 RID: 8135
		private Group _debuffsContainer;

		// Token: 0x04001FC8 RID: 8136
		private Group _buffHudContainer;

		// Token: 0x04001FC9 RID: 8137
		private static readonly string[] _permanentTrinketBuffs = new string[]
		{
			"Trinket_Magic_Feather",
			"Trinket_Ring_Of_Fire",
			"Trinket_The_Camels_Straw",
			"Trinket_Pocket_Cactus",
			"Trinket_Shoe_Glue",
			"Trinket_Power_Glove",
			"Trinket_Bear_Tooth_Necklace",
			"Trinket_Avatars_Capacitor"
		};

		// Token: 0x04001FCA RID: 8138
		private LastStandSkullStatusEffect _lastStandSkull = null;

		// Token: 0x04001FCB RID: 8139
		private VampireFangsStatusEffect _vampireFangs = null;
	}
}
