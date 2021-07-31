// Zachary Tennant
// File Name: MatrixMulti.c
// Date: 1 December, 2018
// Program Description: This program will read in two martices from a text file and if they are of the
//                      appropriate size, then they will be multiplied together. If there are more than
//                      two files, then the product of the first two matrices will be multiplied with the
//                      third and so on.

#include<stdio.h>
#include<stdlib.h>
#include<math.h>

void writeToFile(FILE *file, int matrix1Row, int matrix2Col, int productMatrix[][1000]);

	int matrix1[1000][1000];
	int matrix2[1000][1000];
	int productMatrix[1000][1000];

int main(int argc, char** argv)
{
	FILE *file;
	int matrix1Row;
	int matrix1Col;
	int matrix2Row;
	int matrix2Col;
	int tempRow;
	int tempCol;
	int product;
	char matrixStr[5000];
	char *matrixChar;
	double start, end;
	double dif;

// make sure there are the correct number of arguments
	if(argc < 3)
		printf("Wrong need at least two matrix files.\n");

	else
	{

// loop through all files
		for(int fileNum = 2; fileNum < argc; fileNum++)
		{
			if(fileNum > 2)
			{
				for(int r = 0; r < 1000; r++)
				{
					for(int c = 0; c < 1000; c++)
					{
						matrix1[r][c] = productMatrix[r][c];
						matrix2[r][c] = 0;
						productMatrix[r][c] = 0;
					}
				}
			}

			else
			{

// #################### Read in first matrix ####################

// open the first file
				file = fopen(argv[fileNum - 1], "r");
				if(file == NULL)
				{
					printf("Error opening %s.\n", argv[fileNum - 1]);
					exit(0);
				}

				else
				{
					matrix1Row = 0;

// run through the entire file
					while(!feof(file))
					{
						tempCol = 0;

// get one line
						fgets(matrixStr, 5000, file);

// get the first character out of the line
						matrixChar = (char*)strtok(matrixStr, ",");
						tempCol++;

// get the remaining characters out of the line
						while(matrixChar != NULL)
						{
							matrixChar = (char*)strtok(NULL, ",");

							if(matrixChar == NULL)
								matrix1Row++;

							tempCol++;
						}
						matrix1Col = tempCol - 1;
					} // end while on 80

// reset to start re-reading the file
					clearerr(file);
					fseek(file, 0, SEEK_SET);

					tempRow = -1;

// start re-reading the file
					while(!feof(file))
					{
						tempCol = 0;
						tempRow++;

// get a line out of the file
						fgets(matrixStr, 5000, (FILE*)file);

// get the first character of the line and convert to int
						matrixChar = (char*)strtok(matrixStr, ",");
						matrix1[tempRow][tempCol] = atoi(matrixChar);

// get the rest of the characters out of the line and convert them
						while(matrixChar != NULL)
						{
							tempCol++;

							matrixChar = (char*)strtok(NULL, ",");

							if(matrixChar != NULL)
							{
								if(tempCol < matrix1Col)
									matrix1[tempRow][tempCol] = atoi(matrixChar);
							}
						}
					} // end while on 111
				} // end else on 75

// close the file
				fclose(file);
			} // end else on 62

// #################### End read in first matrix ####################

// #################### Read in second matrix ####################

// open the next file
			file = fopen(argv[fileNum], "r");
			if(file == NULL)
			{
				printf("Error opening %s.\n", argv[fileNum]);
				exit(0);
			}

			else
			{
				matrix2Row = 0;

// run through the entire file
				while(!feof(file))
				{
					tempCol = 0;

// get one line
					fgets(matrixStr, 5000, (FILE*)file);

// get the first character out of the line
					matrixChar = (char*)strtok(matrixStr, ",");
					tempCol++;

// get the remaining characters out of the line
					while(matrixChar != NULL)
					{
						matrixChar = (char*)strtok(NULL, ",");

						if(matrixChar == NULL)
							matrix2Row++;

						tempCol++;
					}
					matrix2Col = tempCol - 1;
				} // end while on 160

// reset to start re-reading the file
				clearerr(file);
				fseek(file, 0, SEEK_SET);

				tempRow = -1;

// start re-reading the file
				while(!feof(file))
				{
					tempCol = 0;
					tempRow++;

// get a line out of the file
					fgets(matrixStr, 5000, (FILE*)file);

// get the first character of the line and convert to int
					matrixChar = (char*)strtok(matrixStr, ",");
					matrix2[tempRow][tempCol] = atoi(matrixChar);

// get the rest of the characters out of the line and convert them
					while(matrixChar != NULL)
					{
						tempCol++;

						matrixChar = (char*)strtok(NULL, ",");

						if(matrixChar != NULL)
						{
							if(tempCol < matrix2Col)
								matrix2[tempRow][tempCol] = atoi(matrixChar);
						}
					}
				} // end while on 191
			} // end else on 155

// close the file
			fclose(file);

// #################### End read in second matrix ####################

// #################### Begin multiplication ####################

// see if the two matrices work for multiplication
			if(matrix1Col != matrix2Row)
			{
				printf("Can't perform multiplication.\n");

				if(fileNum > 2)
					printf("Problem with file %s.\n", argv[fileNum]);

				printf("First matrix number of columns has to be same as second matrix number of rows.\n");
			}

			else
			{

// start timer
				start = clock();

// perform multiplication
				for(int r = 0; r < matrix1Row; r++)
				{
					for(int i = 0; i < matrix2Col; i++)
					{
						product = 0;

						for(int k = 0; k < matrix1Col; k++)
							product = product + matrix1[r][k] * matrix2[k][i];

						productMatrix[r][i] = product;
					}
				}

				writeToFile(file, matrix1Row, matrix2Col, productMatrix);
			} // end else on 237
		} // end for on 47

// end timer
			end = clock();

// calculate and display elapsed time
			dif = (double)(end - start) / 1000;
			printf ("Elasped time is %.2lf seconds.\n", dif );
	} // end else on 43

	return 0;
}

// #################### End multiplication ####################

// #################### Begin writing to file ####################

void writeToFile(FILE *file, int matrix1Row, int matrix2Col, int productMatrix[][1000])
{
// open output file
	file = fopen("Matrix_Product.csv", "w");
	if(file == NULL)
	{
		printf("Error opening Matrix_Product.csv.\n");
		exit(0);
	}

// write to output file
	else
	{
		for(int r = 0; r < matrix1Row; r++)
		{
			for(int c = 0; c < matrix2Col; c++)
			{
				if(c == matrix2Col - 1)
					fprintf(file, "%d", productMatrix[r][c]);
				else
					fprintf(file, "%d,", productMatrix[r][c]);
			}
			if(r != matrix1Row - 1)
				fprintf(file, "\n");
		}
	}

// close output file
	fclose(file);
}