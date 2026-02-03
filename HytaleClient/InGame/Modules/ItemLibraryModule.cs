using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F6 RID: 2294
	internal class ItemLibraryModule : Module
	{
		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x0600443C RID: 17468 RVA: 0x000E5576 File Offset: 0x000E3776
		// (set) Token: 0x0600443D RID: 17469 RVA: 0x000E557E File Offset: 0x000E377E
		public Dictionary<string, ClientResourceType> ResourceTypes { get; private set; }

		// Token: 0x0600443E RID: 17470 RVA: 0x000E5587 File Offset: 0x000E3787
		public ItemLibraryModule(GameInstance gameInstance) : base(gameInstance)
		{
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x000E55A8 File Offset: 0x000E37A8
		public void PrepareItems(Dictionary<string, ItemBase> networkItems, Dictionary<string, Point> entitiesImageLocations, ref Dictionary<string, ClientItemBase> upcomingItems, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			this._modelsByChecksum.Clear();
			this._animationsByChecksum.Clear();
			using (Dictionary<string, ItemBase>.ValueCollection.Enumerator enumerator = networkItems.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemBase networkItem = enumerator.Current;
					bool isCancellationRequested = cancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						break;
					}
					bool flag = networkItem == null;
					if (!flag)
					{
						ClientItemBase clientItemBase = new ClientItemBase();
						ClientItemBaseProtocolInitializer.Parse(networkItem, this._gameInstance.EntityStoreModule.NodeNameManager, ref clientItemBase);
						bool flag2 = clientItemBase.BlockId != 0;
						if (!flag2)
						{
							bool flag3 = networkItem.Model == null;
							if (flag3)
							{
								this._gameInstance.App.DevTools.Error("Missing model for item " + networkItem.Id);
								clientItemBase.Model = new BlockyModel(0);
							}
							else
							{
								string text;
								bool flag4 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(networkItem.Model, ref text);
								if (flag4)
								{
									this._gameInstance.App.DevTools.Error("Missing model asset: " + networkItem.Model + " for item " + networkItem.Id);
									clientItemBase.Model = new BlockyModel(0);
								}
								else
								{
									BlockyModel blockyModel;
									bool flag5 = !this._modelsByChecksum.TryGetValue(text, out blockyModel);
									if (flag5)
									{
										try
										{
											blockyModel = new BlockyModel(BlockyModel.MaxNodeCount);
											BlockyModelInitializer.Parse(AssetManager.GetAssetUsingHash(text, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyModel);
											this._modelsByChecksum[text] = blockyModel;
										}
										catch (Exception innerException)
										{
											throw new Exception(string.Concat(new string[]
											{
												"Failed to parse BlockyModel for item: ",
												networkItem.Id,
												", Model: ",
												networkItem.Model,
												" (",
												text,
												")"
											}), innerException);
										}
									}
									clientItemBase.Model = blockyModel.Clone();
									bool flag6 = networkItem.ItemAppearanceConditions != null;
									if (flag6)
									{
										this.PrepareItemAppearanceConditions(networkItem, clientItemBase, blockyModel, entitiesImageLocations);
									}
								}
							}
							clientItemBase.Model.SetAtlasIndex(1);
							bool flag7 = entitiesImageLocations != null && clientItemBase.Model != null;
							if (flag7)
							{
								this.PrepareItemUV(clientItemBase, entitiesImageLocations);
							}
							bool flag8 = networkItem.Animation != null;
							if (flag8)
							{
								string text2;
								bool flag9 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(networkItem.Animation, ref text2);
								if (flag9)
								{
									this._gameInstance.App.DevTools.Error("Missing animated asset: " + networkItem.Animation + " for item " + networkItem.Id);
								}
								else
								{
									clientItemBase.Animation = this._animationsByChecksum.GetOrAdd(text2, delegate(string x)
									{
										BlockyAnimation result;
										try
										{
											BlockyAnimation blockyAnimation = new BlockyAnimation();
											BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(x, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyAnimation);
											result = blockyAnimation;
										}
										catch (Exception exception)
										{
											ItemLibraryModule.Logger.Error(exception, "Failed to parse BlockyAnimation for item: " + networkItem.Id + ", Animation: " + networkItem.Animation);
											result = null;
										}
										return result;
									});
								}
							}
						}
						bool flag10 = networkItem.DroppedItemAnimation != null;
						if (flag10)
						{
							string text3;
							bool flag11 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(networkItem.DroppedItemAnimation, ref text3);
							if (flag11)
							{
								this._gameInstance.App.DevTools.Error("Missing animated asset: " + networkItem.DroppedItemAnimation + " for item " + networkItem.Id);
							}
							else
							{
								clientItemBase.DroppedItemAnimation = this._animationsByChecksum.GetOrAdd(text3, delegate(string x)
								{
									BlockyAnimation result;
									try
									{
										BlockyAnimation blockyAnimation = new BlockyAnimation();
										BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(x, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyAnimation);
										result = blockyAnimation;
									}
									catch (Exception exception)
									{
										ItemLibraryModule.Logger.Error(exception, "Failed to parse BlockyAnimation for item: " + networkItem.Id + ", Animation: " + networkItem.DroppedItemAnimation);
										result = null;
									}
									return result;
								});
							}
						}
						upcomingItems[networkItem.Id] = clientItemBase;
					}
				}
			}
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x000E59E8 File Offset: 0x000E3BE8
		private void PrepareItemAppearanceConditions(ItemBase networkItem, ClientItemBase clientItem, BlockyModel baseModel, Dictionary<string, Point> entitiesImageLocations)
		{
			foreach (KeyValuePair<int, ItemAppearanceCondition[]> keyValuePair in networkItem.ItemAppearanceConditions)
			{
				BlockyModel[] array = new BlockyModel[keyValuePair.Value.Length];
				for (int i = 0; i < keyValuePair.Value.Length; i++)
				{
					ItemAppearanceCondition itemAppearanceCondition = keyValuePair.Value[i];
					bool flag = itemAppearanceCondition.Model != null;
					BlockyModel blockyModel2;
					if (flag)
					{
						string text;
						bool flag2 = this._gameInstance.HashesByServerAssetPath.TryGetValue(itemAppearanceCondition.Model, ref text);
						if (flag2)
						{
							BlockyModel blockyModel;
							bool flag3 = !this._modelsByChecksum.TryGetValue(text, out blockyModel);
							if (flag3)
							{
								try
								{
									blockyModel = new BlockyModel(BlockyModel.MaxNodeCount);
									BlockyModelInitializer.Parse(AssetManager.GetAssetUsingHash(text, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyModel);
									this._modelsByChecksum[text] = blockyModel;
								}
								catch (Exception innerException)
								{
									throw new Exception(string.Concat(new string[]
									{
										"Failed to parse BlockyModel for item: ",
										networkItem.Id,
										", Model: ",
										itemAppearanceCondition.Model,
										" (",
										text,
										")"
									}), innerException);
								}
							}
							blockyModel2 = blockyModel.Clone();
						}
						else
						{
							blockyModel2 = baseModel.Clone();
						}
					}
					else
					{
						blockyModel2 = baseModel.Clone();
					}
					blockyModel2.SetAtlasIndex(1);
					bool flag4 = entitiesImageLocations != null && itemAppearanceCondition.Texture != null;
					if (flag4)
					{
						string key;
						bool flag5 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(itemAppearanceCondition.Texture, ref key);
						if (flag5)
						{
							this._gameInstance.App.DevTools.Error("Missing texture asset: " + itemAppearanceCondition.Texture + " for item " + networkItem.Id);
						}
						else
						{
							Point offset;
							bool flag6 = !entitiesImageLocations.TryGetValue(key, out offset);
							if (flag6)
							{
								this._gameInstance.App.DevTools.Error("Cannot use " + itemAppearanceCondition.Texture + " as texture for item " + networkItem.Id);
							}
							else
							{
								blockyModel2.OffsetUVs(offset);
							}
						}
					}
					clientItem.ItemAppearanceConditions[keyValuePair.Key][i].Model = blockyModel2;
					array[i] = blockyModel2;
				}
			}
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x000E5C9C File Offset: 0x000E3E9C
		public void PrepareItemIconAtlas(Dictionary<string, ItemBase> networkItems, out Dictionary<string, ClientIcon> icons, out byte[] pixels, out int width, out int height, CancellationToken cancellationToken)
		{
			Dictionary<string, ItemLibraryModule.IconTextureInfo> dictionary = new Dictionary<string, ItemLibraryModule.IconTextureInfo>();
			icons = new Dictionary<string, ClientIcon>();
			pixels = null;
			width = 2048;
			height = 64;
			foreach (ItemBase itemBase in networkItems.Values)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					return;
				}
				bool flag = ((itemBase != null) ? itemBase.Icon : null) == null;
				if (!flag)
				{
					string text;
					bool flag2 = this._gameInstance.HashesByServerAssetPath.TryGetValue(itemBase.Icon, ref text);
					if (flag2)
					{
						ItemLibraryModule.IconTextureInfo iconTextureInfo;
						bool flag3 = !dictionary.TryGetValue(itemBase.Icon, out iconTextureInfo);
						if (flag3)
						{
							iconTextureInfo = new ItemLibraryModule.IconTextureInfo
							{
								Checksum = text,
								Name = itemBase.Icon
							};
							string assetLocalPathUsingHash = AssetManager.GetAssetLocalPathUsingHash(text);
							bool flag4 = Image.TryGetPngDimensions(assetLocalPathUsingHash, out iconTextureInfo.Width, out iconTextureInfo.Height);
							if (flag4)
							{
								dictionary[itemBase.Icon] = iconTextureInfo;
							}
							else
							{
								this._gameInstance.App.DevTools.Error(string.Concat(new string[]
								{
									"Failed to get PNG dimensions for: ",
									itemBase.Icon,
									", ",
									assetLocalPathUsingHash,
									" (",
									text,
									")"
								}));
							}
						}
					}
					else
					{
						this._gameInstance.App.DevTools.Error("Missing icon: " + itemBase.Icon + " for item " + itemBase.Id);
					}
				}
			}
			List<ItemLibraryModule.IconTextureInfo> list = new List<ItemLibraryModule.IconTextureInfo>(dictionary.Values);
			list.Sort((ItemLibraryModule.IconTextureInfo a, ItemLibraryModule.IconTextureInfo b) => b.Height.CompareTo(a.Height));
			Point zero = new Point(0, 0);
			int num = 0;
			foreach (ItemLibraryModule.IconTextureInfo iconTextureInfo2 in list)
			{
				bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested2)
				{
					return;
				}
				int num2 = Math.Min(64, Math.Min(iconTextureInfo2.Width, iconTextureInfo2.Height));
				bool flag5 = zero.X + num2 > width;
				if (flag5)
				{
					zero.X = 0;
					zero.Y = num;
				}
				while (zero.Y + num2 > height)
				{
					height <<= 1;
				}
				icons[iconTextureInfo2.Name] = new ClientIcon(zero.X, zero.Y, num2);
				num = Math.Max(num, zero.Y + num2);
				zero.X += num2;
			}
			pixels = new byte[width * height * 4];
			zero = Point.Zero;
			foreach (ItemLibraryModule.IconTextureInfo iconTextureInfo3 in list)
			{
				bool isCancellationRequested3 = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested3)
				{
					break;
				}
				try
				{
					Image image = new Image(AssetManager.GetAssetUsingHash(iconTextureInfo3.Checksum, false));
					ClientIcon clientIcon = icons[iconTextureInfo3.Name];
					for (int i = 0; i < clientIcon.Size; i++)
					{
						int dstOffset = ((clientIcon.Y + i) * width + clientIcon.X) * 4;
						Buffer.BlockCopy(image.Pixels, i * clientIcon.Size * 4, pixels, dstOffset, clientIcon.Size * 4);
					}
				}
				catch (Exception exception)
				{
					ItemLibraryModule.Logger.Error(exception, "Faile to load icon texture: " + AssetManager.GetAssetLocalPathUsingHash(iconTextureInfo3.Checksum));
				}
			}
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x000E60EC File Offset: 0x000E42EC
		public void PrepareItemUVs(ref Dictionary<string, ClientItemBase> upcomingItems, Dictionary<string, Point> entitiesImageLocations, CancellationToken cancellationToken)
		{
			foreach (ClientItemBase clientItemBase in upcomingItems.Values)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					break;
				}
				bool flag = clientItemBase.Model != null;
				if (flag)
				{
					this.PrepareItemUV(clientItemBase, entitiesImageLocations);
				}
				bool flag2 = clientItemBase.ItemAppearanceConditions != null;
				if (flag2)
				{
					this.PrepareItemAppearanceConditionUV(clientItemBase, entitiesImageLocations);
				}
			}
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x000E617C File Offset: 0x000E437C
		private void PrepareItemUV(ClientItemBase item, Dictionary<string, Point> entitiesImageLocations)
		{
			string key;
			bool flag = !this._gameInstance.HashesByServerAssetPath.TryGetValue(item.Texture, ref key);
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Missing texture asset: " + item.Texture + " for item " + item.Id);
			}
			else
			{
				Point offset;
				bool flag2 = !entitiesImageLocations.TryGetValue(key, out offset);
				if (flag2)
				{
					this._gameInstance.App.DevTools.Error("Cannot use " + item.Texture + " as texture for item " + item.Id);
				}
				else
				{
					item.Model.OffsetUVs(offset);
				}
			}
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x000E6234 File Offset: 0x000E4434
		private void PrepareItemAppearanceConditionUV(ClientItemBase item, Dictionary<string, Point> entitiesImageLocations)
		{
			foreach (KeyValuePair<int, ClientItemAppearanceCondition[]> keyValuePair in item.ItemAppearanceConditions)
			{
				for (int i = 0; i < keyValuePair.Value.Length; i++)
				{
					ClientItemAppearanceCondition clientItemAppearanceCondition = keyValuePair.Value[i];
					bool flag = clientItemAppearanceCondition.Texture == null;
					if (!flag)
					{
						string key;
						bool flag2 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(clientItemAppearanceCondition.Texture, ref key);
						if (flag2)
						{
							this._gameInstance.App.DevTools.Error("Failed to load entity effect texture: " + clientItemAppearanceCondition.Texture);
							return;
						}
						Point offset;
						bool flag3 = !entitiesImageLocations.TryGetValue(key, out offset);
						if (flag3)
						{
							this._gameInstance.App.DevTools.Error("Cannot use " + clientItemAppearanceCondition.Texture + " as an entity effect texture");
							return;
						}
						clientItemAppearanceCondition.Model.OffsetUVs(offset);
					}
				}
			}
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x000E6368 File Offset: 0x000E4568
		public void PrepareItemPlayerAnimations(Dictionary<string, ItemPlayerAnimations> networkItemAnimations, out Dictionary<string, ClientItemPlayerAnimations> upcomingItemPlayerAnimationsById)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingItemPlayerAnimationsById = new Dictionary<string, ClientItemPlayerAnimations>();
			foreach (ItemPlayerAnimations itemPlayerAnimations in networkItemAnimations.Values)
			{
				ClientItemPlayerAnimations clientItemPlayerAnimations = new ClientItemPlayerAnimations(itemPlayerAnimations);
				foreach (KeyValuePair<string, ItemAnimation> keyValuePair in itemPlayerAnimations.Animations)
				{
					clientItemPlayerAnimations.Animations[keyValuePair.Key] = this.LoadItemAnimation(itemPlayerAnimations.Id + "/" + keyValuePair.Key, keyValuePair.Value, itemPlayerAnimations.PullbackConfig, itemPlayerAnimations.UseFirstPersonOverride);
				}
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "RunBackward", "Run");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "Sprint", "Run");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "CrouchWalk", "Run");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "CrouchWalkBackward", "CrouchWalk");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "JumpRun", "JumpWalk");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "JumpSprint", "JumpRun");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "Jump", "JumpWalk");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimBackward", "Swim");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimFast", "Swim");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimFloat", "SwimSink");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimIdle", "SwimSink");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimDive", "Swim");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimDiveFast", "SwimDive");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimDiveBackward", "SwimDive");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SwimJump", "JumpWalk");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "FluidIdle", "Idle");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "FluidWalk", "Run");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "FluidWalkBackward", "RunBackward");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "FluidRun", "Sprint");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "CrouchSlide", "CrouchWalk");
				this.SetAnimationFallback(clientItemPlayerAnimations.Animations, "SafetyRoll", "CrouchWalk");
				upcomingItemPlayerAnimationsById[itemPlayerAnimations.Id] = clientItemPlayerAnimations;
			}
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x000E6668 File Offset: 0x000E4868
		public void SetupItemPlayerAnimations(Dictionary<string, ClientItemPlayerAnimations> animationsById)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._itemPlayerAnimationsById = animationsById;
			bool flag = !animationsById.TryGetValue("Default", out this.DefaultItemPlayerAnimations);
			if (flag)
			{
				this.DefaultItemPlayerAnimations = new ClientItemPlayerAnimations(new ItemPlayerAnimations
				{
					Id = "Default",
					WiggleWeights_ = new ItemPlayerAnimations.WiggleWeights()
				});
			}
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x000E66C8 File Offset: 0x000E48C8
		public void SetupItemIcons(Dictionary<string, ClientIcon> icons, byte[] pixels, int width, int height)
		{
			Texture texture = new Texture(Texture.TextureTypes.Texture2D);
			texture.CreateTexture2D(width, height, pixels, 0, GL.LINEAR_MIPMAP_LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this.ItemIcons = icons;
			this._gameInstance.App.Interface.InGameView.OnItemIconsUpdated(texture);
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x000E6730 File Offset: 0x000E4930
		public void SetupResourceTypes(Dictionary<string, ClientResourceType> resourceTypes)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.ResourceTypes = resourceTypes;
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x000E6746 File Offset: 0x000E4946
		public void SetupItems(Dictionary<string, ClientItemBase> items)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._items = items;
			this.LinkItemPlayerAnimations();
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x000E6764 File Offset: 0x000E4964
		public void LinkItemPlayerAnimations()
		{
			foreach (ClientItemBase clientItemBase in this._items.Values)
			{
				ClientItemPlayerAnimations playerAnimations;
				bool flag = clientItemBase.PlayerAnimationsId != null && this._itemPlayerAnimationsById.TryGetValue(clientItemBase.PlayerAnimationsId, out playerAnimations);
				if (flag)
				{
					clientItemBase.PlayerAnimations = playerAnimations;
				}
				else
				{
					this._gameInstance.App.DevTools.Error("Missing playerAnimationsId for item " + clientItemBase.Id);
					clientItemBase.PlayerAnimations = this.DefaultItemPlayerAnimations;
				}
			}
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x000E681C File Offset: 0x000E4A1C
		public bool GetItemPlayerAnimation(string id, out ClientItemPlayerAnimations ret)
		{
			return this._itemPlayerAnimationsById.TryGetValue(id, out ret);
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x000E683C File Offset: 0x000E4A3C
		public EntityAnimation LoadItemAnimation(string itemId, ItemAnimation animation, ItemPullbackConfiguration pullbackConfig, bool useFirstPersonOverride)
		{
			string thirdPerson = animation.ThirdPerson;
			string thirdPersonMoving = animation.ThirdPersonMoving;
			string thirdPersonFace = animation.ThirdPersonFace;
			string firstPerson = animation.FirstPerson;
			string firstPersonOverride = animation.FirstPersonOverride;
			float speed = (animation.Speed != 0f) ? animation.Speed : 1f;
			float blendingDuration = animation.BlendingDuration * 60f;
			bool looping = animation.Looping;
			bool keepPreviousFirstPersonAnimation = animation.KeepPreviousFirstPersonAnimation;
			bool clipsGeometry = animation.ClipsGeometry;
			return this.LoadAnimation(itemId, thirdPerson, thirdPersonMoving, thirdPersonFace, firstPerson, firstPersonOverride, speed, blendingDuration, looping, keepPreviousFirstPersonAnimation, pullbackConfig, clipsGeometry, useFirstPersonOverride);
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x000E68D0 File Offset: 0x000E4AD0
		private EntityAnimation LoadAnimation(string itemId, string thirdPersonPath, string thirdPersonMovingPath, string thirdPersonFacePath, string firstPersonPath, string firstPersonOverridePath, float speed, float blendingDuration, bool looping, bool keepPreviousFirstPersonAnimation, ItemPullbackConfiguration pullbackConfig, bool clipsGeometry, bool useFirstPersonOverride)
		{
			BlockyAnimation data = null;
			bool flag = thirdPersonPath != null;
			if (flag)
			{
				string text;
				bool flag2 = this._gameInstance.HashesByServerAssetPath.TryGetValue(thirdPersonPath, ref text);
				if (flag2)
				{
					data = this._animationsByChecksum.GetOrAdd(text, delegate(string x)
					{
						BlockyAnimation result;
						try
						{
							BlockyAnimation blockyAnimation = new BlockyAnimation();
							BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(x, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyAnimation);
							result = blockyAnimation;
						}
						catch (Exception exception)
						{
							ItemLibraryModule.Logger.Error(exception, "Failed to parse BlockyAnimation for item: " + itemId + ", Animation: " + thirdPersonPath);
							result = null;
						}
						return result;
					});
				}
				else
				{
					this._gameInstance.App.DevTools.Error("Failed to load third person animation: " + thirdPersonPath + " for item: " + itemId);
				}
			}
			else
			{
				bool flag3 = !itemId.Contains("Default");
				if (flag3)
				{
					this._gameInstance.App.DevTools.Error("Missing third person animation for item: " + itemId);
				}
			}
			BlockyAnimation movingData = null;
			bool flag4 = thirdPersonMovingPath != null;
			if (flag4)
			{
				string text2;
				bool flag5 = this._gameInstance.HashesByServerAssetPath.TryGetValue(thirdPersonMovingPath, ref text2);
				if (flag5)
				{
					movingData = this._animationsByChecksum.GetOrAdd(text2, delegate(string x)
					{
						BlockyAnimation result;
						try
						{
							BlockyAnimation blockyAnimation = new BlockyAnimation();
							BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(x, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyAnimation);
							result = blockyAnimation;
						}
						catch (Exception exception)
						{
							ItemLibraryModule.Logger.Error(exception, "Failed to parse BlockyAnimation for item: " + itemId + ", Animation: " + thirdPersonMovingPath);
							result = null;
						}
						return result;
					});
				}
				else
				{
					this._gameInstance.App.DevTools.Error("Failed to load third person moving animation: " + thirdPersonMovingPath + " for item: " + itemId);
				}
			}
			BlockyAnimation faceData = null;
			bool flag6 = thirdPersonFacePath != null;
			if (flag6)
			{
				string text3;
				bool flag7 = this._gameInstance.HashesByServerAssetPath.TryGetValue(thirdPersonFacePath, ref text3);
				if (flag7)
				{
					faceData = this._animationsByChecksum.GetOrAdd(text3, delegate(string x)
					{
						BlockyAnimation result;
						try
						{
							BlockyAnimation blockyAnimation = new BlockyAnimation();
							BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(x, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyAnimation);
							result = blockyAnimation;
						}
						catch (Exception exception)
						{
							ItemLibraryModule.Logger.Error(exception, "Failed to parse BlockyAnimation for item: " + itemId + ", Animation: " + thirdPersonFacePath);
							result = null;
						}
						return result;
					});
				}
				else
				{
					this._gameInstance.App.DevTools.Error("Failed to load face animation: " + thirdPersonFacePath + " for item: " + itemId);
				}
			}
			BlockyAnimation firstPersonData = null;
			bool flag8 = firstPersonPath != null;
			if (flag8)
			{
				string firstPersonAnimationChecksum;
				bool flag9 = this._gameInstance.HashesByServerAssetPath.TryGetValue(firstPersonPath, ref firstPersonAnimationChecksum);
				if (flag9)
				{
					firstPersonData = this._animationsByChecksum.GetOrAdd(firstPersonAnimationChecksum, delegate(string x)
					{
						BlockyAnimation result;
						try
						{
							BlockyAnimation blockyAnimation = new BlockyAnimation();
							BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(firstPersonAnimationChecksum, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyAnimation);
							result = blockyAnimation;
						}
						catch (Exception exception)
						{
							ItemLibraryModule.Logger.Error(exception, "Failed to parse BlockyAnimation for item: " + itemId + ", Animation: " + firstPersonPath);
							result = null;
						}
						return result;
					});
				}
				else
				{
					this._gameInstance.App.DevTools.Error("Failed to load first person animation: " + firstPersonPath + " for item: " + itemId);
				}
			}
			BlockyAnimation firstPersonOverrideData = null;
			bool flag10 = useFirstPersonOverride && firstPersonOverridePath != null;
			if (flag10)
			{
				string firstPersonOverrideAnimationChecksum;
				bool flag11 = this._gameInstance.HashesByServerAssetPath.TryGetValue(firstPersonOverridePath, ref firstPersonOverrideAnimationChecksum);
				if (flag11)
				{
					firstPersonOverrideData = this._animationsByChecksum.GetOrAdd(firstPersonOverrideAnimationChecksum, delegate(string x)
					{
						BlockyAnimation result;
						try
						{
							BlockyAnimation blockyAnimation = new BlockyAnimation();
							BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(firstPersonOverrideAnimationChecksum, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref blockyAnimation);
							result = blockyAnimation;
						}
						catch (Exception exception)
						{
							ItemLibraryModule.Logger.Error(exception, "Failed to parse BlockyAnimation for item: " + itemId + ", Animation: " + firstPersonPath);
							result = null;
						}
						return result;
					});
				}
				else
				{
					this._gameInstance.App.DevTools.Error("Failed to load first person override animation: " + firstPersonPath + " for item: " + itemId);
				}
			}
			return new EntityAnimation(data, speed, blendingDuration, looping, keepPreviousFirstPersonAnimation, 0U, 0f, Array.Empty<int>(), 0, movingData, faceData, firstPersonData, firstPersonOverrideData, pullbackConfig, clipsGeometry);
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x000E6C64 File Offset: 0x000E4E64
		private void SetAnimationFallback(Dictionary<string, EntityAnimation> animations, string source, string fallback)
		{
			bool flag = !animations.ContainsKey(source);
			if (flag)
			{
				EntityAnimation animation;
				bool flag2 = animations.TryGetValue(fallback, out animation);
				if (flag2)
				{
					animations.Add(source, new EntityAnimation(animation));
				}
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x000E6C9D File Offset: 0x000E4E9D
		public Dictionary<string, ClientItemBase> GetItems()
		{
			return this._items;
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x000E6CA8 File Offset: 0x000E4EA8
		public ClientItemBase GetItem(string itemId)
		{
			ClientItemBase clientItemBase;
			bool flag = itemId == null || !this._items.TryGetValue(itemId, out clientItemBase);
			ClientItemBase result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = clientItemBase;
			}
			return result;
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x000E6CDC File Offset: 0x000E4EDC
		public ClientResourceType GetResourceType(string resourceTypeId)
		{
			ClientResourceType clientResourceType;
			bool flag = resourceTypeId == null || !this.ResourceTypes.TryGetValue(resourceTypeId, out clientResourceType);
			ClientResourceType result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = clientResourceType;
			}
			return result;
		}

		// Token: 0x040021AE RID: 8622
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040021AF RID: 8623
		private Dictionary<string, ClientItemPlayerAnimations> _itemPlayerAnimationsById;

		// Token: 0x040021B0 RID: 8624
		public ClientItemPlayerAnimations DefaultItemPlayerAnimations;

		// Token: 0x040021B1 RID: 8625
		public Dictionary<string, ClientIcon> ItemIcons;

		// Token: 0x040021B2 RID: 8626
		private Dictionary<string, ClientItemBase> _items;

		// Token: 0x040021B3 RID: 8627
		private readonly Dictionary<string, BlockyModel> _modelsByChecksum = new Dictionary<string, BlockyModel>();

		// Token: 0x040021B4 RID: 8628
		private readonly ConcurrentDictionary<string, BlockyAnimation> _animationsByChecksum = new ConcurrentDictionary<string, BlockyAnimation>();

		// Token: 0x02000DC1 RID: 3521
		private class IconTextureInfo
		{
			// Token: 0x040043B7 RID: 17335
			public string Name;

			// Token: 0x040043B8 RID: 17336
			public string Checksum;

			// Token: 0x040043B9 RID: 17337
			public int Width;

			// Token: 0x040043BA RID: 17338
			public int Height;
		}
	}
}
