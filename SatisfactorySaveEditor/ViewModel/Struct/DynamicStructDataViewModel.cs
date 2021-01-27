﻿using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveEditor.ViewModel.Struct
{
    public class DynamicStructDataViewModel : ObservableObject
    {
        private readonly DynamicStructData model;
        public ObservableCollection<SerializedPropertyViewModel> Fields { get; }

        public DynamicStructDataViewModel(DynamicStructData dynamicStruct)
        {
            model = dynamicStruct;

            Fields = new ObservableCollection<SerializedPropertyViewModel>(dynamicStruct.Fields.Select(PropertyViewModelMapper.Convert));
        }

        public void ApplyChanges()
        {
            model.Fields.Clear();
            foreach (var field in Fields)
            {
                field.ApplyChanges();
                model.Fields.Add(field.Model);
            }
        }
    }
}
