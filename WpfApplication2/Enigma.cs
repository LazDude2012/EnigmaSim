/*
 * 
 * LazDude's EnigmaSim, (c) 2012 Alex Fischer
 * Enigma simulation logic.
 * 
 */
//Imports standard .NET Framework libraries.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LazDude2012.EnigmaSim
{
    //Enigma has INotifyPropertyChanged to let MainWindow know the rotor positions have changed.
    public class Enigma : INotifyPropertyChanged
    {
        //These chars are the labels that MainWindow displays on the rotors. They start out in the AAA position.
        private char _leftLabel='A', _centreLabel='A', _rightLabel = 'A';
        //These store the alphabet and rotors I through V respectively. UKW is the reflector.
        public static char[] alphabet;
        int[] rotorI, rotorII, rotorIII, rotorIV, rotorV, UKW;
        // These arrays are the actual simulated rotors. The above arrays are copied into these in the InitialiseRotors method.
        int[] rtrLeft, rtrRight, rtrCentre, Reflector;
        //These store the rotation of each rotor, a.k.a. its position.
        public int leftRotation, rightRotation, centreRotation;
        //Each rotor advances the next at a different rotation, seemingly arbitrary. These values store those positions.
        int leftTurnover, centreTurnover, rightTurnover = 1;

        //The PropertyChanged event lets MainWindow know a property has changed.
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructor: when a new Enigma is created with 3 numbers, initialise it with the rotors those numbers represent.
        public Enigma(int rotorLeft, int rotorCentre, int rotorRight)
        {
            InitialiseRotors(rotorLeft, rotorCentre, rotorRight);
        }
        //Void Constructor: when a new Enigma is created without any data, initialise it with rotors I, II, and III.
        public Enigma()
        {
            new Enigma(1, 2, 3);
        }

        //This method fires a ProprtyChanged event for the property that calls it.
        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // Initialise the Enigma simulator with the specified rotors.
        public void InitialiseRotors(int left, int centre, int right)
        {
            /*
             *  HARD CODED ROTOR ARRAYS: These store the values of the rotors "in the box", so to speak.
             *  rtrLeft, rtrCentre, and rtrRight store the values of the rotors actually in the machine. 
             */

            // alphabet = "a=1,b=2,c=3,d=4,e=5,f=6,g=7,h=8,i=9,j=10,k=11,l=12,m=13,n=14,o=15,p=16,q=17,r=18,s=19,t=20,u=21,v=22,w=23,x=24,y=25,z=26"
            alphabet = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            rotorI = new int[26] { 5, 11, 13, 6, 12, 7, 4, 17, 22, 26, 14, 20, 15, 23, 25, 8, 24, 21, 19, 16, 1, 9, 2, 18, 3, 10 };
            //rotorI = "A=E,B=K,C=M,D=F,E=L,F=G,G=D,H=Q,I=V,J=Z,K=N,L=T,M=O,N=W,O=Y,P=H,Q=X,R=U,S=S,T=P,U=A,V=I,W=B,X=R,Y=C,Z=J"
            rotorII = new int[26] { 1, 10, 4, 11, 19, 9, 18, 21, 24, 2, 12, 8, 23, 20, 13, 3, 17, 7, 26, 14, 16, 25, 6, 22, 15, 5 };
            //rotorII = "A=A,B=J,C=D,D=K,E=S,F=I,G=R,H=U,I=X,J=B,K=L,L=H,M=W,N=T,O=M,P=C,Q=Q,R=G,S=Z,T=N,U=P,V=Y,W=F,X=V,Y=O,Z=E"
            rotorIII = new int[26] { 2, 4, 6, 8, 10, 12, 3, 16, 18, 20, 24, 22, 26, 14, 25, 5, 9, 23, 7, 1, 11, 13, 21, 19, 17, 15 };
            //rotorIII = "A=B,B=D,C=F,D=H,E=J,F=L,G=C,H=P,I=R,J=T,K=X,L=V,M=Z,N=N,O=Y,P=E,Q=I,R=W,S=G,T=A,U=K,V=M,W=U,X=S,Y=Q,Z=O"
            rotorIV = new int[26] { 5, 19, 15, 22, 16, 26, 10, 1, 25, 17, 21, 9, 18, 8, 24, 12, 14, 6, 20, 7, 11, 4, 3, 13, 23, 2 };
            //rotorIV = "A=E,B=S,C=O,D=V,E=P,F=Z,G=J,H=A,I=Y,J=Q,K=U,L=I,M=R,N=H,O=X,P=L,Q=N,R=F,S=T,T=G,U=K,V=D,W=C,X=M,Y=W,Z=B"
            rotorV = new int[26] { 22, 26, 2, 18, 7, 9, 20, 25, 21, 16, 19, 4, 14, 8, 12, 24, 1, 23, 13, 10, 17, 15, 6, 5, 3, 11 };
            //rotorV = "A=V,B=Z,C=B,D=R,E=G,F=I,G=T,H=Y,I=U,J=P,K=S,L=D,M=N,N=H,O=L,P=X,Q=A,R=W,S=M,T=J,U=Q,V=O,W=F,X=E,Y=C,Z=K"
            UKW = new int[26] { 17, 25, 8, 15, 7, 14, 5, 3, 22, 16, 21, 26, 20, 6, 4, 10, 1, 24, 23, 13, 11, 9, 19, 18, 2, 12 }; ;
            //UKW = "A=Q,B=Y,C=H,D=O,E=G,F=N,G=E,H=C,I=V,J=P,K=U,L=Z,M=T,N=F,O=D,P=J,Q=A,R=X,S=W,T=M,U=K,V=I,W=S,X=R,Y=B,Z=L"
            
            //Each of these switch blocks takes a number and copies the approprate rotor into the appropriate slot.
            // Left rotor.
            switch (left)
            {
                case 1:
                    rtrLeft = rotorI;
                    leftTurnover = 17;
                    break;
                case 2:
                    rtrLeft = rotorII;
                    leftTurnover = 5;
                    break;
                case 3:
                    rtrLeft = rotorIII;
                    leftTurnover = 22;
                    break;
                case 4:
                    rtrLeft = rotorIV;
                    leftTurnover = 11;
                    break;
                case 5:
                    rtrLeft = rotorV;
                    leftTurnover = 26;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("left", "The rotor selected must be between 1 and 5.");
                    break;
            }

            //Each of these switch blocks takes a number and copies the approprate rotor into the appropriate slot.
            // Centre rotor.
            switch (centre)
            {
                case 1:
                    rtrCentre = rotorI;
                    centreTurnover = 17;
                    break;
                case 2:
                    rtrCentre = rotorII;
                    centreTurnover = 5;
                    break;
                case 3:
                    rtrCentre = rotorIII;
                    centreTurnover = 22;
                    break;
                case 4:
                    rtrCentre = rotorIV;
                    centreTurnover = 11;
                    break;
                case 5:
                    rtrCentre = rotorV;
                    centreTurnover = 26;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("centre", "The rotor selected must be between 1 and 5.");
                    break;
            }

            //Each of these switch blocks takes a number and copies the approprate rotor into the appropriate slot.
            // Right rotor.
            switch (right)
            {
                case 1:
                    rtrRight = rotorI;
                    rightTurnover = 17;
                    break;
                case 2:
                    rtrRight = rotorII;
                    rightTurnover = 5;
                    break;
                case 3:
                    rtrRight = rotorIII;
                    rightTurnover = 22;
                    break;
                case 4:
                    rtrRight = rotorIV;
                    rightTurnover = 11;
                    break;
                case 5:
                    rtrRight = rotorV;
                    rightTurnover = 26;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("right", "The rotor selected must be between 1 and 5.");
                    break;
            }
            Reflector = UKW;
            // When new rotors are put in the machine, their positions are AAA.
            leftRotation = 1;
            rightRotation = 1;
            centreRotation = 1;
        }
        
        // Rotate the rotor in the specified slot by the given amount. Returns false on any error condition.
        public bool Rotate(int rotor, int offset)
        {
            //rtrTmp is used in a clever fashion later.
            int rtrTmp;
            // The Enigma simulated by this application only has 3 rotor slots.
            if (rotor > 3) return false;
            //Which slot are we rotating?
            switch (rotor)
            {
                    //Slot 1: Left rotor
                case 1:
                    /*
                     * This takes the rotor, and shifts all its values up by the specified amount. In other words, it rotates it.
                     */
                    for (int i = 0; i < offset; i++)
                    {
                        rtrTmp = rtrLeft[0];
                        for (int j = 0; j <= 25; j++)
                        {
                            if (j != 25)
                            {
                                rtrLeft[j] = rtrLeft[j + 1];
                            }
                            else { rtrLeft[j] = rtrTmp; }
                        }
                        leftRotation++;
                        if (leftRotation == 27) leftRotation = 1;
                        LeftLabel = alphabet[leftRotation - 1].ToString();
                    }
                    break;
                    
                    //Slot 2: Centre rotor
                case 2:
                    /*
                     * This takes the rotor, and shifts all its values up by the specified amount, simulating rotation.
                     */
                    for (int i = 0; i < offset; i++)
                    {
                        rtrTmp = rtrCentre[0];
                        for (int j = 0; j <= 25; j++)
                        {
                            if (j != 25)
                            {
                                rtrCentre[j] = rtrCentre[j + 1];
                            }
                            else { rtrCentre[j] = rtrTmp; }
                        }
                        centreRotation++;
                        if (centreRotation == 27) centreRotation = 1;
                        CentreLabel = alphabet[centreRotation - 1].ToString();
                    }
                    break;

                    //Slot 3: Right rotor
                case 3:
                    /*
                     * This takes the rotor and shifts all its values up by the specified amount. In other words, it rotates it.
                     */
                    for (int i = 0; i < offset; i++)
                    {
                        rtrTmp = rtrRight[0];
                        for (int j = 0; j <= 25; j++)
                        {
                            if (j != 25)
                            {
                                rtrRight[j] = rtrRight[j + 1];
                            }
                            else { rtrRight[j] = rtrTmp; }
                        }
                        rightRotation++;
                        if (rightRotation == 27) rightRotation = 1;
                        RightLabel = alphabet[rightRotation - 1].ToString();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("rotor", "The rotor must be left (1), centre (2), or right (3).");
                    break;
            }
            // Rotors rotated successfully! Let the calling method know.
            return true;
        }

        //Rotate rotates a specified rotor by a given amount. In contrast, RotateRotors simulates actual Enigma rotation.
        public void RotateRotors()
        {
            // The right rotor always shifts up one.
            Rotate(3, 1);
            
            // If any of the rotors reach their turnover point, shift them and the rotor directly to their left up one.
            if (rightRotation == rightTurnover)
            {
                Rotate(2, 1);
            }
            if (centreRotation == centreTurnover)
            {
                Rotate(2, 1);
                Rotate(1, 1);
            }
            if (leftRotation == leftTurnover)
            {
                Rotate(1, 1);
            }
        }

        //EncryptLetter passes a letter, plaintext, through the Enigma machine and returns its encrypted equivalent.
        public char EncryptLetter(char plaintext)
        {
            //Rotors are rotated BEFORE the letter goes through.
            RotateRotors();
            int tmp1 = Array.IndexOf(alphabet, plaintext);          // Convert the letter into a number by its position in the alphabet.
                                                                    // Arrays start from 0, but the rotor wirings start from 1; we must convert when necessary.
            int tmp2 = rtrRight[tmp1];                              // Both these factors are indices, therefore no conversion is required.
            int tmp3 = rtrCentre[tmp2 - 1];                         // tmp2 is a rotor wire; tmp2 - 1 is the equivalent index.
            int tmp4 = rtrLeft[tmp3 - 1];                           // tmp3 is a rotor wire; tmp3 - 1 is the equivalent index.
            int tmp5 = Reflector[tmp4 - 1];                         // tmp4 is a rotor wire; tmp3 - 1 is the equivalent index.
            int tmp6 = Array.IndexOf(rtrLeft, tmp5);                // tmp5 is a rotor wire, tmp6 is an index. Array.IndexOf() does the conversion for us.
            int tmp7 = Array.IndexOf(rtrCentre, tmp6 + 1);          // Converting from index to rotor wire means adding 1.
            int tmp8 = Array.IndexOf(rtrRight, tmp7 + 1);           // This is another conversion from index to rotor wire.
            return alphabet[tmp8];

        }
        // These are the 3 properties that MainWindow monitors: the labels on the rotors. Their values are changed in the Rotate method.
        // Each fires a PropertyChanged event when it is changed, letting MainWindow know to update the displayed value.
        public string LeftLabel
        {
            get
            {
                return _leftLabel.ToString();
            }
            set
            {
                _leftLabel = value.ToCharArray()[0];
                OnPropertyChanged("LeftLabel");
            }
        }
        public string CentreLabel
        {
            get
            {
                return _centreLabel.ToString();
            }
            set
            {
                _centreLabel = value.ToCharArray()[0];
                OnPropertyChanged("CentreLabel");
            }
        }
        public string RightLabel
        {
            get
            {
                return _rightLabel.ToString();
            }
            set
            {
                _rightLabel = value.ToCharArray()[0];
                OnPropertyChanged("RightLabel");
            }
        }
    }
}
