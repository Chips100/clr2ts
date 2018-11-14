using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using Clr2Ts.Transpiler.Input;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Input
{
    /// <summary>
    /// Tests for the AppDomainContext that manages access to a separate AppDomain.
    /// </summary>
    public class AppDomainContextTests
    {
        /// <summary>
        /// CreateProxyInstance should throw an <see cref="ObjectDisposedException"/>
        /// if the AppDomainContext has already been disposed.
        /// </summary>
        [Fact]
        public void AppDomainContext_CreateProxyInstance_ThrowsObjectDisposedException()
        {
            Assert.Throws<ObjectDisposedException>(() =>
            {
                var sut = AppDomainContext.Create();
                sut.Dispose();
                sut.CreateProxyInstance<ProxyMockup>();
            });
        }

        /// <summary>
        /// CreateProxyInstance should return a proxy that delegates calls
        /// to the underlying AppDomain.
        /// </summary>
        [Fact]
        public void AppDomainContext_CreateProxyInstance_CreatesProxyForAppDomain()
        {
            using (var sut = AppDomainContext.Create())
            {
                var proxy = sut.CreateProxyInstance<ProxyMockup>();
                proxy.SetStaticValue("Other");

                Assert.True(RemotingServices.IsTransparentProxy(proxy), "IsTransparentProxy");
                Assert.True(RemotingServices.IsObjectOutOfAppDomain(proxy), "IsObjectOutOfAppDomain");
                Assert.Equal("Default", ProxyMockup.SomeStaticValue);
                Assert.Equal("Other", proxy.GetStaticValue());
            }
        }

        /// <summary>
        /// AddAssemblyResolveDirectory should throw an <see cref="ArgumentNullException"/>
        /// if null is passed as the directory to add.
        /// </summary>
        [Fact]
        public void AppDomainContext_AddAssemblyResolveDirectory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                using (var sut = AppDomainContext.Create())
                {
                    sut.AddAssemblyResolveDirectory(null);
                }
            });
        }

        /// <summary>
        /// AddAssemblyResolveDirectory should throw an <see cref="ObjectDisposedException"/>
        /// if the AppDomainContext has already been disposed.
        /// </summary>
        [Fact]
        public void AppDomainContext_AddAssemblyResolveDirectory_ThrowsObjectDisposedException()
        {
            Assert.Throws<ObjectDisposedException>(() =>
            {
                var sut = AppDomainContext.Create();
                sut.Dispose();
                sut.AddAssemblyResolveDirectory(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
            });
        }

        /// <summary>
        /// AddAssemblyResolveDirectory should use the provided directory
        /// to resolve assemblies when loaded in the underlying AppDomain.
        /// </summary>
        [Fact]
        public void AppDomainContext_AddAssemblyResolveDirectory_LoadsAssembliesFromDirectory()
        {
            using (var sut = AppDomainContext.Create())
            {
                sut.AddAssemblyResolveDirectory(SampleAssemblyInfo.LocationDirectory);

                var proxy = sut.CreateProxyInstance<ProxyMockup>();
                var result = proxy.LoadAssemblyByName(SampleAssemblyInfo.Name);
                
                // Check that the version of the assembly was determined correctly
                // and that it was not loaded in this AppDomain.
                Assert.Equal(SampleAssemblyInfo.Version , result);
                Assert.DoesNotContain(AppDomain.CurrentDomain.GetAssemblies(), a => a.FullName.Contains(SampleAssemblyInfo.Name));
            }
        }

        /// <summary>
        /// Disposing the AppDomainContext should unload the underlying AppDomain.
        /// </summary>
        [Fact]
        public void AppDomainContext_Dispose_UnloadsAppDomain()
        {
            Assert.Throws<AppDomainUnloadedException>(() =>
            {
                // Test this by trying to access a proxy from the unloaded AppDomain.
                ProxyMockup proxy;

                using (var sut = AppDomainContext.Create())
                {
                    proxy = sut.CreateProxyInstance<ProxyMockup>();
                }

                proxy.SetStaticValue("Other");
            });
        }

        private class ProxyMockup : MarshalByRefObject
        {
            public static string SomeStaticValue { get; private set; } = "Default";

            public void SetStaticValue(string value) => SomeStaticValue = value;
            public string GetStaticValue() => SomeStaticValue;

            public string LoadAssemblyByName(string assemblyName)
            {
                var assembly = Assembly.Load(assemblyName);
                return assembly.GetName().Version.ToString();
            }
        }
    }
}