namespace NbuReservationSystem.Web.Controllers.Tests
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using Xunit;

    public static class AssertExtenstions
    {
        /// <summary>
        /// Validates that the given model has error set for the specific property.
        /// </summary>
        /// <typeparam name="T">type of the model</typeparam>
        /// <param name="view">the action result of the controller method invocation</param>
        /// <param name="property">property which has invalid value</param>
        public static void ModelErrorIsSet<T>(ViewResult view, Expression<Func<T>> property)
        {
            Assert.False(view.ViewData.ModelState.IsValid);
            Assert.NotNull(view.ViewData.ModelState[GetPropertyName(property)]);
        }

        /// <summary>
        /// Validates that the view has "general" errors, not related to specific key.
        /// </summary>
        /// <param name="view">the action result of the controller method invocation</param>
        public static void ModelErrorIsSet(ViewResult view)
        {
            Assert.False(view.ViewData.ModelState.IsValid);
            Assert.NotNull(view.ViewData.ModelState[string.Empty]);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> property)
        {

            return GetMemberInfo(property).Member.Name;
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            var lambda = method as LambdaExpression;
            if (lambda == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
            {
                throw new ArgumentException(nameof(method));
            }

            return memberExpr;
        }
    }
}
