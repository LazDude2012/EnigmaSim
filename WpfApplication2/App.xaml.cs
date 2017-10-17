using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace LazDude2012.EnigmaSim
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

    //This is the simplest class possible. It merely has one public String property, which is the value it displays in the ComboBox.
    public class ComboBoxString
    {
        public ComboBoxString() { }
        public string ValueString { get; set; }
    }
}
