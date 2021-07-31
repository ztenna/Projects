// Zachary Tennant

#include<iostream>
#include<fstream>
using namespace std;

void encrypt();
void getEncryptPlaintext(char sourceFileName[], char plaintext[], int& size);
void getEncryptCiphertext(char plaintext[], int& size, char ciphertext[], int encryptMatrix[][2]);
void writeToCipherFile(char ciphertext[], int size);

void decrypt();
void getDecryptCiphertext(char sourceFileName[], char ciphertext[], int& size);
void getDecryptMatrix(int encryptMatrix[][2], int decryptMatrix[][2]);
int findReciprocal(int det);
void getDecryptPlaintext(char ciphertext[], int& size, char plaintext[], int decryptMatrix[][2]);
void writeToPlainFile(char plaintext[], int size);

int main()
{
	string choice;

// set choice to default
	choice = "";

// continue asking until a valid choice is made
	while(choice != "encrypt" && choice != "Encrypt" &&
			choice != "decrypt" && choice != "Decrypt" &&
			choice != "exit" && choice != "Exit")
	{

// prompt for encrypting or decrypting
		cout << "Encrypt or Decrypt or Exit? ";
		cin >> choice;
		cout << endl;

// validate choice
		if(choice == "Encrypt" || choice == "encrypt")
			encrypt();

		else if(choice == "Decrypt" || choice == "decrypt")
			decrypt();

		else if(choice == "Exit" || choice == "exit")
			return 0;

		else
			cout << "Invalid response" << endl << endl;
	}
}

//===============================================

void encrypt()
{
	char sourceFileName[20];
	int encryptMatrix[2][2];
	char plaintext[25000];
	char ciphertext[25000];
	int size;

// get source file name
	cout << "Enter source filename: ";
	cin >> sourceFileName;
	cout << endl;

// get encryption matrix
	cout << "Enter the encryption matrix of form: a  b : as \"a b c d\": " << endl;
	cout << "                                     c  d                 ";
	cin >> encryptMatrix[0][0];
	cin >> encryptMatrix[0][1];
	cin >> encryptMatrix[1][0];
	cin >> encryptMatrix[1][1];

// get plaintext and size of plaintext
	getEncryptPlaintext(sourceFileName, plaintext, size);

// get ciphertext
	getEncryptCiphertext(plaintext, size, ciphertext, encryptMatrix);

// write to destination file
	writeToCipherFile(ciphertext, size);
}

//===============================================

void getEncryptPlaintext(char sourceFileName[], char plaintext[], int& size)
{
	ifstream inFile;
	int i;
	char c;

// open source file
	inFile.open(sourceFileName);
	if(!inFile)
	{
		cout << "Error opening source file";
		exit(0);
	}

	else
	{
		i = 0;

// loop through the source file to encrypt
		while(!inFile.eof())
		{
			if(inFile.peek() == EOF)
				break;

// get character from file
			inFile.get(c);

// skip non-alphabetic characters
			if(c > ' ')
			{

// convert uppercase to lowercase
				if(c >= 'A' && c <= 'Z')
					c += ('a' - 'A');

				plaintext[i] = c;

				i++;
			}
		}
		size = i;
	}

// close file
	inFile.close();
}

//===============================================

void getEncryptCiphertext(char plaintext[], int& size, char ciphertext[], int encryptMatrix[][2])
{
	char c1;
	char c2;

// make an even number of chars
	if(size % 2 == 1)
	{
		plaintext[size] = 'x';
		size++;
	}

// get ciphertext
	for(int k = 0; (k + 1) < size; k += 2)
	{
		c1 = (encryptMatrix[0][0] * (plaintext[k] - 'a' + 1) +
					encryptMatrix[0][1] * (plaintext[k + 1] - 'a' + 1)) % 26 + 'a' - 1;

		c2 = (encryptMatrix[1][0] * (plaintext[k] - 'a' + 1) +
					encryptMatrix[1][1] * (plaintext[k + 1] - 'a' + 1)) % 26 + 'a' - 1;

// if ciphertext char is less than 'a' then add 26
		if(c1 < 'a')
			c1 += 26;
		if(c2 < 'a')
			c2 += 26;

		ciphertext[k] = c1;
		ciphertext[k + 1] = c2;
	}
}

//===============================================

void writeToCipherFile(char ciphertext[], int size)
{
	ofstream outFile;
	char destFileName[20];

// get destination file name
	cout << "Enter destination filename: ";
	cin >> destFileName;

// open file
	outFile.open(destFileName);
	if(!outFile)
	{
		cout << "Error opening destination file";
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
	char sourceFileName[20];
	int encryptMatrix[2][2];
	int decryptMatrix[2][2];
	char plaintext[25000];
	char ciphertext[25000];
	int size;

// get source file name
	cout << "Enter source filename: ";
	cin >> sourceFileName;
	cout << endl;

// get encryption matrix
	cout << "Enter the encryption matrix of form: a  b : as \"a b c d\": " << endl;
	cout << "                                     c  d                 ";
	cin >> encryptMatrix[0][0];
	cin >> encryptMatrix[0][1];
	cin >> encryptMatrix[1][0];
	cin >> encryptMatrix[1][1];

// get plaintext and size of plaintext
	getDecryptCiphertext(sourceFileName, ciphertext, size);

// get decryption matrix
	getDecryptMatrix(encryptMatrix, decryptMatrix);

// display decryption matirx
	cout << "Decryption Matrix is: " << endl;
	for(int row = 0; row < 2; row++)
	{
		for(int col = 0; col < 2; col++)
			cout << "  " << decryptMatrix[row][col];

		cout << endl;
	}

// get ciphertext
	getDecryptPlaintext(ciphertext, size, plaintext, decryptMatrix);

// write to destination file
	writeToPlainFile(plaintext, size);
}

//===============================================

void getDecryptCiphertext(char sourceFileName[], char ciphertext[], int& size)
{
	ifstream inFile;
	int i;
	char c;

// open source file
	inFile.open(sourceFileName);
	if(!inFile)
	{
		cout << "Error opening source file";
		exit(0);
	}

	else
	{
		i = 0;

// loop through the source file to encrypt
		while(!inFile.eof())
		{
			if(inFile.peek() == EOF)
				break;

// get character from file
			inFile.get(c);

// skip non-alphabetic characters
			if(c > ' ')
			{

// convert uppercase to lowercase
				if(c >= 'A' && c <= 'Z')
					c += ('a' - 'A');

				ciphertext[i] = c;

				i++;
			}
		}
		size = i;
	}

// close file
	inFile.close();
}

//===============================================

void getDecryptMatrix(int encryptMatrix[][2], int decryptMatrix[][2])
{
	int det;
	int reciprocal;

// find determinant
	det = (encryptMatrix[0][0] * encryptMatrix[1][1]) - (encryptMatrix[0][1] * encryptMatrix[1][0]);

// find reciprocal
	reciprocal = findReciprocal(det);

// find decryption matrix
	decryptMatrix[0][0] = (encryptMatrix[1][1] * reciprocal) % 26;
	decryptMatrix[0][1] = (-1 * encryptMatrix[0][1] * reciprocal) % 26;
	decryptMatrix[1][0] = (-1 * encryptMatrix[1][0] * reciprocal) % 26;
	decryptMatrix[1][1] = (encryptMatrix[0][0] * reciprocal) % 26;

// fix negative values
	for(int row = 0; row < 2; row++)
	{
		for(int col = 0; col < 2; col++)
		{
			if(decryptMatrix[row][col] < 0)
				decryptMatrix[row][col] += 26;
		}
	}
}

//===============================================

int findReciprocal(int det)
{
	int n = 1;

// try possible combinations
	while((det * n) % 26 != 1)
		n++;

// return the reciprocal
	return n;
}

//===============================================

void getDecryptPlaintext(char ciphertext[], int& size, char plaintext[], int decryptMatrix[][2])
{
	char p1;
	char p2;

// make an even number of chars
	if(size % 2 == 1)
	{
		ciphertext[size] = 'x';
		size++;
	}

// get plaintext
	for(int k = 0; (k + 1) < size; k += 2)
	{
		p1 = (decryptMatrix[0][0] * (ciphertext[k] - 'a' + 1) +
					decryptMatrix[0][1] * (ciphertext[k + 1] - 'a' + 1)) % 26 + 'a' - 1;

		p2 = (decryptMatrix[1][0] * (ciphertext[k] - 'a' + 1) +
					decryptMatrix[1][1] * (ciphertext[k + 1] - 'a' + 1)) % 26 + 'a' - 1;

// if plaintext char is less than 'a' then add 26
		if(p1 < 'a')
			p1 += 26;
		if(p2 < 'a')
			p2 += 26;

		plaintext[k] = p1;
		plaintext[k + 1] = p2;
	}
}

//===============================================

void writeToPlainFile(char plaintext[], int size)
{
	ofstream outFile;
	char destFileName[20];

// get destination file name
	cout << "Enter destination filename: ";
	cin >> destFileName;

// open file
	outFile.open(destFileName);
	if(!outFile)
	{
		cout << "Error opening destination file";
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