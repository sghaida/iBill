using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CCC.ORM.DataAttributes;

namespace CCC.ORM.DataAccess
{
    public class CustomExpressionVisitor : ExpressionVisitor
    {
        private StringBuilder sb;
        private string _orderBy = string.Empty;
        private int? _skip = null;
        private int? _take = null;
        private string _toUpper = string.Empty;
        private string _toLower = string.Empty;
        private string _whereClause = string.Empty;

        public int? Skip
        {
            get
            {
                return _skip;
            }
        }

        public int? Take
        {
            get
            {
                return _take;
            }
        }

        public string OrderBy
        {
            get
            {
                return _orderBy;
            }
        }

        public string ToUpper
        {
            get
            {
                return _toUpper;
            }
        }

        public string ToLower
        {
            get
            {
                return _toLower;
            }
        }

        public string WhereClause
        {
            get
            {
                return _whereClause;
            }
        }

        public CustomExpressionVisitor() 
        {
            sb = new StringBuilder();
        }

        public string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);
            
            return this.sb.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                this.Visit(m.Arguments[0]);
                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;
            }
            else if (m.Method.Name == "Take")
            {
                if (this.ParseTakeExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "Skip")
            {
                if (this.ParseSkipExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderBy")
            {
                if (this.ParseOrderByExpression(m, "ASC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderByDescending")
            {
                if (this.ParseOrderByExpression(m, "DESC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "ToLower") 
            {
                if (this.ParseToLowerExpression(m, "LOWER"))
                {
                    return null;
                }
            }
            else if (m.Method.Name == "ToUpper")
            {   
                if (this.ParseToUpperExpression(m, "UPPER"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "ToDateTime") 
            {
                m.Method.Invoke(null,null);
            }
            else if (m.Method.Name == "Input") 
            {
                string x = string.Empty;
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.TypeAs:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS ");
                    }
                    else
                    {
                        sb.Append(" = ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS NOT ");
                    }
                    else
                    {
                        sb.Append(" <> ");
                    }
                    break;

                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));

            }

            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                sb.Append("NULL");
            }
            else if (q == null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                    default:
                        sb.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            string fieldName = GetMemberName(m);

            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                //sb.Append(m.Member.Name);
                sb.Append(fieldName);
                return m;
            }
            else if(m.Expression != null && m.Expression.NodeType == ExpressionType.Constant)
            {  
                var value = GetValue(m);

                if (m.Type == typeof(string) || m.Type == typeof(char))
                {
                    sb.Append("'" + value + "'");
                }
                else if (m.Type == typeof(DateTime)) 
                {
                    sb.Append("'" + Convert.ToDateTime(value).ToString("yyyy-MM-dd hh:mm:ss.fff") + "'");
                }
                else
                {
                    sb.Append(value);
                }

                return null;
            }
            else if (m.Expression != null && m.Expression.NodeType == ExpressionType.MemberAccess) 
            {

                var value = GetValue(m);

                if (m.Type == typeof(string) || m.Type == typeof(char))
                {
                    sb.Append("'" + value + "'");
                }
                else
                {
                    sb.Append(value);
                }

                return null;

            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        
        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }


        private bool ParseOrderByExpression(MethodCallExpression expression, string order)
        {
            string fieldName = GetMemberName(expression);
            UnaryExpression unary = (UnaryExpression)expression.Arguments[1];
            LambdaExpression lambdaExpression = (LambdaExpression)unary.Operand;

            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            MemberExpression body = lambdaExpression.Body as MemberExpression;

            if (body != null)
            {
                if (string.IsNullOrEmpty(_orderBy))
                {
                    //_orderBy = string.Format("{0} {1}", body.Member.Name, order);
                    _orderBy = string.Format("{0} {1}", fieldName, order);
                }
                else
                {
                    //_orderBy = string.Format("{0}, {1} {2}", _orderBy, body.Member.Name, order);
                    _orderBy = string.Format("{0}, {1} {2}", _orderBy, fieldName, order);
                }

                return true;
            }

            return false;
        }


        private bool ParseSkipExpression(MethodCallExpression expression)
        {
            string fieldName = GetMemberName(expression);
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _skip = size;
                return true;
            }

            return false;
        }


        private bool ParseTakeExpression(MethodCallExpression expression)
        {
            string fieldName = GetMemberName(expression);
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _take = size;
                return true;
            }

            return false;
        }


        private bool ParseToUpperExpression(MethodCallExpression expression, string toUpper)
        {
            string fieldName = GetMemberName(expression);

            _toUpper = string.Format("{0}({1})", toUpper, fieldName);
            sb.Append(_toUpper);

            return true;
        }


        private bool ParseToLowerExpression(MethodCallExpression expression, string toLower)
        {
            string fieldName = GetMemberName(expression);

            _toLower = string.Format("{0}({1})", toLower, fieldName);
            sb.Append(_toLower);
           
            return true;
        }
        

        private string GetMemberName(MethodCallExpression expression)
        {
            string fieldName = string.Empty;
            var member = ((MemberExpression)expression.Object).Member;

            if (member != null && member.CustomAttributes != null && member.CustomAttributes.Count() > 0)
            {
                var dbColumn = member.CustomAttributes.FirstOrDefault(item => item.AttributeType == typeof(DbColumnAttribute));

                if (dbColumn != null && dbColumn.ConstructorArguments.Count > 0)
                {
                    fieldName = Convert.ToString(dbColumn.ConstructorArguments.First().Value);
                }
                else
                {
                    fieldName = member.Name;
                }
            }
            else
            {
                fieldName = member.Name;
            }

            return fieldName;
        }


        private string GetMemberName(MemberExpression expression)
        {
            string fieldName = string.Empty;
            var member = expression.Member;

            if (member != null && member.CustomAttributes != null && member.CustomAttributes.Count() > 0)
            {
                var dbColumn = member.CustomAttributes.FirstOrDefault(item => item.AttributeType == typeof(DbColumnAttribute));

                if (dbColumn != null && dbColumn.ConstructorArguments.Count > 0)
                {
                    fieldName = Convert.ToString(dbColumn.ConstructorArguments.First().Value);
                }
                else
                {
                    fieldName = member.Name;
                }
            }
            else
            {
                fieldName = member.Name;
            }

            return fieldName;
        }


        private object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }


        //protected override Expression VisitConstant(ConstantExpression node)
        //{

        //    sb.Append(node.Value);
        //    return node;
        //}

        //protected override Expression VisitParameter(ParameterExpression node)
        //{
        //    sb.Append(node.Name);
        //    return node;
        //}

    }
}
