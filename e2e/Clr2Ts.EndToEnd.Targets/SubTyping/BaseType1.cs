namespace Clr2Ts.EndToEnd.Targets.SubTyping
{
    /// <summary>
    /// Class that serves as the base for other types.
    /// </summary>
    public abstract class BaseType1 : IBaseInterface1
    {
        /// <summary>
        /// Property from the base type.
        /// </summary>
        public string BaseProperty1 { get; set; }
    }
}