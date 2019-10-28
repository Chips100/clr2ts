namespace Clr2Ts.EndToEnd.Targets.SubTyping
{
    /// <summary>
    /// Example interface to be put on the example types.
    /// </summary>
    public interface IInterface1
    {
        /// <summary>
        /// Property from the base type.
        /// </summary>
        string BaseProperty1 { get; set; }

        /// <summary>
        /// Property from the derived class.
        /// </summary>
        string SubProperty1 { get; set; }
    }
}