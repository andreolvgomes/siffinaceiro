using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SIF
{
    public static class XmlXElement
    {
        public static object GetValueProperty<TObject>(this object obj, Expression<Func<TObject, object>> expression) where TObject : class, new()
        {
            PropertyInfo property = obj.GetType().GetProperty(obj.GetNameProperty(expression));
            object value = property.GetValue(obj, null);
            if (property.PropertyType.Name == "String")
            {
                if (value == null) return "";
            }
            return value;
        }

        public static string GetNameProperty<TObject>(this object obj, Expression<Func<TObject, object>> expression) where TObject : class, new()
        {
            Expression body = expression.Body;
            UnaryExpression convertExpression = body as UnaryExpression;
            if (convertExpression != null)
            {
                if (convertExpression.NodeType != ExpressionType.Convert)
                    throw new ArgumentException("Invalid property expression.", "exp");
                body = convertExpression.Operand;
            }
            return ((MemberExpression)body).Member.Name;
        }

        public static XElement GetXelementValue<TObject>(this object obj, Expression<Func<TObject, object>> expression) where TObject : class, new()
        {
            return new XElement(obj.GetNameProperty(expression), obj.GetValueProperty(expression));
        }

        public static String GetValueTag< TObject>(this XmlDocument xml, object obj, Expression<Func<TObject, object>> expression) where TObject : class, new()
        {
            string name = obj.GetNameProperty(expression);
            XmlNodeList node = xml.GetElementsByTagName(name);
            return node[0].InnerText;
        }
    }
}