using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using WinMVVM.Utils;

namespace WinMVVM.Design {
    public class BindingManagerCodeDomSerializer : CodeDomSerializer {
        public override object Serialize(IDesignerSerializationManager manager, object value) {
            CodeDomSerializer baseClassSerializer = (CodeDomSerializer)manager.
                GetSerializer(typeof(BindingManager).BaseType, typeof(CodeDomSerializer));

            object codeObject = baseClassSerializer.Serialize(manager, value);

            if(codeObject is CodeStatementCollection && value is BindingManager) {
                CodeStatementCollection statements = (CodeStatementCollection)codeObject;
                BindingManager bindingManager = (BindingManager)value;

                foreach(var action in bindingManager.GetActions()) {
                    CodeMethodInvokeExpression methodInvoke = GetSetBindingExpression(manager, bindingManager, action);
                    statements.Add(methodInvoke);
                }
            }
            return codeObject;
        }

        CodeMethodInvokeExpression GetSetBindingExpression(IDesignerSerializationManager manager, BindingManager bindingManager, WinMVVM.SetBindingAction action) {
            CodeExpression controlExpression = base.SerializeToExpression(manager, action.Control);
            if(controlExpression == null)
                return null;
            CodeExpression[] parameters = new CodeExpression[] { controlExpression, new CodePrimitiveExpression(action.StringProperty), GetBindingCreateExpression(action.Binding) };
            CodeExpression managerReferenceExpression = base.GetExpression(manager, bindingManager);
            return new CodeMethodInvokeExpression(managerReferenceExpression, "SetBinding", parameters);
        }
        static CodeObjectCreateExpression GetBindingCreateExpression(BindingBase binding) {
            return  new CodeObjectCreateExpression(typeof(Binding), new CodeExpression[] { new CodePrimitiveExpression(((Binding)binding).Path) });
        }


    }
}



// The code statement collection is valid, so add a comment.
//string commentText = "This comment was added to this object by a custom serializer.";
//CodeCommentStatement comment = new CodeCommentStatement(commentText);
//statements.Add(comment);

//string name = GetUniqueName(manager, new BooleanToVisibilityConverter());



//            CodeVariableDeclarationStatement variableDeclaration = new CodeVariableDeclarationStatement(
//typeof(IValueConverter),
//name,
//new CodeObjectCreateExpression(typeof(BooleanToVisibilityConverter), new CodeExpression[] { }));

//            statements.Insert(1, variableDeclaration);

//            CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression(
//new CodePropertyReferenceExpression(GetExpression(manager, value), "Resources"),
//"Add",
//new CodeExpression[] { new CodeVariableReferenceExpression(name) });
//            statements.Add(methodInvoke);

//            BindingManager bindingManager = (BindingManager)value;
//            if(bindingManager.Collection.Count > 0) {
//            methodInvoke = new CodeMethodInvokeExpression(
//new CodePropertyReferenceExpression(GetExpression(manager, value), "Resources"),
//"Add",
//new CodeExpression[] { GetExpression(manager, bindingManager.Collection[0].Control) });
//            statements.Add(methodInvoke);


//            methodInvoke = new CodeMethodInvokeExpression(
//new CodeTypeReferenceExpression(typeof(BindingOperations)),
//"SetBinding",
//new CodeExpression[] { GetExpression(manager, bindingManager.Collection[0].Control), new CodePrimitiveExpression("Text"), new CodeObjectCreateExpression(typeof(Binding), new CodeExpression[] { }) });
//            statements.Add(methodInvoke);

//            }

