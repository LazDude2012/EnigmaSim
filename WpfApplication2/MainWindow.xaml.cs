/*
 * 
 *      LazDude's EnigmaSim, (c) 2012 Alex Fischer.
 *      MainWindow class.
 * 
 */


//Import the standard .NET class libraries.
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
using System.Windows.Navigation;
using System.Windows.Shapes;

//This just lets me sort all my code better. :)
namespace LazDude2012.EnigmaSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Create the class that does the actual Enigma-ing.
        public Enigma enigma1;
        // Creates the rotor selection dialog box, but doesn't show it.
        RotorSelect rtrPicker = new RotorSelect();
        int keypresses = 0;

        // This method is called whenever an instance of the MainWindow class is created.
        public MainWindow()
        {
            InitializeComponent();
            
            //Initialises the Enigma simulator in its default configuration: rotors 1,2,and 3 in the left, centre, and right slots respectively.
            enigma1 = new Enigma(1, 2, 3);

            //This sets the DataContext of the window to enigma1, which is required for WPF Data Binding.
            this.DataContext = enigma1;
        }

        // This method is called any time the "Increase" button over the right rotor display is clicked.
        private void increaseRight_Click(object sender, RoutedEventArgs e)
        {
            //Rotate the right rotor (the one in Slot 3) 1 space forward.
            enigma1.Rotate(3, 1);
        }

        //This method is called whenever the "Decrease" button under the right rotor display is clicked.
        private void decreaseRight_Click(object sender, RoutedEventArgs e)
        {
            //Rotate the right rotor (the one in Slot 3) 25 spaces forward, in lieu of a backwards rotation method.
            enigma1.Rotate(3, 25);
        }

        //Same as above, but for the centre rotor (in Slot 2).
        private void increaseCentre_Click(object sender, RoutedEventArgs e)
        {
            enigma1.Rotate(2, 1);
        }

        private void decreaseCentre_Click(object sender, RoutedEventArgs e)
        {
            enigma1.Rotate(2, 25);
        }

        //Same as above, but for the right rotor (in Slot 3).
        private void increaseLeft_Click(object sender, RoutedEventArgs e)
        {
            enigma1.Rotate(1, 1);
        }

        private void decreaseLeft_Click(object sender, RoutedEventArgs e)
        {
            enigma1.Rotate(1, 25);
        }

        //This method intercepts any key presses made while the EnigmaSim window has focus.
        private void EnigmaSim_KeyDown(object sender, KeyEventArgs e)
        {
            //Number of characters encoded; used for formatting.
            ++keypresses;
            //Gets the key's name as text.
            string key = e.Key.ToString();
            
            //for later :)
            string endtext = null;

            //This foreach loop checks the key against the whole alphabet to see what it is.
            foreach (char j in Enigma.alphabet)
            {
                if (key == j.ToString())
                {
                    //If the key matches a letter, encrypt that letter into the endtext string.
                    endtext = enigma1.EncryptLetter(j).ToString();

                    //Display the encrypted letter in the indicator light box.
                    indicatorLight.Text = endtext;
                }
            }

            

            //This is here because there are more keys on the keyboard than just alphabet keys. It keeps the display from being messy.
            if (endtext != null)
            {
                //Number of characters encoded; used for formatting.
                ++keypresses;
                //Put the letter that was typed in the top text box, and the encrypted letter in the bottom one.
                txtboxPlaintext.AppendText(key);
                txtboxCiphertext.AppendText(endtext);
                //If the number of keys encoded is divisible by 5, add a space for formatting.
                if(keypresses % 5 == 0)
                {
                    txtboxPlaintext.AppendText(" ");
                    txtboxCiphertext.AppendText(" ");
                }
            }
        }

        //This method is called any time the "Rotors..." button is clicked.
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            //Shows the "Rotor Select" window.
            rtrPicker.ShowDialog();

            //Check to see if the user clicked "Confirm", and if so, reload the Enigma with the new rotors picked by the user.
            if (rtrPicker.isConfirmed)
            {
                enigma1.InitialiseRotors(rtrPicker.SelectLeft, rtrPicker.SelectCentre, rtrPicker.SelectRight);

                //This is to set the indicators back to "AAA".
                enigma1.Rotate(1, 26);
                enigma1.Rotate(2, 26);
                enigma1.Rotate(3, 26);
            }

            //Lets the user know the rotors they picked are actually the ones being used.
            leftIndicator.Text = rtrPicker.LblLeft;
            centreIndicator.Text = rtrPicker.LblCentre;
            rightIndicator.Text = rtrPicker.LblRight;
        }

        //This is called when the text on the button is clicked. All it does is call Button7_Click.
        //This way, the button works even if you happen to click a letter instead of the actual button.
        private void RotorsLabel_Click(object sender, MouseButtonEventArgs e)
        {
            Button7_Click(sender, e);
        }

        //When the main EnigmaSim window is closed, get rid of the "Rotor Select" dialog for good.
        private void EnigmaSim_Closed(object sender, EventArgs e)
        {
            rtrPicker.Close();
        }

        //This is called when the "Reset Simulation" button is clicked.
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Reloads the default rotors.
            enigma1.InitialiseRotors(1, 2, 3);
            
            //Sets the rotor indicators to AAA.
            enigma1.Rotate(1, 26);
            enigma1.Rotate(2, 26);
            enigma1.Rotate(3, 26);

            //Clears the text out of the indicator box.
            indicatorLight.Text = "";

            //Erases the message, both in its encrypted and unencrypted forms.
            txtboxPlaintext.Clear();
            txtboxCiphertext.Clear();
        }

    }
}
