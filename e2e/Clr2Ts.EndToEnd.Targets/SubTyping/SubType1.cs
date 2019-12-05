namespace Clr2Ts.EndToEnd.Targets.SubTyping
{
    /// <summary>
    /// Class that is derived from a base class.
    /// </summary>
    public sealed class SubType1 : BaseType1, IInterface1
    {
        /// <summary>
        /// Property from the derived class.
        /// </summary>
        public string SubProperty1 { get; set; }
    }
}