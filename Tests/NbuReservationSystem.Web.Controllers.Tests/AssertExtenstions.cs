namespace NbuReservationSystem.Web.Controllers.Tests
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using Xunit;

    public static class AssertExtenstions
    {
        public static void ModelErrorIsSet<T>(ViewResult view, Expression<Func<T>> property)
        {
            Assert.False(view.ViewData.ModelState.IsValid);
            Assert.NotNull(view.ViewData.ModelState[GetPropertyName(property)]);
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
