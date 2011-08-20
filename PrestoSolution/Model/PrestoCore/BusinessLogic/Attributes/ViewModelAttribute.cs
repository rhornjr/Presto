using System;

namespace PrestoCore.BusinessLogic.Attributes
{
    /// <summary>
    /// Provides a way to associate a View with its ViewModel
    /// </summary>
    public sealed class ViewModelAttribute : Attribute
    {
        /// <summary>
        /// The type of ViewModel to which the View is associated
        /// </summary>
        public Type ViewModelType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        public ViewModelAttribute( Type viewModelType )
        {
            this.ViewModelType = viewModelType;
        }
    }
}
