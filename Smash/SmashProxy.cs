using Castle.DynamicProxy;

namespace Smash
{
    internal class SmashProxy : IInterceptor
    {
        private readonly ICacher _cacher;

        public SmashProxy(ICacher cacher)
        {
            _cacher = cacher;
        }

        public void Intercept(IInvocation invocation)
        {
            var key = new Key(invocation.Method, invocation.Arguments);

            object cachedValue;

            if (_cacher.TryRetrieve(key, out cachedValue))
            {
                invocation.ReturnValue = cachedValue;
            }
            else
            {
                invocation.Proceed();
                _cacher.Save(key, invocation.ReturnValue);
            }
        }
    }
}
