// Copyright (c) 2026 Xomega.Net. All rights reserved.

using System;
using System.Linq.Expressions;

namespace Xomega.Framework
{
    /// <summary>
    /// A static class that provides reusable expressions for computing property attributes,
    /// such as visibility, editability, etc. based on other data properties.
    /// </summary>
    public static class ComputedExpressions
    {
        /// <summary>
        /// An expression that evaluates to <c>true</c> when the given data property is not null
        /// for the current data row context, if applicable.
        /// </summary>
        public static readonly Expression<Func<DataProperty, DataRow, bool>> PropertyIsNotNull = (p, row) => !p.IsNull(row);

        /// <summary>
        /// An expression that evaluates to <c>true</c> when the given data object has not been modified.
        /// </summary>
        public static readonly Expression<Func<DataObject, bool>> ObjectIsNotModified = (o) => !o.Modified;

        /// <summary>
        /// An expression that evaluates to <c>true</c> when the given data object has been modified,
        /// or when the object doesn't track modifications, in which case it is always considered modified.
        /// </summary>
        public static readonly Expression<Func<DataObject, bool>> ObjectIsModified = (o) => o != null && (o.Modified || !o.TrackModifications);

        /// <summary>
        /// An expression that evaluates to <c>true</c> when the given data object is not new.
        /// </summary>
        public static readonly Expression<Func<DataObject, bool>> ObjectIsNotNew = (o) => o != null && !o.IsNew;

        /// <summary>
        /// An expression that evaluates to <c>true</c> when the given data object is new.
        /// </summary>
        public static readonly Expression<Func<DataObject, bool>> ObjectIsNew = (o) => o != null && o.IsNew;
    }
}
