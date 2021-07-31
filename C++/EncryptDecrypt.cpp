// Zachary Tennant

#include<iostream>
#include<fstream>
using namespace std;

void encrypt();
void writeToFile(char ciphertext[], int size);
void decrypt();
float getIOC(char ciphertext[], int size);
float getKeywordLen(int size, float ioc);
void getKeyword(int keywordLen, char keyword[], char ciphertext[], int size, int numLetters[]);
char findMostFreqChar(char ciphertext[], int size, int keywordCharPos, int keywordLen, int numLetters[]);
void getPlaintext(int size, char plaintext[], char ciphertext[], char keyword[], int keywordLen);
void display(float ioc, int numLetters[], char keyword[], int keywordLen, char plaintext[]);
void interact(char plaintext[], int size, int keywordLen, char keyword[], char ciphertext[], int numLetters[], float ioc);
void writeToPlainFile(char plaintext[], int size);

int main()
{
	string choice;

// set choice to default
	choice = "";

// continue asking until a valid choice is made
	while(choice != "Encrypt" || choice != "encrypt" ||
		choice != "Decrypt" || choice != "decrypt" ||
		choice != "Exit" || choice != "exit")
	{

// prompt for encrypting or decrypting
		cout << "Encrypt or Decrypt or Exit? ";
		cin >> choice;
		cout << endl;

// validate choice
		if(choice == "Encrypt" || choice == "encrypt")
		{
			encrypt();
			return 0;
		}

		else if(choice == "Decrypt" || choice == "decrypt")
		{
			decrypt();
			return 0;
		}

		else if(choice == "Exit" || choice == "exit")
			return 0;

		else
			cout << "Invalid response" << endl;
	}
} // end of program

//===============================================

void encrypt()
{
	ifstream inFile;
	char fileName[20];
	char keyword[20];
	char ciphertext[25000];
	int keywordLen;
	int i;
	char c;
	int size;
	string response;

// get file name
	cout << "Enter plaintext filename: ";
	cin >> fileName;

// get keyword
	cout << "Enter the keyword: ";
	cin >> keyword;
	cout << endl;

// get length of keyword
	keywordLen = strlen(keyword);

// open file
	inFile.open(fileName);
	if(!inFile)
	{
		cout << "Error opening plaintext file";
		exit(0);
	}

	else
	{
		i = 0;

// loop through the plaintext to encrypt
		while(!inFile.eof())
		{
			if(inFile.peek() == EOF)
				break;

// get character
			inFile.get(c);

// find ciphertext
			if(c > ' ')
			{
// convert uppercase to lowercase
				if(c >= 'A' && c <= 'Z')
					c += ('a' - 'A');

				ciphertext[i] = (c - 'a' + keyword[i % keywordLen] - 'a') % 26 + 'a';

				i++;
			}
		}
		size = i;
	}
// close file
	inFile.close();

	response = "";

	while(response != "file" || response != "see")
	{
// write to file or display text
		cout << "Create ciphertext file (file) or see ciphertext on screen (see): ";
		cin >> response;

		if(response == "file")
		{
			writeToFile(ciphertext, size);
			break;
		}

		else if(response == "see")
		{
			for(int i = 0; i < size; i++)
				cout << ciphertext[i];

			break;
		}

		else
			cout << "Invalid response, either enter file or see" << endl;
	}
}

//===============================================

void writeToFile(char ciphertext[], int size)
{
	ofstream outFile;
	char fileName[20];

// get filename
	cout << "Enter ciphertext filename: ";
	cin >> fileName;

// open file
	outFile.open(fileName);
	if(!outFile)
	{
		cout << "Error opening ciphertext file";
		exit(0);
	}

// write to file
	else
	{
		for(int i = 0; i < size; i++)
			outFile << ciphertext[i];
	}

// close file
	outFile.close();
}

//===============================================

void decrypt()
{
	ifstream inFile;
	char fileName[20];
	char ciphertext[25000];
	int i;
	char c;
	int size;
	float ioc;
	int keywordLen;
	char keyword[20];
	char plaintext[25000];
	string response;
	int numLetters[26] = {0};

// get filename
	cout << "Enter ciphertext filename: ";
	cin >> fileName;

// open file
	inFile.open(fileName);
	if(!inFile)
	{
		cout << "Error opening ciphertext file";
		exit(0);
	}

// get ciphertext from file
	else
	{
		i = 0;

		while(!inFile.eof())
		{
			if(inFile.peek() == EOF)
				break;

// get character
			inFile.get(c);

// get ciphertext
			if(c != ' ' && c != '\n')
			{

// convert uppercase to lowercase
				if(c >= 'A' && c <= 'Z')
					c += ('a' - 'A');

				ciphertext[i] = c;
				i++;
			}
		}
		cout << endl;
		size = i;
	}
// close file
	inFile.close();

// get index of coincidence
	ioc = getIOC(ciphertext, size);

// get keyword length
	keywordLen = getKeywordLen(size, ioc);

// get keyword
	getKeyword(keywordLen, keyword, ciphertext, size, numLetters);

// get plaintext mapped to each keyword character
	getPlaintext(size, plaintext, ciphertext, keyword, keywordLen);

// display the ioc, char frequencies, keyword, and plaintext
	display(ioc, numLetters, keyword, keywordLen, plaintext);

// interact with the program
	interact(plaintext, size, keywordLen, keyword, ciphertext, numLetters, ioc);
}

//===============================================

float getIOC(char ciphertext[], int size)
{
	int numLetters[27] = {0};
	float sum;

	sum = 0;

// get # of each letter in ciphertext
	for(int k = 0; k < size; k++)
		numLetters[ciphertext[k] - 'a']++;

// numerator of ioc equation
	for(int k = 0; k < 26; k++)
		sum += numLetters[k] * (numLetters[k] - 1);

// return index of coincidence
	return sum / (size * (size - 1));
}

//===============================================

float getKeywordLen(int size, float ioc)
{
	float numerator;
	float denominator;

// get numerator
	numerator = 0.027 * size;

// get denominator
	denominator = (size - 1) * ioc - 0.038 * size + 0.065;

// return estimated length of keyword
	return numerator / denominator;
}

//===============================================

void getKeyword(int keywordLen, char keyword[], char ciphertext[], int size, int numLetters[])
{
	int keywordCharPos = 0;
	char mostFreqChar;

	for(int k = 0; k < keywordLen; k++)
	{

// find the most frequent character mapped to first letter in keyword
		mostFreqChar = findMostFreqChar(ciphertext, size, keywordCharPos, keywordLen, numLetters);

// get keyword
		keyword[k] = (mostFreqChar - 'a' - 5 + 27) % 26 + 'a';

		keywordCharPos++;
	}
}

//===============================================

char findMostFreqChar(char ciphertext[], int size, int keywordCharPos, int keywordLen, int numLetters[])
{
	int largestSoFar;
	int posOfLargest;
	char mostFreqChar;

	largestSoFar = 0;

// get # of each letter in positions mapped to
	for(int k = keywordCharPos; k < size; k += keywordLen)
		numLetters[ciphertext[k] - 'a']++;

// find most frequent character
	for(int k = 0; k < 26; k++)
	{
		if(numLetters[k] > largestSoFar)
		{
			largestSoFar = numLetters[k];
			posOfLargest = k;
		}
	}

// get most frequent character
	mostFreqChar = posOfLargest + 'a';

// return most frequent character
	return mostFreqChar;
}

//===============================================

void getPlaintext(int size, char plaintext[], char ciphertext[], char keyword[], int keywordLen)
{
	int keywordCharPos = 0;

	//char keyword2[20] = "catdog";
	//int keywordLen2 = 6;

// get plaintext
	for(int i = keywordCharPos; i < size; i++)
	{
		plaintext[i] = (ciphertext[i] - 'a' - (keyword[keywordCharPos] - 'a') + 26) % 26 + 'a';
		keywordCharPos = (keywordCharPos + 1) % keywordLen;
	}
}

//===============================================

void display(float ioc, int numLetters[], char keyword[], int keywordLen, char plaintext[])
{
	char alphabet = 'a';

// display the index of coincidence
	cout << "Index of Coincidence: " << ioc << endl;

// display the character frequency
	cout << "                                ";
	for(int k = 0; k < 26; k++)
	{
		cout << alphabet << " ";
		alphabet += 1;
	}
	cout << endl;

	cout << "Ciphertext Character Frequency: ";
	for(int k = 0; k < 26; k++)
		cout << numLetters[k] << " ";
	cout << endl;

// display the keyword length
	cout << "Keyword Length: " << keywordLen << endl;

// display the keyword
	cout << "Keyword: ";
	for(int k = 0; k < keywordLen; k++)
		cout << keyword[k];
	cout << endl;

// display part of the plaintext
	cout << "Plaintext: ";
	for(int k = 0; k < 15; k++)
		cout << plaintext[k];
	cout << endl;
}

//===============================================

void interact(char plaintext[], int size, int keywordLen, char keyword[], char ciphertext[], int numLetters[], float ioc)
{
// create menu items
	string response = "";

// keep running through the interaction until they decide to quit
	while(response != "quit")
	{
// menu of options
		cout << "Do you want to write to a file? (write)" << endl;
		cout << "Do you want to quit? (quit)" << endl;
		cout << "Do you want to change the key length? (length)" << endl;
		cout << "Do you want to change the keyword? (keyword)" << endl;
		cout << "What would you like to do?: ";
		cin >> response;

// write to a file
		if(response == "write")
			writeToPlainFile(plaintext, size);

// quit
		else if(response == "quit")
			exit(0);

// change the key length
		else if(response == "length")
		{
			cout << "What would you like the key length to be?: ";
			cin >> keywordLen;

// use the new key length to get and show the new keyword and plaintext
			getKeyword(keywordLen, keyword, ciphertext, size, numLetters);
			getPlaintext(size, plaintext, ciphertext, keyword, keywordLen);
			display(ioc, numLetters, keyword, keywordLen, plaintext);
		}

// change the keyword
		else if(response == "keyword")
		{
			cout << "What would you like the keyword to be?: ";
			cin >> keyword;

// get the new keyword length
			keywordLen = strlen(keyword);

// get rid of capital letters in the new keyword
			for(int k = 0; k < keywordLen; k++)
			{
				if(keyword[k] >= 'A' && keyword[k] <= 'Z')
					keyword[k] += ('a' - 'A');
			}

// use the new keyword to get and show the new plaintext
			getPlaintext(size, plaintext, ciphertext, keyword, keywordLen);
			display(ioc, numLetters, keyword, keywordLen, plaintext);
		}

		else
			cout << "Invalid option!" << endl;
	}
}

//===============================================

void writeToPlainFile(char plaintext[], int size)
{
	ofstream outFile;
	char fileName[20];

// get filename
	cout << "Enter plaintext filename: ";
	cin >> fileName;

// open file
	outFile.open(fileName);
	if(!outFile)
	{
		cout << "Error opening plaintext file";
		exit(0);
	}

// write to file
	else
	{
		for(int i = 0; i < size; i++)
			outFile << plaintext[i];
	}

// close file
	outFile.close();
}