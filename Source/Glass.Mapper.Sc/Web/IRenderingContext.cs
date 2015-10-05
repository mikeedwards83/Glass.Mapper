namespace Glass.Mapper.Sc.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRenderingContext
    {
       

         /// <summary>
         /// Indicates that the current rendering has a DataSource set
         /// </summary>
        bool HasDataSource { get; }

        /// <summary>
        /// Retrieves the Rendering parameters associated to the control
        /// </summary>
        /// <returns></returns>
        string GetRenderingParameters();

        /// <summary>
        /// Retrieves the DataSource associated to the control
        /// </summary>
        /// <returns></returns>
        string GetDataSource();
    }
}
