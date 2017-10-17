# EnigmaSim
A short and simple simulation of the Enigma code machine, used by the Germans in World War II to encrypt their troop movements.
This encryption device was the catalyst for one of the first uses of general purpose computing: Colossus, a code-breaking computer developed with the aid of Alan Turing.

## How does it work?

This Enigma simulator closely mimics the real machine's use of rotating wires that transpose one value with another, using integer arrays. Instead of a wheel composed of 26 wire pairs, each leading to a position on the left side of the wheel different from the position on the right from which it entered, EnigmaSim uses the index of an integer array as the position on the right side at which the "signal" enters. The integer at that position is used as the position on the left side at which the "signal" departs. 
The mechanism of the real machine repeats this process three times, and so does the simulator; the value from the first array is used as the index to look up in the second, and so on for the third, and back through, another three times.
In a real Enigma machine, the rotors rotate with the mechanical action of a keypress, and in the simulator, the furthest right array is rotated with a simple loop, every keypress, and the other two at specified offsets.

This takes the plain text in the screenshot below, "Three kilometres of ground lost to Canadian forces overnight. Four divisions and three armored squads have been lost. Including this one. Hello from Edmonton." and turns it into the cipher text "CMACJ WYQBU UDJTD GNBEM KSPQD LSWET DOKKY QKVZU SDMFD BGHLS WQPEV MPEEO TNKYU LZYJF CYISG ONUEO SNBAB FDMFV NCHKO SIKOA WXDYY AAYIW BVTMP OWJYQ PFKNZ HJTOZ NWJYY HGEKJ KXWCQ FZWNJ", significantly less intelligible.
To confirm this, simply load up the program, set the rotors to IV, II, III, setting IWW, and put in the cipher as shown. Enigma encryption is symmetric, and the same plain text should appear.

![Screenshot](https://github.com/LazDude2012/EnigmaSim/blob/master/screenshot-inuse.JPG?raw=true)


