using System.Linq.Expressions;

namespace Al.Components.QueryableFilterExpression
{
    /// <summary>
    /// Соотносит уникальное имя поля с выражением, определяющим его в типе
    /// </summary>
    /// <typeparam name="T">Тип, внутри которого определено поле</typeparam>
    public interface IFilterExpressionProperty<T>
        where T : class
    {
        string UniqueName { get; }
        public Expression<Func<T, object>> Expression { get; }
    }
}
