using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Utils {
    static class ExpressionHelper {
        public static MemberInfo GetMemberInfo<T>(Expression<Func<T>> expression, MemberTypes memberType) {
            MemberExpression memberExpression = GetMemberExpression(expression);
            if(memberType != memberExpression.Member.MemberType)
                Guard.ArgumentException("expression");
            return memberExpression.Member;
        }
        public static string GetPropertyName<T>(Expression<Func<T>> expression) {
            MemberExpression memberExpression = GetMemberExpression(expression);
            MemberExpression nextMemberExpression = memberExpression.Expression as MemberExpression;
            if(IsPropertyExpression(nextMemberExpression)) {
                Guard.ArgumentException("expression");
            }
            return memberExpression.Member.Name;
        }

        public static string GetPropertyPath<T>(Expression<Func<T>> expression) {
            MemberExpression memberExpression = GetMemberExpression(expression);
            return GetPropertyPathCore(memberExpression);
        }
        static string GetPropertyPathCore(MemberExpression memberExpression) {
            string prefix = string.Empty;
            MemberExpression nextMemberExpression = memberExpression.Expression as MemberExpression;
            if(IsPropertyExpression(nextMemberExpression)) {
                prefix = GetPropertyPathCore(nextMemberExpression) + ".";
            }
            return prefix + memberExpression.Member.Name;
        }
        static bool IsPropertyExpression(MemberExpression expression) {
            return expression != null && expression.Member.MemberType == MemberTypes.Property;
        }
        static MemberExpression GetMemberExpression<T>(Expression<Func<T>> expression) {
            Guard.ArgumentNotNull(expression, "expression");
            Expression body = expression.Body;
            if(body is UnaryExpression) {
                body = ((UnaryExpression)body).Operand;
            }
            MemberExpression memberExpression = body as MemberExpression;
            if(memberExpression == null) {
                Guard.ArgumentException("expression");
            }
            return memberExpression;
        }
    }
}
