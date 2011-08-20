using System;
using System.Collections.Generic;

namespace Presto.MvvmTools
{
    internal class ViewMapper
    {
        private Dictionary<Type, Type> viewDictionary;

        /// <summary>
        /// 
        /// </summary>
        public ViewMapper()
        {
            viewDictionary = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="viewType"></param>
        public void Register(Type viewModelType, Type viewType)
        {
            viewDictionary.Add(viewModelType, viewType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        public void Unegister(Type viewModelType)
        {
            viewDictionary.Remove(viewModelType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public Type ResolveView(ViewModelBase viewModel)
        {
            return viewDictionary[viewModel.GetType()];
        }
    }
}
