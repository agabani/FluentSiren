namespace FluentSiren.Builders
{
    public abstract class Builder<TBuilder, TEntity>
        where TBuilder : Builder<TBuilder, TEntity>
        where TEntity : class
    {
        protected TBuilder This;

        protected Builder()
        {
            This = (TBuilder) this;
        }

        public abstract TEntity Build();
    }
}