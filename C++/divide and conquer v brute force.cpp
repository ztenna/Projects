#include<iostream>
#include<fstream>
#include<string>
#include<chrono>
#include<algorithm>
#include<vector>
using namespace std;
using namespace std::chrono;

int bruteForce(vector<int>& subsequenceArr, int startPos, int endPos, int k, int size, int& begIndex, int& endIndex);
int divideAndConquer(vector<int> subsequenceArr, int k, int size, int& largestAvg, int& largestAvgBegIndex, int& largestAvgEndIndex);
void display(int k, int size, int begIndex, int endIndex, int average, auto time);

int main(int argc, char* argv[])
{
	ifstream inFile;
	string file;
	int k;
	string length;
	int size;
	int i;
	char digit;
	string num;
	int average;
	int largestAvg = 0;
	int begIndex = 0;
	int endIndex = 0;
	int largestAvgBegIndex = 0;
	int largestAvgEndIndex = 0;
	string algorithm;

// validating the command arguments
	if (argc < 4 || argc > 4)
	{
		cout << "Must have 4 arguments (program name, input file name, subsequence length, and -b for brute-force or -d for divide and conquer algorithm.";
		exit(1);
	}
	file = argv[1];

	//k = 0;
	size = 0;
	for (int s = 0; s < 10; s++)
	{
		//k += 20;
		size += 100000;

// open input file and check for error
		inFile.open(file.c_str());
		if (!inFile)
		{
			cout << "Error opening " << argv[1];
			exit(1);
		}
		else
		{

// check subsequence length
			k = atoi(argv[2]);
			getline(inFile, length);
			//size = stoi(length);

			if (size < k || k < 2)
			{
				cout << "Subsequence length must be between 2 and " << size << ".";
				exit(1);
			}
			else
			{
				i = 0;
				num = "";
				algorithm = argv[3];

				vector<int> subsequenceArr(size, 0);

// create subsequence array
				while (i < size)
				{
					digit = inFile.get();
					if (digit != ' ' && digit != '\n' && digit != '\r')
						num += digit;

					if (digit == ' ')
					{
						subsequenceArr.at(i) = stoi(num);
						num = "";
						i++;
					}
				}

// see which algorithm to use
				if (algorithm == "-b")
				{

// use the brute force algorithm
					auto start = chrono::steady_clock::now();
					average = bruteForce(subsequenceArr, 0, (size - k), k, size, begIndex, endIndex);
					auto stop = chrono::steady_clock::now();
					auto duration = stop - start;
					display(k, size, begIndex, endIndex, average, duration);
				}
				else if (algorithm == "-d")
				{

// make sure the divide and conquer algorithm can be used
					if (k > (size / 2))
					{
						cout << "You can't use the divide and conquer method with a subsequence length greater than " << size / 2 << ".";
						exit(1);
					}

// use the divide and conquer algorithm
					else
					{
						auto start = chrono::steady_clock::now();
						average = divideAndConquer(subsequenceArr, k, size, largestAvg, largestAvgBegIndex, largestAvgEndIndex);
						auto stop = chrono::steady_clock::now();
						auto duration = stop - start;
						display(k, size, largestAvgBegIndex, largestAvgEndIndex, average, duration);
					}
				}
				else
					cout << "Algorithm choice must be either -b or -d.";
			}
		}
// close input file
		inFile.close();
	}
}

//===================================================================

int bruteForce(vector<int>& subsequenceArr, int startPos, int endPos, int k, int size, int& begIndex, int& endIndex)
{
	int largestAvg = 0;
	int average = 0;

	if (startPos == endPos)
	{
		for (int j = 0; j < k; j++)
			average += subsequenceArr.at(startPos+j);

		largestAvg = average / k;
		begIndex = startPos;
		endIndex = startPos + k - 1;
	}
	else
	{
		for (int i = startPos; i < endPos; i++)
		{
// find current average
			for (int j = 0; j < k; j++)
				average += subsequenceArr.at(i+j);

// see if largest average
			if (average > largestAvg)
			{
				largestAvg = average;
				begIndex = i;
				endIndex = i + k - 1;
			}
			average = 0;
		}
	}

	return largestAvg;
}

//===================================================================

int divideAndConquer(vector<int> subsequenceArr, int k, int size, int& largestAvg, int& largestAvgBegIndex, int& largestAvgEndIndex)
{
	int leftAvg;
	int rightAvg;
	int a;
	int tempAvg = 0;

// see if array needs divided
	if (size / 2 > k)
	{

// since being divided, create a left and right sub-arrays
		vector<int> leftArr((size/2)+1, 0);
		vector<int> rightArr((size/2)+1, 0);

		for (int i = 0; i < size/2; i++)
			leftArr.at(i) = subsequenceArr.at(i);

		a = 0;
		for (int i = size/2; i < size; i++)
		{
			rightArr.at(a) = subsequenceArr.at(i);
			a++;
		}

// find largest average of left array
		leftAvg = divideAndConquer(leftArr, k, (size/2)+1, largestAvg, largestAvgBegIndex, largestAvgEndIndex);

// find largest average of right array
		rightAvg = divideAndConquer(rightArr, k, (size/2)+1, largestAvg, largestAvgBegIndex, largestAvgEndIndex);

// see if left or right are the new largest avg
		if (leftAvg > rightAvg && leftAvg > largestAvg)
			largestAvg = leftAvg;

		else if (rightAvg > largestAvg)
			largestAvg = rightAvg;

// find largest average of middle of left and right arrays
		for (int i = (size/2)-k+1; i <= (size/2)-1; i++)
		{
			for (int a = 0; a < k; a++)
				tempAvg += subsequenceArr.at(a + i);

			if (tempAvg > largestAvg)
			{
				largestAvgBegIndex = i;
				largestAvgEndIndex = i + k - 1;
				largestAvg = tempAvg;
			}

			tempAvg = 0;
		}

		return largestAvg;
	}

// find largest average of an array
	else
	{
		for (int i = 0; i <= size-k; i++)
		{
			for (int a = 0; a < k; a++)
				tempAvg += subsequenceArr.at(a + i);

			if (tempAvg > largestAvg)
			{
				largestAvgBegIndex = i;
				largestAvgEndIndex = i + (k-1);
				largestAvg = tempAvg;
			}

			tempAvg = 0;
		}

		return largestAvg;
	}
}

//===================================================================

void display(int k, int size, int begIndex, int endIndex, int average, auto time)
{
	average = average / k;

	cout << "k: " << k << endl;
	cout << "n: " << size << endl;
	cout << "Beginning Index: " << begIndex << endl;
	cout << "Ending Index: " << endIndex << endl;
	cout << "Average: " << average << endl;
	cout << "Time for Execution: " << chrono::duration_cast<chrono::milliseconds>(time).count() << " milliseconds" << endl;
}