using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Clr2Ts.Transpiler.Input
{
    /// <summary>
    /// Context for working in a seperate AppDomain.
    /// </summary>
    public sealed class AppDomainContext : IDisposable
    {
        private bool _disposed;
        private readonly AppDomain _appDomain = AppDomain.CreateDomain($"AssemblyReaderAppDomain_{Guid.NewGuid()}", null, new AppDomainSetup
        {
            // Use directory of current AppDomain to be able to use all types from this assembly.
            ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
        });

        // Private constructor to enfore usage of factory method.
        private AppDomainContext()
        { }

        /// <summary>
        /// Creates a new AppDomainContext.
        /// </summary>
        /// <returns>The newly created AppDomainContext.</returns>
        public static AppDomainContext Create() => new AppDomainContext();

        /// <summary>
        /// Creates a new proxy of the specified type that will delegate calls to an instance in this AppDomain.
        /// </summary>
        /// <typeparam name="T">Type of the instance to create in this AppDomain.</typeparam>
        /// <returns>A proxy that will delegate calls to the instance in this AppDomain.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if the AppDomainContext has already been disposed.</exception>
        public T CreateProxyInstance<T>() where T : MarshalByRefObject
        {
            EnsureNotDisposed();
            
            return (T)_appDomain.CreateInstanceAndUnwrap(
                typeof(T).Assembly.FullName,
                // ReSharper disable once AssignNullToNotNullAttribute
                // Cannot happen because T is constrained to be a valid type (MarshalByRefObject).
                typeof(T).FullName);
        }

        /// <summary>
        /// Configures this AppDomain to use the specified directory for resolving assemblies.
        /// </summary>
        /// <param name="directory">Directory that should be used for resolving assemblies.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="directory"/> is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the AppDomainContext has already been disposed.</exception>
        public void AddAssemblyResolveDirectory(DirectoryInfo directory)
        {
            EnsureNotDisposed();
            if (directory == null) throw new ArgumentNullException(nameof(directory));

            _appDomain.AssemblyResolve += new AssemblyFromDirectoryResolver(directory).LoadAssembly;
        }

        /// <summary>
        /// Disposes of this AppDomainContext by unloading the underlying AppDomain.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            AppDomain.Unload(_appDomain);
        }

        private void EnsureNotDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(AppDomainContext));
        }

        /// <summary>
        /// Resolver that looks in a specified directory for assemblies.
        /// </summary>
        /// <remarks>Serializable wrapper instead of simple lambda expression for usability in another AppDomain.</remarks>
        [Serializable]
        private sealed class AssemblyFromDirectoryResolver : ISerializable
        {
            private const string SerializationKeyDirectoryFullname = "DirectoryFullName";
            private readonly DirectoryInfo _directory;

            public AssemblyFromDirectoryResolver(DirectoryInfo directory)
            {
                _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            }

            // Constructor for deserialization.
            private AssemblyFromDirectoryResolver(SerializationInfo info, StreamingContext context)
            {
                if (info == null) throw new ArgumentNullException(nameof(info));

                _directory = new DirectoryInfo(info.GetString(SerializationKeyDirectoryFullname));
            }

            public Assembly LoadAssembly(object sender, ResolveEventArgs args)
            {
                // Look for the assembly file in the specified directory.
                var assemblyFilename = Path.Combine(_directory.FullName, $"{new AssemblyName(args.Name).Name}.dll");
                if (!File.Exists(assemblyFilename)) return null;

                return Assembly.LoadFrom(assemblyFilename);
            }

            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null) throw new ArgumentNullException(nameof(info));

                info.AddValue(SerializationKeyDirectoryFullname, _directory.FullName);
            }
        }
    }
}