# WeightedTree

First you need it's create a tree, for this you can either load test data or write it manually, to do this you need to click on the appropriate buttons. 
If any error occurs during processing of the loaded data, for example: the absence of any element of the symbol-frequency pair, the frequency representation is not in an integer format, the content of two pairs with the same symbols, or the tree contains less than two elements. In this case, a corresponding message will be displayed and the test1.txt file will be automatically loaded. 

After creating a tree and the results of the software product appear on the screen, the following operations are possible for the tree: 
adding and deleting an element, decoding, saving tree data. 

In order to use the decoding procedure, you need to enter a string of 0 and 1 in the appropriate window; by default, this is the string “00011110 00 11111110 00010001”. 
Then, by clicking on the appropriate button, we get a string of characters corresponding to the entered Huffman code string. 
If some characters do not match the Huffman code for the tree in question, they will be ignored.