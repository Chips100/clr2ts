namespace Clr2Ts.EndToEnd.Targets.Generics
{
    /// <summary>
    /// Generic Type with constraints on its type parameters.
    /// </summary>
    /// <typeparam name="T1">Type that is constrained to be a value type.</typeparam>
    /// <typeparam name="T2">Type that is constrained to be a reference type with a default constructor.</typeparam>
    /// <typeparam name="T3">Type that is constrained to be a subtype of other types and have a default constructor.</typeparam>
    /// <typeparam name="T4">Type with no constraints.</typeparam>
    public interface IGenericInterfaceWithConstraints<T1, T2, T3, T4>
        where T1 : struct
        where T2 : class, new()
        where T3 : ExampleClass1, IConstraintInterface, new()
    { }
}
