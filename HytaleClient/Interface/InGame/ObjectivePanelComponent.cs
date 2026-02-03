using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000883 RID: 2179
	internal class ObjectivePanelComponent : InterfaceComponent
	{
		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x000A0616 File Offset: 0x0009E816
		public bool HasObjectives
		{
			get
			{
				return this._objectivesByGuid.Count > 0;
			}
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x000A0628 File Offset: 0x0009E828
		public ObjectivePanelComponent(InGameView view) : base(view.Interface, view.HudContainer)
		{
			this.InGameView = view;
			this.Interface.RegisterForEventFromEngine<Objective>("objectives.updateObjective", new Action<Objective>(this.OnAddUpdateObjective));
			this.Interface.RegisterForEventFromEngine<Guid>("objectives.removeObjective", new Action<Guid>(this.OnRemoveObjective));
			this.Interface.RegisterForEventFromEngine<Guid, int, ObjectiveTask>("objectives.updateTask", new Action<Guid, int, ObjectiveTask>(this.OnUpdateTask));
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x000A06C0 File Offset: 0x0009E8C0
		public void Build()
		{
			base.Clear();
			this.Interface.TryGetDocument("InGame/Hud/ObjectivePanelObjectiveSlot.ui", out this._objectiveDocument);
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/ObjectiveCommon.ui", out document);
			this._inProgressIconStyle = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "TaskIconInProgress");
			this._completeIconStyle = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "TaskIconComplete");
			this._inProgressTaskDescriptionStyle = document.ResolveNamedValue<LabelStyle>(this.Desktop.Provider, "TaskDescInProgress");
			this._completeTaskDescriptionStyle = document.ResolveNamedValue<LabelStyle>(this.Desktop.Provider, "TaskLabelComplete");
			this.Interface.TryGetDocument("InGame/Hud/ObjectivePanelTask.ui", out this._taskDocument);
			this._inProgressTaskCompletionStyle = this._taskDocument.ResolveNamedValue<LabelStyle>(this.Desktop.Provider, "PanelTaskCompletionInProgress");
			this._completeTaskCompletionStyle = this._taskDocument.ResolveNamedValue<LabelStyle>(this.Desktop.Provider, "PanelTaskCompletionComplete");
			Document document2;
			this.Interface.TryGetDocument("InGame/Hud/ObjectivePanel.ui", out document2);
			UIFragment uifragment = document2.Instantiate(this.Desktop, this);
			this._objectivePanel = uifragment.Get<Group>("ObjectivePanel");
			this._objectiveUIsByGuid.Clear();
			foreach (Objective objective in this._objectivesByGuid.Values)
			{
				this.AddPanelObjective(objective);
			}
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x000A0858 File Offset: 0x0009EA58
		private void OnAddUpdateObjective(Objective objective)
		{
			bool flag = !this._objectivesByGuid.ContainsKey(objective.ObjectiveUuid);
			if (flag)
			{
				this._objectivesByGuid.Add(objective.ObjectiveUuid, objective);
				this.AddPanelObjective(objective);
			}
			else
			{
				this._objectivesByGuid[objective.ObjectiveUuid] = objective;
				this.UpdatePanelObjective(objective);
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x000A08D8 File Offset: 0x0009EAD8
		private void AddPanelObjective(Objective objective)
		{
			UIFragment uifragment = this._objectiveDocument.Instantiate(this.Desktop, this._objectivePanel);
			uifragment.Get<Label>("ObjectiveTitle").Text = this.Desktop.Provider.GetText(objective.ObjectiveTitleKey, null, true);
			ObjectivePanelComponent.ObjectiveUI objectiveUI = new ObjectivePanelComponent.ObjectiveUI(uifragment.RootElements[0]);
			this._objectiveUIsByGuid.Add(objective.ObjectiveUuid, objectiveUI);
			this.AddTasks(objective, objectiveUI);
			this.InGameView.UpdateObjectivePanelVisibility(true);
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x000A0964 File Offset: 0x0009EB64
		private void UpdatePanelObjective(Objective objective)
		{
			ObjectivePanelComponent.ObjectiveUI objectiveUI = this._objectiveUIsByGuid[objective.ObjectiveUuid];
			objectiveUI.ObjectiveElement.Find<Group>("Tasks").Clear();
			objectiveUI.TaskElements.Clear();
			this.AddTasks(objective, objectiveUI);
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x000A09B0 File Offset: 0x0009EBB0
		private void AddTasks(Objective objective, ObjectivePanelComponent.ObjectiveUI objectiveUI)
		{
			Group root = objectiveUI.ObjectiveElement.Find<Group>("Tasks");
			foreach (ObjectiveTask objectiveTask in objective.Tasks)
			{
				Element item = this.AddTask(objectiveTask.TaskDescriptionKey, objectiveTask.CurrentCompletion, objectiveTask.CompletionNeeded, root);
				objectiveUI.TaskElements.Add(item);
			}
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x000A0A14 File Offset: 0x0009EC14
		private Element AddTask(string taskKey, int currentCompletion, int completionNeeded, Element root)
		{
			UIFragment uifragment = this._taskDocument.Instantiate(this.Desktop, root);
			uifragment.Get<Label>("TaskKey").Text = this.Desktop.Provider.GetText(taskKey, null, true);
			uifragment.Get<Label>("TaskCompletion").Text = this.Desktop.Provider.FormatNumber(currentCompletion) + "/" + this.Desktop.Provider.FormatNumber(completionNeeded);
			this.SetTaskStyle(uifragment.RootElements[0], currentCompletion == completionNeeded);
			return uifragment.RootElements[0];
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x000A0AC0 File Offset: 0x0009ECC0
		private void SetTaskStyle(Element taskElement, bool isComplete)
		{
			taskElement.Find<Group>("TaskIcon").Background = (isComplete ? this._completeIconStyle : this._inProgressIconStyle);
			taskElement.Find<Label>("TaskKey").Style = (isComplete ? this._completeTaskDescriptionStyle : this._inProgressTaskDescriptionStyle);
			taskElement.Find<Label>("TaskCompletion").Style = (isComplete ? this._completeTaskCompletionStyle : this._inProgressTaskCompletionStyle);
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x000A0B34 File Offset: 0x0009ED34
		private void OnRemoveObjective(Guid objectiveUuid)
		{
			ObjectivePanelComponent.ObjectiveUI objectiveUI;
			bool flag = !this._objectiveUIsByGuid.TryGetValue(objectiveUuid, out objectiveUI);
			if (!flag)
			{
				this._objectivePanel.Remove(objectiveUI.ObjectiveElement);
				this._objectiveUIsByGuid.Remove(objectiveUuid);
				this._objectivesByGuid.Remove(objectiveUuid);
				this.InGameView.UpdateObjectivePanelVisibility(false);
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x000A0BB0 File Offset: 0x0009EDB0
		private void OnUpdateTask(Guid objectiveUuid, int taskIndex, ObjectiveTask task)
		{
			ObjectivePanelComponent.ObjectiveUI objectiveUI;
			bool flag = !this._objectiveUIsByGuid.TryGetValue(objectiveUuid, out objectiveUI);
			if (!flag)
			{
				this._objectivesByGuid[objectiveUuid].Tasks[taskIndex] = task;
				Element element = objectiveUI.TaskElements[taskIndex];
				element.Find<Label>("TaskCompletion").Text = task.CurrentCompletion.ToString() + "/" + task.CompletionNeeded.ToString();
				this.SetTaskStyle(element, task.CurrentCompletion == task.CompletionNeeded);
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x000A0C5C File Offset: 0x0009EE5C
		public void ResetState()
		{
			this._objectivePanel.Clear();
			this._objectiveUIsByGuid.Clear();
			this._objectivesByGuid.Clear();
		}

		// Token: 0x04001CC7 RID: 7367
		public readonly InGameView InGameView;

		// Token: 0x04001CC8 RID: 7368
		private readonly Dictionary<Guid, Objective> _objectivesByGuid = new Dictionary<Guid, Objective>();

		// Token: 0x04001CC9 RID: 7369
		private readonly Dictionary<Guid, ObjectivePanelComponent.ObjectiveUI> _objectiveUIsByGuid = new Dictionary<Guid, ObjectivePanelComponent.ObjectiveUI>();

		// Token: 0x04001CCA RID: 7370
		private Document _objectiveDocument;

		// Token: 0x04001CCB RID: 7371
		private Document _taskDocument;

		// Token: 0x04001CCC RID: 7372
		private PatchStyle _inProgressIconStyle;

		// Token: 0x04001CCD RID: 7373
		private PatchStyle _completeIconStyle;

		// Token: 0x04001CCE RID: 7374
		private LabelStyle _inProgressTaskDescriptionStyle;

		// Token: 0x04001CCF RID: 7375
		private LabelStyle _completeTaskDescriptionStyle;

		// Token: 0x04001CD0 RID: 7376
		private LabelStyle _inProgressTaskCompletionStyle;

		// Token: 0x04001CD1 RID: 7377
		private LabelStyle _completeTaskCompletionStyle;

		// Token: 0x04001CD2 RID: 7378
		private Group _objectivePanel;

		// Token: 0x02000D56 RID: 3414
		private class ObjectiveUI
		{
			// Token: 0x1700142A RID: 5162
			// (get) Token: 0x06006522 RID: 25890 RVA: 0x00210E19 File Offset: 0x0020F019
			public Element ObjectiveElement { get; }

			// Token: 0x1700142B RID: 5163
			// (get) Token: 0x06006523 RID: 25891 RVA: 0x00210E21 File Offset: 0x0020F021
			public List<Element> TaskElements { get; }

			// Token: 0x06006524 RID: 25892 RVA: 0x00210E29 File Offset: 0x0020F029
			public ObjectiveUI(Element objectiveElement)
			{
				this.ObjectiveElement = objectiveElement;
				this.TaskElements = new List<Element>();
			}
		}
	}
}
