using System;
using System.IO;
using System.Runtime.Remoting;
using Clr2Ts.Transpiler.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Clr2Ts.Transpiler.Tests.Input
{
    /// <summary>
    /// Tests for the AppDomainContext that manages access to a separate AppDomain.
    /// </summary>
    [TestClass]
    public class AppDomainContextTests
    {
        /// <summary>
        /// CreateProxyInstance should throw an <see cref="ObjectDisposedException"/>
        /// if the AppDomainContext has already been disposed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void AppDomainContext_CreateProxyInstance_ThrowsObjectDisposedException()
        {
            var sut = AppDomainContext.Create();
            sut.Dispose();
            sut.CreateProxyInstance<ProxyMockup>();
        }

        /// <summary>
        /// CreateProxyInstance should return a proxy that delegates calls
        /// to the underlying AppDomain.
        /// </summary>
        [TestMethod]
        public void AppDomainContext_CreateProxyInstance_CreatesProxyForAppDomain()
        {
            using (var sut = AppDomainContext.Create())
            {
                var proxy = sut.CreateProxyInstance<ProxyMockup>();
                proxy.SetStaticValue("Other");

                Assert.IsTrue(RemotingServices.IsTransparentProxy(proxy), "IsTransparentProxy");
                Assert.IsTrue(RemotingServices.IsObjectOutOfAppDomain(proxy), "IsObjectOutOfAppDomain");
                Assert.AreEqual("Default", ProxyMockup.SomeStaticValue, "SomeStaticValue in this AppDomain");
                Assert.AreEqual("Other", proxy.GetStaticValue(), "SomeStaticValue in other AppDomain");
            }
        }

        /// <summary>
        /// AddAssemblyResolveDirectory should throw an <see cref="ObjectDisposedException"/>
        /// if the AppDomainContext has already been disposed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void AppDomainContext_AddAssemblyResolveDirectory_ThrowsObjectDisposedException()
        {
            var sut = AppDomainContext.Create();
            sut.Dispose();
            sut.AddAssemblyResolveDirectory(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
        }

        /// <summary>
        /// AddAssemblyResolveDirectory should use the provided directory
        /// to resolve assemblies when loaded in the underlying AppDomain.
        /// </summary>
        [TestMethod]
        public void AppDomainContext_AddAssemblyResolveDirectory_LoadsAssembliesFromDirectory()
        {
            // TODO.
        }

        /// <summary>
        /// Disposing the AppDomainContext should unload the underlying AppDomain.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AppDomainUnloadedException))]
        public void AppDomainContext_Dispose_UnloadsAppDomain()
        {
            // Test this by trying to access a proxy from the unloaded AppDomain.
            ProxyMockup proxy;

            using (var sut = AppDomainContext.Create())
            {
                proxy = sut.CreateProxyInstance<ProxyMockup>();
            }

            proxy.SetStaticValue("Other");
        }

        private class ProxyMockup : MarshalByRefObject
        {
            public static string SomeStaticValue { get; private set; } = "Default";

            public void SetStaticValue(string value) => SomeStaticValue = value;
            public string GetStaticValue() => SomeStaticValue;
        }
    }
}