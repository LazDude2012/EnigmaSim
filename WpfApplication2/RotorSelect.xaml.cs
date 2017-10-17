/*
 * 
 * LazDude's EnigmaSim, (c) 2012 Alex Fischer.
 * RotorSelect class and Rotors class.
 *
 */
//Imports standard .NET framework libraries.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace LazDude2012.EnigmaSim
{
    /// <summary>
    /// Interaction logic for RotorSelect.xaml
    /// </summary>
    //The RotorSelect class is a Window.
    public partial class RotorSelect : Window
    {
        // These three are properties, meaning other classes can get their values and set them.
        public string LblLeft { get; set; }
        public string LblCentre { get; set; }
        public string LblRight { get; set; }
        // Creates a Rotors for data binding purposes.
        private Rotors rotors = new Rotors();
        // The result of the dialog.
        public Boolean isConfirmed;
        // Constructor: Any time a RotorSelect is created, it sets its DataContext to its rotors.
        public RotorSelect()
        {
            InitializeComponent();

            this.DataContext = rotors;
        }
        // These three properties are the ones that will store the selected rotors.
        public int SelectLeft { get; set; }
        public int SelectRight { get; set; }
        public int SelectCentre { get; set; }

        // Cancel button click method.
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // Cancels the window.
            isConfirmed = false;
            this.Hide();
        }

        // This is the method for when the OK button is clicked.
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Gets the value of the "LEFT" drop down box, and returns the number of the rotor.
            switch (rotors.Left)
            {
                case "Rotor I":
                    SelectLeft = 1;
                    break;
                case "Rotor II":
                    SelectLeft = 2;
                    break;
                case "Rotor III":
                    SelectLeft = 3;
                    break;
                case "Rotor IV":
                    SelectLeft = 4;
                    break;
                case "Rotor V":
                    SelectLeft = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //Gets the value of the "CENTRE" drop down box, and returns the number of the rotor.
            switch (rotors.Centre)
            {
                case "Rotor I":
                    SelectCentre = 1;
                    break;
                case "Rotor II":
                    SelectCentre = 2;
                    break;
                case "Rotor III":
                    SelectCentre = 3;
                    break;
                case "Rotor IV":
                    SelectCentre = 4;
                    break;
                case "Rotor V":
                    SelectCentre = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //Gets the value of the "RIGHT" drop down box, and returns the number of the rotor.
            switch (rotors.Right)
            {
                case "Rotor I":
                    SelectRight = 1;
                    break;
                case "Rotor II":
                    SelectRight = 2;
                    break;
                case "Rotor III":
                    SelectRight = 3;
                    break;
                case "Rotor IV":
                    SelectRight = 4;
                    break;
                case "Rotor V":
                    SelectRight = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //Gets the names of the selected rotors, for the labels in the main window.
            LblLeft = rotors.Left;
            LblCentre = rotors.Centre;
            LblRight = rotors.Right;
            //Confirms the changes and hides the window.
            isConfirmed = true;
            this.Hide();
        }

    }

    // This stores the selected rotors. INotifyPropertyChanged means that the Rotors class implements a few methods and events to let RotorSelect know when the values change.
    public class Rotors : INotifyPropertyChanged
    {
        //Constructor: It doesn't do anything, but without it, it's not a valid class for WPF Data Binding.
        public Rotors() { }

        // These are the private values (other classes can't see them) behind the public properties (which other classes can not only see, but modify).
        private string _left, _right, _centre;

        //These are the public properties that represent those three private fields.
        public string Left
        {
            get { return _left ; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    NotifyPropertyChanged("Left"); //This fires a PropertyChanged event to let RotorSelect know the property's value has changed.
                }
            }
        }

        public string Right 
        {
            get { return _right; }
            set
            {
                if (_right != value)
                {
                    _right = value;
                    NotifyPropertyChanged("Right"); //This fires a PropertyChanged event to let RotorSelect know the property's value has changed.
                }
            }
        }

        public string Centre
        {
            get { return _centre; }
            set
            {
                if (_centre != value)
                {
                    _centre = value;
                    NotifyPropertyChanged("Centre"); //This fires a PropertyChanged event to let RotorSelect know the property's value has changed.
                }
            }
        }

        //#region tags let me collapse sections of the code I don't really want to see.
        #region INotifyPropertyChanged Members

        //This is the method that actually fires the PropertyChanged event.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
