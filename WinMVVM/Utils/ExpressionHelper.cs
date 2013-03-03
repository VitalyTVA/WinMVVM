using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Utils {
    static class ExpressionHelper {
        public static string GetPropertyName<T>(Expression<Func<T>> expression) {
            Guard.ArgumentNotNull(expression, "expression");
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if(memberExpression == null) {
                Guard.ArgumentException("expression");
            }
            MemberExpression nextMemberExpression = memberExpression.Expression as MemberExpression;
            if(IsPropertyExpression(nextMemberExpression)) {
                Guard.ArgumentException("expression");
            }
            return memberExpression.Member.Name;
        }

        public static string GetPropertyPath<T>(Expression<Func<T>> expression) {
            Guard.ArgumentNotNull(expression, "expression");
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if(memberExpression == null) {
                Guard.ArgumentException("expression");
            }

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
    }
}
