using SchemeEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SchemeEditor.Views
{
    public partial class ChangeSchemeWindow : Window
    {
        private ChangeShemeViewModel _changeSchemeVM;
        public ChangeSchemeWindow()
        {
            _changeSchemeVM = new ChangeShemeViewModel();
            InitializeComponent();
            DataContext = _changeSchemeVM;
        }
    }
}
