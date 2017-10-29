using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Todo.Context;
using Todo.Web.Service.Interfaces;

namespace Todo.Web.Service.Services
{
    public enum OperatorComparer
    {
        Contains,
        StartsWith,
        EndsWith,
        Equals = ExpressionType.Equal,
        GreaterThan = ExpressionType.GreaterThan,
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        LessThan = ExpressionType.LessThan,
        LessThanOrEqual = ExpressionType.LessThan,
        NotEqual = ExpressionType.NotEqual
    }

    public class Service<TBase> : IService<TBase> where TBase : class
    {
        protected  TodoContext Context { get; set; }

        public Service(TodoContext context)
        {
            Context = context;
        }

        public IList<TBase> GetAll()
        {
            return Context.Set<TBase>().ToList();
        }

        public TBase GetSingle(int id)
        {
            ParameterExpression expression = Expression.Parameter(typeof(TBase), typeof(TBase).Name);
            Expression buildExpression = BuildCondition(expression, "Id", OperatorComparer.Equals, id);
            Expression<Func<TBase, bool>> predicate = (Expression<Func<TBase, bool>>) buildExpression;

            return Context.Set<TBase>().AsNoTracking().FirstOrDefault(predicate);
        }

        public async Task<TBase> Add(TBase item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            try
            {
                TBase newEntry = Context.Set<TBase>().Add(item);
                await Context.SaveChangesAsync();
                return newEntry;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> Update(TBase item)
        {
            int success = 0;

            try
            {
                success = await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return success;
            }

            return success;
        }

        public bool Remove(TBase item)
        {
            try
            {
                Context.Set<TBase>().Remove(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #region Extra Helping

        private static Expression BuildCondition(Expression parameter, string property, OperatorComparer comparer, object value)
        {
            PropertyInfo childProperty = parameter.Type.GetProperty(property);
            MemberExpression left = Expression.Property(parameter, childProperty);
            ConstantExpression right = Expression.Constant(value);
            Expression predicate = BuildComparison(left, comparer, right);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildComparison(Expression left, OperatorComparer comparer, Expression right)
        {
            List<OperatorComparer> mask = new List<OperatorComparer>{
                OperatorComparer.Contains,
                OperatorComparer.StartsWith,
                OperatorComparer.EndsWith
            };
            if (mask.Contains(comparer) && left.Type != typeof(string))
            {
                comparer = OperatorComparer.Equals;
            }
            return !mask.Contains(comparer) ? Expression.MakeBinary((ExpressionType)comparer, left, Expression.Convert(right, left.Type)) : BuildStringCondition(left, comparer, right);
        }

        private static Expression BuildStringCondition(Expression left, OperatorComparer comparer, Expression right)
        {
            MethodInfo compareMethod = typeof(string).GetMethods().Single(m => m.Name.Equals(Enum.GetName(typeof(OperatorComparer), comparer)) && m.GetParameters().Count() == 1);
            //we assume ignoreCase, so call ToLower on paramter and memberexpression
            MethodInfo toLowerMethod = typeof(string).GetMethods().Single(m => m.Name.Equals("ToLower") && m.GetParameters().Count() == 0);
            left = Expression.Call(left, toLowerMethod);
            right = Expression.Call(right, toLowerMethod);
            return Expression.Call(left, compareMethod, right);
        }

        private static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            ParameterVisitor resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            Expression resultParameter = resultParameterVisitor.Parameter;
            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }

        #endregion
    }
}
