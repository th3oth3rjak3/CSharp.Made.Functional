namespace Functional.Types;

/// <summary>
/// Define how a series of Tasks should be processed.
/// </summary>
public enum ProcessingOrder
{
    /// <summary>
    /// Process a collection of tasks in Sequential order.
    /// </summary>
    Sequential = 0,
    /// <summary>
    /// Process a collection of tasks in Parallel.
    /// </summary>
    Parallel = 1
}
