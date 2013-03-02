using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Utils {
    public static class ExpressionHelper {
        public static string GetPropertyName<T>(Expression<Func<T>> expression) {
            Guard.ArgumentNotNull(expression, "expression");
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if(memberExpression == null) {
                Guard.ArgumentException("expression");
            }
            return memberExpression.Member.Name;
        }
    }
}
