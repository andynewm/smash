using System;
using Castle.DynamicProxy;

namespace Smash
{
    public static class SmashFactory
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        public static T Smash<T>(this T concrete)
            where T : class
        {
            var proxy = (T) Generator.CreateInterfaceProxyWithTarget(
                typeof(T), concrete, new SmashProxy(new SimpleCacher()));

            return proxy;
        }
    }
}