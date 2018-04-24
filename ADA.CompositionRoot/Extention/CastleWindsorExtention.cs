using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;

namespace ADA.CompositionRoot.CastleWindsor.Extentions
{
    /// <summary>
    /// Méthodes d'extensions pour CastleWindsor.
    /// </summary>
    public static class CastleWindsorExtension
    {
        /// <summary>
        /// Searches for the first interface found associated with the
        /// <see cref="ServiceDescriptor" /> which is not generic and which
        /// is found in the specified namespace.
        /// </summary>
        /// <param name="descriptor">The service descriptor.</param>
        /// <param name="interfaceNamespace">The interface's namespace.</param>
        /// <returns>The based on descriptor.</returns>
        public static BasedOnDescriptor FirstNonGenericCoreInterface(this ServiceDescriptor descriptor, List<Type> excludeType, params string[] interfaceNamespaces)
        {
            return descriptor.Select((type, baseType) =>
            {
                IEnumerable<Type> result = null;

                foreach (var interfaceNamespace in interfaceNamespaces)
                {
                    IEnumerable<Type> interfaces = type
                        .GetInterfaces()
                        .Where(t 
                            => {
                                return t.IsGenericType == false && t.Namespace.StartsWith(interfaceNamespace) && !excludeType.Contains(t);
                        }
                            ).Reverse();

                    

                    if (interfaces.Count() != 0)
                    {
                        result = interfaces;
                        break;
                    }
                }

                if (result == null)
                    return null;

                var element = result.FirstOrDefault();

                if (element == null)
                    return null;

                return new[] { element };
            });
        }

        /// <summary>
        /// Searches for the first interface found associated with the
        /// <see cref="ServiceDescriptor" /> which is not generic and which
        /// is found in the specified namespace.
        /// </summary>
        /// <param name="descriptor">The service descriptor.</param>
        /// <param name="interfaceNamespace">The interface's namespace.</param>
        /// <returns>The based on descriptor.</returns>
        public static BasedOnDescriptor FirstNonGenericCoreInterface(this ServiceDescriptor descriptor, params string[] interfaceNamespaces)
        {
            return descriptor.FirstNonGenericCoreInterface(new List<Type>(), interfaceNamespaces);
        }
    }
}