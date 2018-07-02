using Ninject;
using Ninject.Syntax;
using System;
using System.Web;

namespace FabricWcf.DependencyInjection
{
    public static class IoC
    {
        public static IKernel Kernel { get; set; }

        public static IBindingNamedWithOrOnSyntax<T> InCustomScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(t => ProcessingScope.Current ?? (object)HttpContext.Current); //InRequestScope() is based on "HttpContext.Current"
        }
    }

    public class ProcessingScope : IDisposable
    {
        [ThreadStatic]
        private static ProcessingScope _currentScope = null;

        private static object lockObject = new object();
        private string _scopeId = null;

        /* Scope can only be created using static CreateNew() */

        private ProcessingScope()
        {
            _scopeId = Guid.NewGuid().ToString();
        }

        private static ProcessingScope SetCurrentScope(ProcessingScope value)
        {
            lock (lockObject) { return (_currentScope = value); }
        }

        public static ProcessingScope CreateNew()
        {
            return SetCurrentScope(new ProcessingScope());
        }

        public static ProcessingScope Current
        {
            get
            {
                lock (lockObject) { return _currentScope; }
            }
        }

        public void Dispose()
        {
            SetCurrentScope(null);
            GC.SuppressFinalize(this);
        }
    }
}