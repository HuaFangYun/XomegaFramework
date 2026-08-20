// Copyright (c) 2026 Xomega.Net. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace Xomega.Framework
{
    /// <summary>
    /// A property that represents an action, which can be bound to action buttons
    /// and control the state of the bound button, such visible or enabled.
    /// </summary>
    public class ActionProperty : BaseProperty
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Constructs an action property that is not part of any data object, e.g. is defined on a view level.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to obtain resource manager for the action.</param>
        /// <param name="name">The action name.</param>
        public ActionProperty(IServiceProvider serviceProvider, string name) : base(null, name)
        {
            this.serviceProvider = serviceProvider;
            ResourceKey = GetActionResourceKey(name);
        }

        /// <summary>
        /// Constructs an action property that is a part of a data object.
        /// </summary>
        /// <param name="parent">The data object the action belongs to.</param>
        /// <param name="name">The name of the action, which should be unique within the parent data object.</param>
        public ActionProperty(DataObject parent, string name) : base(parent, name)
        {
            parent?.AddAction(this);
            ResourceKey = GetActionResourceKey(name);
        }

        /// <summary>
        /// Computes the resource key for an action with the given name, prepending the 'Action_' prefix as needed.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>The resource key for the action.</returns>
        private static string GetActionResourceKey(string name) => (name.StartsWith("Action_") ? "" : "Action_") + name;

        /// <summary>
        /// Specifies whether the action is currently enabled.
        /// This is just an alias for the underlying editable flag to avoid confusion,
        /// since actions technically don't support editability.
        /// </summary>
        public bool Enabled { get => editable; set => Editable = value; }

        /// <summary>
        /// Sets the expression to use for computing whether the action is enabled.
        /// This is just an alias for setting computed editable flag to avoid confusion,
        /// since actions technically don't support editability.
        /// </summary>
        /// <param name="expression">Lambda expression used to compute if the action is enabled,
        /// or null to make the enabled flag non-computed.</param>
        /// <param name="args">Arguments for the specified expression to use for evaluation.</param>
        public void SetComputedEnabled(LambdaExpression expression, params object[] args) => SetComputedEditable(expression, args);

        /// <summary>
        /// Manually updates the property enabled (editable) flag with the computed result,
        /// in addition to automatic updates when the underlying properties change.
        /// </summary>
        public void UpdateComputedEnabled() => UpdateComputedEditable();

        /// <summary>
        /// Manually asynchronously updates the action enabled (editable) flag with the computed result,
        /// in addition to automatic updates when the underlying properties change.
        /// This is just an alias for the async computed editable update to avoid confusion,
        /// since actions technically don't support editability.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>The task for the asynchronous operation.</returns>
        public Task UpdateComputedEnabledAsync(CancellationToken token = default) => UpdateComputedEditableAsync(null, token);

        /// <summary>
        /// Overrides the parent resource key to use underscore as a separator,
        /// in order to distinguish action resources from the ones for regular properties that are separated with a dot.
        /// </summary>
        protected override string ParentResourceKey => parent?.GetResourceKey() + "_";

        /// <summary>
        /// Overrides access to resource manager to use the action's service provider for actions that don't have a parent data object.
        /// </summary>
        protected override ResourceManager ResourceMgr => serviceProvider?.GetService<ResourceManager>() ?? base.ResourceMgr;
    }
}
